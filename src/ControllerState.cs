using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.DirectInput;
using System.Reflection;

namespace ForzaTrackIR
{
    /// <summary>
    /// This class exists so that we can load some fake values into the properties we need.
    /// It's otherwise identical to JoystickState.
    /// </summary>
    public class ControllerState : JoystickState
    {
        private bool[] _buttons;
        private int[] _hats;
        
        public new int X { get; private set; }
        public new int Y { get; private set; }
        public new int Z { get; set; }
        public new int RotationX { get; private set; }
        public new int RotationY { get; private set; }
        public new int RotationZ { get; set; }

        public ControllerState() : base()
        {

        }

        public ControllerState(JoystickState state) : this()
        {
            _buttons = state.GetButtons();
            _hats = state.GetPointOfViewControllers();
            X = state.X;
            Y = state.Y;
            RotationX = state.RotationX;
            RotationY = state.RotationY;

        }
        
        public new bool[] GetButtons()
        {
            return _buttons;
        }

        public new int[] GetPointOfViewControllers()
        {
            return _hats;
        }
    }
}
