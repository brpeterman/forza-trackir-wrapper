using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackIRUnity;
using SharpDX.XInput;
using System.Threading;

namespace ForzaTrackIR
{
    public class FakeController : ScpDevice
    {
        #region private constants
        private const string BUS_CLASS_GUID = "{F679F562-3164-42CE-A4DB-E7DDBE723909}";
        private const int MAX_STICK = short.MaxValue;
        #endregion

        #region Private fields
        State _trackIRState;
        State _controllerState;
        #endregion

        #region Public fields
        public TrackIRWrapper TrackIR
        {
            get;
            private set;
        }

        public ControllerWrapper Controller
        {
            get;
            private set;
        }
        #endregion

        #region Delegates
        public delegate void TrackIRUpdate(TrackIRClient.LPTRACKIRDATA state);

        public TrackIRUpdate UpdateHandler;
        #endregion

        #region Constructors

        public FakeController() : base(BUS_CLASS_GUID)
        {
            TrackIR = new TrackIRWrapper();
            Controller = new ControllerWrapper();

            TrackIR.UpdateHandler += HandleTrackIRUpdate;
            Controller.UpdateHandler += HandleControllerUpdate;
        }

        #endregion

        #region Public methods

        public void Poll()
        {
            TrackIR.Poll();
            Controller.Poll();
            UpdateControllerState();
        }

        #endregion

        #region Private methods

        private void HandleTrackIRUpdate(TrackIRClient.LPTRACKIRDATA state)
        {
            // Resolve yaw to left stick X and Y positions
            // -180 = full right, 180 = full left
            // We only care about the forward 120 degrees
            double yaw = TrackIRWrapper.ToDegrees(state.fNPYaw);
            if (yaw > 60 || yaw < -60) return;

            yaw += 90; // Move phase for trig calculations

            short stickX = (short) (Math.Cos(yaw * (Math.PI/180)) * MAX_STICK);
            short stickY = (short) (Math.Sin(yaw * (Math.PI / 180)) * MAX_STICK);

            State newState = new State();
            newState.Gamepad.RightThumbX = stickX;
            newState.Gamepad.RightThumbY = stickY;

            _trackIRState = newState;
        }

        private void HandleControllerUpdate(State state)
        {
            // Pass through to ScpDevice unless it's the right stick
            _controllerState = state;
        }

        private void UpdateControllerState()
        {
            byte[] input = new byte[28];
            byte[] output = new byte[28];
            int transferred = 0;
            State state = _controllerState;
            state.Gamepad.RightThumbX = _trackIRState.Gamepad.RightThumbX;
            state.Gamepad.RightThumbY = _trackIRState.Gamepad.RightThumbY;
            
            ParseState(state, ref output);

            DeviceIoControl(m_FileHandle, 0x2A400C, output, output.Length, output, output.Length, ref transferred, IntPtr.Zero);
        }

