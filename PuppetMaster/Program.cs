using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using CommonTypes;

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
