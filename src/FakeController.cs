using SlimDX.DirectInput;
using System;
using System.Windows.Forms;
using TrackIRUnity;

namespace ForzaTrackIR
{
    public class FakeController : ScpDevice
    {
        #region private constants
        private const string BUS_CLASS_GUID = "{F679F562-3164-42CE-A4DB-E7DDBE723909}";
        #endregion

        #region Private fields
        ControllerState _trackIRState;
        ControllerState _controllerState;
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

        private Joystick _device;
        public Joystick Device
        {
            get
            {
                return _device;
            }
            set
            {
                _device = value;
                Controller = new ControllerWrapper(_device, WindowHandle);
                Controller.UpdateHandler += HandleControllerUpdate;
            }
        }
        public Control WindowHandle
        {
            get; set;
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

            TrackIR.UpdateHandler += HandleTrackIRUpdate;
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

            // Move phase for trig calculations
            // This isn't *really* necessary, but to me cos is X and sin is Y.
            yaw += 90;

            // full left:  X = 0,            Y = ushort.max/2
            // full right: X = ushort.max,   Y = ushort.max/2
            // ahead:      X = ushort.max/2, Y = ushort.max
            short stickX = (short) ((Math.Cos(yaw * (Math.PI / 180)) * short.MaxValue));
            short stickY = (short) ((Math.Sin(yaw * (Math.PI / 180)) * ushort.MaxValue));

            ControllerState newState = new ControllerState();
            newState.Z = stickX;
            newState.RotationZ = stickY;

            _trackIRState = newState;
        }

        private void HandleControllerUpdate(JoystickState state)
        {
            // Pass through to ScpDevice unless it's the right stick
            _controllerState = new ControllerState(state);
        }

        private void UpdateControllerState()
        {
            byte[] input = new byte[28];
            byte[] output = new byte[28];
            int transferred = 0;
            ControllerState state = _controllerState;
            if (_trackIRState != null)
            {
                state.Z = _trackIRState.Z;
                state.RotationZ = _trackIRState.RotationZ;
            }
            
            ParseState(state, ref output);

            DeviceIoControl(m_FileHandle, 0x2A400C, output, output.Length, output, output.Length, ref transferred, IntPtr.Zero);
        }

        /// <summary>
        /// Convert joystick + trackIR state to Xbox controller state.
        /// Conventions:
        /// Z and RotZ: right stick, replace with TrackIR input
        /// X and Y:    left stick
        /// </summary>
        /// <param name="state"></param>
        /// <param name="output"></param>
        private void ParseState(ControllerState state, ref byte[] output)
        {
            byte serial = 1;

            for (int index = 0; index < 28; index++) output[index] = 0x00;

            output[0] = 0x1C;
            output[4] = serial;
            output[9] = 0x14;

            bool[] buttons = state.GetButtons();

            Buttons mappings = Controller.GetButtonMappings();
            if (mappings == null) return;

            if (buttons[mappings.GetButton(Buttons.ButtonName.Back)]) output[10] |= (byte)(1 << 5); // Back
            if (buttons[mappings.GetButton(Buttons.ButtonName.LS)]) output[10] |= (byte)(1 << 6); // Left  Thumb
            if (buttons[mappings.GetButton(Buttons.ButtonName.RS)]) output[10] |= (byte)(1 << 7); // Right Thumb
            if (buttons[mappings.GetButton(Buttons.ButtonName.Start)]) output[10] |= (byte)(1 << 4); // Start

            Buttons.DPad dpad = GetDPadButtons(state);

            if ((dpad & Buttons.DPad.Up) > 0) output[10] |= (byte)(1 << 0); // Up
            if ((dpad & Buttons.DPad.Right) > 0) output[10] |= (byte)(1 << 3); // Right
            if ((dpad & Buttons.DPad.Down) > 0) output[10] |= (byte)(1 << 1); // Down
            if ((dpad & Buttons.DPad.Left) > 0) output[10] |= (byte)(1 << 2); // Left

            if (buttons[mappings.GetButton(Buttons.ButtonName.LB)]) output[11] |= (byte)(1 << 0); // Left  Shoulder
            if (buttons[mappings.GetButton(Buttons.ButtonName.RB)]) output[11] |= (byte)(1 << 1); // Right Shoulder

            if (buttons[mappings.GetButton(Buttons.ButtonName.Y)]) output[11] |= (byte)(1 << 7); // Y
            if (buttons[mappings.GetButton(Buttons.ButtonName.B)]) output[11] |= (byte)(1 << 5); // B
            if (buttons[mappings.GetButton(Buttons.ButtonName.A)]) output[11] |= (byte)(1 << 4); // A
            if (buttons[mappings.GetButton(Buttons.ButtonName.X)]) output[11] |= (byte)(1 << 6); // X

            if (buttons[mappings.GetButton(Buttons.ButtonName.Guide)]) output[11] |= (byte)(1 << 2); // Guide
            
            output[12] = (byte) (state.RotationX >> 8); // Left Trigger
            output[13] = (byte) (state.RotationY >> 8); // Right Trigger

            int ThumbLX = state.X - short.MinValue;
            int ThumbLY = -state.Y + short.MaxValue;
            int ThumbRX = state.Z;
            int ThumbRY = state.RotationZ + short.MaxValue;

            output[14] = (byte)((ThumbLX >> 0) & 0xFF); // LX
            output[15] = (byte)((ThumbLX >> 8) & 0xFF);

            output[16] = (byte)((ThumbLY >> 0) & 0xFF); // LY
            output[17] = (byte)((ThumbLY >> 8) & 0xFF);

            output[18] = (byte)((ThumbRX >> 0) & 0xFF); // RX
            output[19] = (byte)((ThumbRX >> 8) & 0xFF);

            output[20] = (byte)((ThumbRY >> 0) & 0xFF); // RY
            output[21] = (byte)((ThumbRY >> 8) & 0xFF);
        }

        private Buttons.DPad GetDPadButtons(ControllerState state)
        {
            Buttons.DPad pressed = Buttons.DPad.None;
            switch(state.GetPointOfViewControllers()[0])
            {
                case 0:
                    pressed = Buttons.DPad.Up;
                    break;
                case 4500:
                    pressed = Buttons.DPad.Up | Buttons.DPad.Right;
                    break;
                case 9000:
                    pressed = Buttons.DPad.Right;
                    break;
                case 13500:
                    pressed = Buttons.DPad.Down | Buttons.DPad.Right;
                    break;
                case 18000:
                    pressed = Buttons.DPad.Down;
                    break;
                case 22500:
                    pressed = Buttons.DPad.Down | Buttons.DPad.Left;
                    break;
                case 27000:
                    pressed = Buttons.DPad.Left;
                    break;
                case 31500:
                    pressed = Buttons.DPad.Up | Buttons.DPad.Left;
                    break;
            }
            return pressed;
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
