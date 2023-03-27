using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace XeRaVaoCong
{
    static class Program
    {
        /// <summary>
        /// Keep DPI runtime as DPI screen (125%)
        /// </summary>WMS-2017
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Validate login info
            frmLogin loginForm = new frmLogin();
            loginForm.ShowDialog();
            if (loginForm.HasLogin)
            {
                Application.Run(new frmMain());
            }
                
        }
    }
}
