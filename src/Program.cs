using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForzaTrackIR
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            FakeController controller = new FakeController();
            controller.TrackIR.UpdateHandler += HandleTrackIRUpdate;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Monitor(controller));
        }

        static void HandleTrackIRUpdate(TrackIRUnity.TrackIRClient.LPTRACKIRDATA state)
        {
            Console.WriteLine("--------------------");
            Console.WriteLine(TrackIRDataToString(state));
        }

        static string TrackIRDataToString(TrackIRUnity.TrackIRClient.LPTRACKIRDATA state)
        {
            string output = "";
            output += "Pitch: " + state.fNPPitch + "\n";
            output += "Yaw: " + state.fNPYaw + "\n";
            return output;
        }
    }
}
