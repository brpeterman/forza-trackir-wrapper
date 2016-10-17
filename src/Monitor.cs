using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.XInput;
using TrackIRUnity;

namespace ForzaTrackIR
{
    public partial class Monitor : Form
    {
        #region private constants
        private const int POLL_INTERVAL = 10;
        #endregion

        #region private fields
        FakeController _controller;
        CancellationTokenSource _cancellationToken;
        #endregion

        public Monitor(FakeController controller)
        {
            _controller = controller;
            _controller.TrackIR.UpdateHandler += HandleTrackIRUpdate;
            _controller.Controller.UpdateHandler += HandleControllerUpdate;
            InitializeComponent();
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
                    lblPitch.Text = TrackIRWrapper.ToDegrees(state.fNPPitch).ToString();
                    lblYaw.Text = TrackIRWrapper.ToDegrees(state.fNPYaw).ToString();
                }));
            }
            catch (Exception e)
            {
                // thread is no longer active, so just quit
            }
        }

        private void HandleControllerUpdate(State state)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    GamepadButtonFlags buttons = state.Gamepad.Buttons;
                    lblButtonA.Visible = (buttons & GamepadButtonFlags.A) != 0;
                    lblButtonB.Visible = (buttons & GamepadButtonFlags.B) != 0;
                    lblButtonX.Visible = (buttons & GamepadButtonFlags.X) != 0;
                    lblButtonY.Visible = (buttons & GamepadButtonFlags.Y) != 0;
                }));
            }
            catch(Exception e)
            {
                // thread is no longer active, so just quit
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (_controller.IsActive)
            {
                StopPolling();
                _controller.Stop();
            }
            else
            {
                _controller.Start();
                BeginPolling();
            }
        }
    }
}
