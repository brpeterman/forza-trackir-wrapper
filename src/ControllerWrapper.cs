using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.XInput;

namespace ForzaTrackIR
{
    public class ControllerWrapper
    {

        #region private fields
        private Controller _controller;
        #endregion

        #region Delegates
        public delegate void StateChanged(State state);

        public StateChanged UpdateHandler;
        #endregion

        #region constructors
        public ControllerWrapper()
        {
            _controller = GetController();
        }
        #endregion

        #region public methods
        public void Poll()
        {

            State state = _controller.GetState();
            if (UpdateHandler != null)
            {
                UpdateHandler(state);
            }
        }
        #endregion

        #region private methods

        /// <summary>
        /// Get the first XInput controller registered in Windows.
        /// Throws an Exception if said controller is not connected.
        /// </summary>
        /// <returns></returns>
        private Controller GetController()
        {
            Controller controller = new Controller(UserIndex.One);
            if (!controller.IsConnected)
            {
                throw new Exception("No XInput controller found");
            }
            return controller;
        }

        #endregion
    }
}
