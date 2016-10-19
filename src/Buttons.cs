using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForzaTrackIR
{

    public abstract class Buttons
    {
        [Flags]
        public enum DPad
        {
            None = -1,
            Up = 1,
            Right = 2,
            Down = 4,
            Left = 8
        }

        public enum ButtonName
        {
            X,
            A,
            B,
            Y,
            LB,
            RB,
            LT,
            RT,
            Back,
            Start,
            LS,
            RS,
            Guide
        }

        public abstract int GetButton(ButtonName button);
    }

    public class DS4Buttons : Buttons
    {
        public override int GetButton(ButtonName button)
        {
            switch (button) {
                case ButtonName.X:
                    return 0;
                case ButtonName.A:
                    return 1;
                case ButtonName.B:
                    return 2;
                case ButtonName.Y:
                    return 3;
                case ButtonName.LB:
                    return 4;
                case ButtonName.RB:
                    return 5;
                case ButtonName.LT:
                    return 6;
                case ButtonName.RT:
                    return 7;
                case ButtonName.Back:
                    return 8;
                case ButtonName.Start:
                    return 9;
                case ButtonName.LS:
                    return 10;
                case ButtonName.RS:
                    return 11;
                case ButtonName.Guide:
                    return 12;
                default:
                    return -1;
            }
        }
    }
}