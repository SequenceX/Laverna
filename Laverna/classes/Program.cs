using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laverna
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ProGSYMainFrame  mainFrame = new  ProGSYMainFrame();
            
            Application.Run(mainFrame);
            //Application.Run(new ProGSYMainFrame());

           
        }
    }
}
