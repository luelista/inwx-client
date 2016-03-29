using CookComputing.XmlRpc;
using DbScriptApp;
using InwxClient.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InwxClient {
    public partial class MainWindow : Form {
        public MainWindow() {
            InitializeComponent();

            client = XmlRpcProxyGen.Create<IInwxClient>();
            // Tracer tracer = new Tracer();
            //  tracer.Attach(client);
            //client.CookieContainer = new System.Net.CookieContainer();

            //RequestResponseLogger dumper = new RequestResponseLogger();
            //dumper.Attach(client);

        }

        public IInwxClient client;

        public class Tracer : XmlRpcLogger {
            protected override void OnRequest(object sender,
              XmlRpcRequestEventArgs e) {
                DumpStream(e.RequestStream);
            }

            protected override void OnResponse(object sender,
              XmlRpcResponseEventArgs e) {
                DumpStream(e.ResponseStream);
            }

            private void DumpStream(Stream stm) {
                stm.Position = 0;
                byte[] buffer = new byte[1024];
                stm.Read(buffer, 0, 1024);
                Console.WriteLine(System.Text.Encoding.ASCII.GetString(buffer));
                //TextReader trdr = new StreamReader(stm);
                //string s = trdr.ReadLine();
                //while (s != null) {
                //    Console.WriteLine(s);
                //    s = trdr.ReadLine();
                //}
                stm.Position = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            setStatus(0, "Loading data ...");
            Settings.Default.InwxUsername = textBox1.Text;
            Settings.Default.InwxPasswort = textBox2.Text;
            Settings.Default.Save();

            domainSel.Items.Clear();
            domainSel.Text = "";
            objectListView1.ClearObjects();

            var test = client.login(new LoginParameter( textBox1.Text, textBox2.Text));
            setStatus(test.code, test.msg);

            //dumpstruct(test);
            textBox3.Text = "Customer ID: " + test.resData.customerId.ToString() + "\r\nAccount ID: " + test.resData.accountId.ToString() + "\r\n" + test.resData.tfa;
            if (!String.IsNullOrEmpty(test.resData.tfa) && test.resData.tfa != "0") {
                string unlockCode = InputDlg.InputBox("Please unlock: "+test.resData.tfa,"");
                if (String.IsNullOrEmpty(unlockCode)) return;
                setStatus(0, "Loading data ...");
                test = client.login_unlock(new UnlockParameter(unlockCode));
                setStatus(test.code, test.msg);
                if (test.code != 1000) return;
            }

            var domains = client.nameserver_list();
            setStatus(domains.code, domains.msg);

            //Program.dumpstruct(domains);
            //Program.dumpstruct((XmlRpcStruct) domains["resData"]);

            List<DbScriptApp.MulticolComboItem> items = new List<DbScriptApp.MulticolComboItem>();
            foreach(var dom in domains.resData.domains) {
                var item = new DbScriptApp.MulticolComboItem(new string[] {
                    dom.domain,
                    dom.roId.ToString(),
                    dom.type,
                    dom.ipv4,
                    dom.ipv6,
                    dom.web,
                    dom.mail

                });
                items.Add(item);
            }
            domainSel.Items.AddRange(items.ToArray());
            if (domainSel.Items.Count > 0) domainSel.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e) {
            listRecords();
        }

        void listRecords() {
            setStatus(0, "Loading data ...");
            var test = client.nameserver_info(new NameserverInfoParameter(domainSel.Text));
            setStatus(test.code, test.msg);
            //MessageBox.Show(test.ToString());
            objectListView1.SetObjects(test.resData.record);
            objectListView1.BuildGroups(objectListView1.AllColumns[2], SortOrder.Ascending);
        }

        public void setStatus(int code, string status) {
            toolStripStatusLabel1.Text = code.ToString();
            toolStripStatusLabel2.Text = status;
        }

        private void objectListView1_CellEditFinishing(object sender, BrightIdeasSoftware.CellEditEventArgs e) {
            
        }

        private void objectListView1_ItemsChanged(object sender, BrightIdeasSoftware.ItemsChangedEventArgs e) {
            
        }

        private void Form1_Load(object sender, EventArgs e) {
            textBox1.Text = Settings.Default.InwxUsername;
            textBox2.Text = Settings.Default.InwxPasswort;

        }

        private void speicherToolStripMenuItem_Click(object sender, EventArgs e) {
            setStatus(0, "Loading data ...");
            var test = client.nameserver_updateRecord(contextModel);
            setStatus(test.code, test.msg);
        }

        NameserverRecord contextModel;
        private void objectListView1_CellRightClick(object sender, BrightIdeasSoftware.CellRightClickEventArgs e) {
            if (e.Model is NameserverRecord) {
                contextModel = (NameserverRecord)e.Model;
                e.MenuStrip = contextMenuStrip1;
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            using (var f = new AddRecordDialog(client, domainSel.Text)) {
                if (f.ShowDialog() == DialogResult.OK) {
                    listRecords();
                }

            }
        }

        private void domainSel_SelectedIndexChanged(object sender, EventArgs e) {
            listRecords();
        }

        private void button4_Click(object sender, EventArgs e) {
            using (var f = new AboutBox1())
                f.ShowDialog();
        }
    }
}
