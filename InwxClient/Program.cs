using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

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


        public static string RpcStructToString(XmlRpcStruct str, string indentation = "") {
            StringBuilder x = new StringBuilder();
            foreach (var key in str.Keys) {
                x.Append(indentation);
                x.Append(key);
                x.Append(": ");
                if (str[key] is XmlRpcStruct) {
                    x.AppendLine("{");
                    x.Append(RpcStructToString((XmlRpcStruct)str[key]));
                    x.AppendLine("}");
                } else {
                    x.AppendLine(str[key].ToString());
                }
            }
            return x.ToString();
        }
        public static void dumpstruct(XmlRpcStruct str) {
            MessageBox.Show(RpcStructToString(str));
        }
        public static void dumpstruct<T>(InwxResult<T> res) {
            MessageBox.Show(res.ToString(), "Server Response", MessageBoxButtons.OK, res.code<2000 ? MessageBoxIcon.None : MessageBoxIcon.Error);
        }



        /// <summary>
        /// Saves to an xml string
        /// </summary>
        public static string Serialize<T>(this T value) {
            if (value == null) return string.Empty;

            var xmlSerializer = new XmlSerializer(typeof(T));

            using (var stringWriter = new StringWriter()) {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true })) {
                    xmlSerializer.Serialize(xmlWriter, value);
                    return stringWriter.ToString();
                }
            }
        }
    }
}
