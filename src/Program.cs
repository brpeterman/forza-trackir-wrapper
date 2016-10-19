using System;
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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Monitor(controller));
        }
    }
}