        private void ParseState(State state, ref byte[] output)
        {
            byte serial = 1;

            for (int index = 0; index < 28; index++) output[index] = 0x00;

            output[0] = 0x1C;
            output[4] = serial;
            output[9] = 0x14;

            GamepadButtonFlags buttons = state.Gamepad.Buttons;

            if ((buttons & GamepadButtonFlags.Back) > 0) output[10] |= (byte)(1 << 5); // Back
            if ((buttons & GamepadButtonFlags.LeftThumb) > 0) output[10] |= (byte)(1 << 6); // Left  Thumb
            if ((buttons & GamepadButtonFlags.RightThumb) > 0) output[10] |= (byte)(1 << 7); // Right Thumb
            if ((buttons & GamepadButtonFlags.Start) > 0) output[10] |= (byte)(1 << 4); // Start

            if ((buttons & GamepadButtonFlags.DPadUp) > 0) output[10] |= (byte)(1 << 0); // Up
            if ((buttons & GamepadButtonFlags.DPadRight) > 0) output[10] |= (byte)(1 << 3); // Right
            if ((buttons & GamepadButtonFlags.DPadDown) > 0) output[10] |= (byte)(1 << 1); // Down
            if ((buttons & GamepadButtonFlags.DPadLeft) > 0) output[10] |= (byte)(1 << 2); // Left

            if ((buttons & GamepadButtonFlags.LeftShoulder) > 0) output[11] |= (byte)(1 << 0); // Left  Shoulder
            if ((buttons & GamepadButtonFlags.RightShoulder) > 0) output[11] |= (byte)(1 << 1); // Right Shoulder

            if ((buttons & GamepadButtonFlags.Y) > 0) output[11] |= (byte)(1 << 7); // Y
            if ((buttons & GamepadButtonFlags.B) > 0) output[11] |= (byte)(1 << 5); // B
            if ((buttons & GamepadButtonFlags.A) > 0) output[11] |= (byte)(1 << 4); // A
            if ((buttons & GamepadButtonFlags.X) > 0) output[11] |= (byte)(1 << 6); // X

            output[12] = state.Gamepad.LeftTrigger; // Left Trigger
            output[13] = state.Gamepad.RightTrigger; // Right Trigger

            int ThumbLX = state.Gamepad.LeftThumbX;
            int ThumbLY = state.Gamepad.LeftThumbY;
            int ThumbRX = state.Gamepad.RightThumbX;
            int ThumbRY = state.Gamepad.RightThumbY;

            output[14] = (byte)((ThumbLX >> 0) & 0xFF); // LX
            output[15] = (byte)((ThumbLX >> 8) & 0xFF);

            output[16] = (byte)((ThumbLY >> 0) & 0xFF); // LY
            output[17] = (byte)((ThumbLY >> 8) & 0xFF);

            output[18] = (byte)((ThumbRX >> 0) & 0xFF); // RX
            output[19] = (byte)((ThumbRX >> 8) & 0xFF);

            output[20] = (byte)((ThumbRY >> 0) & 0xFF); // RY
            output[21] = (byte)((ThumbRY >> 8) & 0xFF);
        }

        private int GetNextSlot()
        {
            return 1;
        }

        private bool PlugIn(int serial)
        {
            if (IsActive)
            {
                int transferred = 0;
                byte[] buffer = new byte[16];

                buffer[0] = 0x10;
                buffer[1] = 0x00;
                buffer[2] = 0x00;
                buffer[3] = 0x00;

                buffer[4] = (byte)((serial >> 0) & 0xFF);
                buffer[5] = (byte)((serial >> 8) & 0xFF);
                buffer[6] = (byte)((serial >> 16) & 0xFF);
                buffer[7] = (byte)((serial >> 24) & 0xFF);

                return DeviceIoControl(m_FileHandle, 0x2A4000, buffer, buffer.Length, null, 0, ref transferred, IntPtr.Zero);
            }

            return false;
        }

        private bool Unplug(int serial)
        {
            if (IsActive)
            {
                int transferred = 0;
                byte[] buffer = new byte[16];

                buffer[0] = 0x10;
                buffer[1] = 0x00;
                buffer[2] = 0x00;
                buffer[3] = 0x00;

                buffer[4] = (byte)((serial >> 0) & 0xFF);
                buffer[5] = (byte)((serial >> 8) & 0xFF);
                buffer[6] = (byte)((serial >> 16) & 0xFF);
                buffer[7] = (byte)((serial >> 24) & 0xFF);

                return DeviceIoControl(m_FileHandle, 0x2A4004, buffer, buffer.Length, null, 0, ref transferred, IntPtr.Zero);
            }

            return false;
        }

        #endregion

        #region ScpDevice overrides

        public override bool Open(string DevicePath)
        {
            m_Path = DevicePath;
            m_WinUsbHandle = (IntPtr)INVALID_HANDLE_VALUE;

            if (GetDeviceHandle(m_Path))
            {
                m_IsActive = true;
            }
            return true;
        }

        public override bool Start()
        {
            Open();
            PlugIn(GetNextSlot());
            return base.Start();
        }

        public override bool Stop()
        {
            Unplug(1);
            return true;
        }

        #endregion
    }
}
