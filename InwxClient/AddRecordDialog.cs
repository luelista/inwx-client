using InwxClient.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InwxClient {
    public partial class AddRecordDialog : Form {
        IInwxClient Client;

        public AddRecordDialog(IInwxClient client, string domain) {
            InitializeComponent();
            Client = client;
            textBox1.Text = domain;
            textBox1.Enabled = false;

            comboBox1.Text = Settings.Default.NewRecordDefaultType;
            textBox3.Text = Settings.Default.NewRecordDefaultName;
            textBox4.Text = Settings.Default.NewRecordDefaultContent;
            textBox5.Text = Settings.Default.NewRecordDefaultTtl;
        }



        private void button1_Click(object sender, EventArgs e) {
            try {
                if (textBox3.Text == "@") textBox3.Text = textBox1.Text;
                if (!textBox3.Text.Contains(textBox1.Text))
                    textBox3.Text += "." + textBox1.Text;

                NameserverCreateRecord rec;
                rec.domain = textBox1.Text;
                rec.type = comboBox1.Text;
                rec.content = textBox4.Text;
                rec.name = textBox3.Text;
                rec.ttl = Convert.ToInt32(textBox5.Text);
                rec.prio = Convert.ToInt32(textBox6.Text);
                var result = Client.nameserver_createRecord(rec);
                Program.dumpstruct( result);
                if (result.code < 2000)
                    DialogResult = DialogResult.OK;
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddRecordDialog_Load(object sender, EventArgs e) {

        }

        private void button3_Click(object sender, EventArgs e) {

            Settings.Default.NewRecordDefaultType = comboBox1.Text;
            Settings.Default.NewRecordDefaultName = textBox3.Text;
            Settings.Default.NewRecordDefaultContent = textBox4.Text;
            Settings.Default.NewRecordDefaultTtl = textBox5.Text;
            Settings.Default.Save();
            MessageBox.Show("Default values stored successfully.");
        }
    }
}
