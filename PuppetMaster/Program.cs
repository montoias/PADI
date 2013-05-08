using System;
using System.Windows.Forms;

namespace PuppetMaster
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

            PuppetMaster puppetMaster = new PuppetMaster();
            Form1 form = new Form1(puppetMaster);
            puppetMaster.setForm(form);

            Application.Run(form);
        }
    }
}
