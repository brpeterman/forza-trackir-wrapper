using SlimDX.DirectInput;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackIRUnity;
using System.Collections.Generic;

namespace ForzaTrackIR
{
    public partial class Monitor : Form
    {
        #region private constants
        private const int POLL_INTERVAL = 5;
        #endregion

        #region private fields
        private FakeController _controller;
        private CancellationTokenSource _cancellationToken;
        private DirectInput _directInput;
        private Dictionary<string, Joystick> _controllers;
        #endregion

        public Monitor(FakeController controller)
        {
            _controller = controller;
            _controller.WindowHandle = this;
            _controller.TrackIR.UpdateHandler += HandleTrackIRUpdate;
            InitializeComponent();

            _directInput = new DirectInput();
            PopulateControllers();
        }

        private void Monitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopPolling();
        }

        private void BeginPolling()
        {
            _cancellationToken = new CancellationTokenSource();
            CancellationToken token = _cancellationToken.Token;
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

        private void PopulateControllers()
        {
            _controllers = new Dictionary<string, Joystick>();

            int index = 1;
            foreach(DeviceInstance deviceInstance in _directInput.GetDevices())
            {
                Joystick device = new Joystick(_directInput, deviceInstance.InstanceGuid);
                if (device.Information.ProductGuid.ToString() == "028e045e-0000-0000-0000-504944564944") //If it's an emulated controller skip it
                    continue;

                if (device.Capabilities.ButtonCount < 1 || device.Capabilities.AxesCount < 1)
                    continue;

                if (device.Information.Type != DeviceType.Gamepad && device.Information.UsageId != SlimDX.Multimedia.UsageId.Gamepad)
                    continue;

                _controllers.Add("[" + index + "] " + device.Information.ProductName, device);
            }

            cboControllers.DataSource = new BindingSource(_controllers, null);
            cboControllers.DisplayMember = "Key";
            cboControllers.ValueMember = "Value";
        }

        private void StopPolling()
        {
            if (_cancellationToken != null)
            {
                _cancellationToken.Cancel();
            }
        }

        private void HandleTrackIRUpdate(TrackIRClient.LPTRACKIRDATA state)
        {
            try {
                Invoke(new Action(() =>
                {

                }));
            }
            catch (Exception e)
            {
                // thread is no longer active, so just quit
            }
        }

        private void HandleControllerUpdate(JoystickState state)
        {
            int[] povs = state.GetPointOfViewControllers();
            List<string> vals = new List<string>();
            for (int i = 0; i < povs.Length; i++)
            {
                vals.Add(i.ToString() + ":" + povs[i]);
            }
            Invoke(new Action(() =>
            {
                lblTest.Text = String.Join(", ", vals);
            }));
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
        }

        private void Start()
        {
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
