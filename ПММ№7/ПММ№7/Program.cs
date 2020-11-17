using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ПММ_7
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = @"bats.bat";
            proc.StartInfo.WorkingDirectory = @".\";
            proc.Start();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
