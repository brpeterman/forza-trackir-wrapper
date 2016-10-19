using SlimDX.DirectInput;
using System;
using System.Windows.Forms;

namespace ForzaTrackIR
{
    public class ControllerWrapper
    {

        #region private fields
        private Joystick _controller;
        private Control _handle;
        #endregion

        #region Delegates
        public delegate void StateChanged(JoystickState state);

        public StateChanged UpdateHandler;
        #endregion

        #region constructors
        public ControllerWrapper(Joystick device, Control windowHandle)
        {
            _controller = device;
            _handle = windowHandle;
            AcquireController();
        }
        #endregion

        #region Destructor
        ~ControllerWrapper()
        {
            ReleaseController();
        }
        #endregion

        #region public methods
        public void Poll()
        {

            JoystickState state = _controller.GetCurrentState();
            if (UpdateHandler != null)
            {
                UpdateHandler(state);
            }
        }

        public Buttons GetButtonMappings()
        {
            if (_controller.Information.ProductName == "Wireless Controller" &&
                _controller.Information.Type == DeviceType.FirstPerson &&
                _controller.Information.Subtype == 259)
            {
                return new DS4Buttons();
            }

            return null;
        }
        #endregion

        #region private methods
        private bool AcquireController()
        {
            _controller.SetCooperativeLevel(_handle, CooperativeLevel.Exclusive | CooperativeLevel.Background);
            _controller.Acquire();
            return true;
        }

        private bool ReleaseController()
        {

            try
            {
                _controller.Unacquire();
            }
            catch (NullReferenceException)
            {
                // If we get a NullReferenceException, the controller has already been disposed of.
            }
            return true;
        }
        #endregion
    }
}
