using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InwxClient {
    static class Program {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }


        public static void dumpstruct(XmlRpcStruct str) {
            String x = "";
            foreach (var key in str.Keys) {
                x += key + ": " + str[key].ToString() + "\n";
            }
            MessageBox.Show(x);
        }
    }
}
