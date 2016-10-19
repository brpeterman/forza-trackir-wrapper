using SlimDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackIRUnity;

namespace ForzaTrackIR
{
    public partial class Monitor : Form
    {
        #region private constants
        private const int POLL_INTERVAL = 5;
        #endregion

        #region private fields
        private FakeController _controller;
        private CancellationTokenSource _pollToken;
        private CancellationTokenSource _probeToken;
        private Dictionary<string, Joystick> _controllers;
        #endregion

        public Monitor(FakeController controller)
        {
            this.FormClosing += this.Monitor_FormClosing;

            _controller = controller;
            _controller.WindowHandle = this;
            _controller.TrackIR.UpdateHandler += HandleTrackIRUpdate;
            _controller.TrackIR.Connected += TrackIRConnected;
            _controller.TrackIR.Disconnected += TrackIRDisconnected;
            InitializeComponent();
            
            PopulateControllers();

            ProbeTrackIR();
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                ClearJoysticks();

                if (_pollToken != null)
                {
                    _pollToken.Dispose();
                }

                if (_probeToken != null)
                {
                    _probeToken.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        private void Monitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopPolling();
        }

        private void ClearJoysticks()
        {
            foreach (Joystick joystick in _controllers.Values)
            {
                joystick.Unacquire();
                joystick.Dispose();
            }
        }

        private void BeginPolling()
        {
            if (_pollToken != null)
            {
                _pollToken.Dispose();
            }
            _pollToken = new CancellationTokenSource();
            CancellationToken token = _pollToken.Token;
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    _controller.Poll();
                    Thread.Sleep(POLL_INTERVAL);
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                }
            });
        }

        private void ProbeTrackIR()
        {
            if (_probeToken != null)
            {
                _probeToken.Dispose();
            }
            _probeToken = new CancellationTokenSource();
            CancellationToken token = _probeToken.Token;
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    _controller.TrackIR.Probe();
                    Thread.Sleep(1000);
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                }
            });
        }

        private void PopulateControllers()
        {
            if (_controllers != null)
            {
                ClearJoysticks();
            }

            _controllers = new Dictionary<string, Joystick>();
            DirectInput directInput = new DirectInput();

            int index = 1;
            foreach(DeviceInstance deviceInstance in directInput.GetDevices())
            {
                Joystick device = new Joystick(directInput, deviceInstance.InstanceGuid);
                if ((device.Information.ProductGuid.ToString() == "028e045e-0000-0000-0000-504944564944") ||                           // Emulated controller
                   (device.Capabilities.ButtonCount < 1 || device.Capabilities.AxesCount < 1) ||                                       // No axes or buttons
                   (device.Information.Type != DeviceType.Gamepad && device.Information.UsageId != SlimDX.Multimedia.UsageId.Gamepad)) // Not a gamepad
                {
                    device.Dispose();
                    continue;
                }

                _controllers.Add("[" + index + "] " + device.Information.ProductName, device);
            }
            directInput.Dispose();

            cboControllers.DataSource = new BindingSource(_controllers, null);
            cboControllers.DisplayMember = "Key";
            cboControllers.ValueMember = "Value";
        }

        private void StopPolling()
        {
            if (_pollToken != null)
            {
                _pollToken.Cancel();
            }
        }

        private void HandleTrackIRUpdate(TrackIRClient.LPTRACKIRDATA state)
        {
            try {
                
            }
            catch (Exception)
            {
                // thread is no longer active, so just quit
            }
        }

        private void TrackIRConnected()
        {
            Invoke(new Action(() =>
            {
                lblTrackIR.Text = "Connected";
                btnStartStop.Enabled = true;
            }));
        }
        
        private void TrackIRDisconnected()
        {
            Invoke(new Action(() =>
            {
                lblTrackIR.Text = "Disconnected";
                btnStartStop.Enabled = false;
            }));
        }

        private void HandleControllerUpdate(JoystickState state)
        {
            
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (_controller.IsActive)
            {
                Stop();
            }
            else
            {
                Start();
            }
        }

        private void Stop()
        {
            StopPolling();
            _controller.Stop();
            btnStartStop.Text = "Start";
            cboControllers.Enabled = true;
            btnRefresh.Enabled = true;
            ProbeTrackIR();
        }

        private void Start()
        {
            _probeToken.Cancel();
            _controller.Device = (Joystick) cboControllers.SelectedValue;
            _controller.Start();
            _controller.Controller.UpdateHandler += HandleControllerUpdate;
            BeginPolling();
            btnStartStop.Text = "Stop";
            cboControllers.Enabled = false;
            btnRefresh.Enabled = false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateControllers();
        }
    }
}
