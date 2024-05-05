using System;
using System.Threading;
using System.Windows.Forms;

namespace QModReloadedGUI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var mutex = new Mutex(false, "QModManagerReloaded");
            try
            {
                if (mutex.WaitOne(0, false))
                {
                    var settings = Settings.FromJsonFile();
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FrmMain(settings));
                }
                else
                {
                    MessageBox.Show(@"QMod Manager Reloaded is already running! Checked the tray?", @"One!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    mutex.GetAccessControl();
                }
            }
            finally
            {
                if (true)
                {
                    mutex.Close();
                }
            }
        }
    }
}