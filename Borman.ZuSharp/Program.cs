using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Borman.ZuSharp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                using (ZuLogic logic = new ZuLogic())
                {
                    Application.Run(new MainForm(logic));
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Unexpected error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
