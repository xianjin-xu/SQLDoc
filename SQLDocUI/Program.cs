using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;
using System.Data;

namespace SQLDocUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SQLDocSharp());
        }
    }
}
