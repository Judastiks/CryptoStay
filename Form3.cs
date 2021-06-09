using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoStayPR
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string file, user, server, pass;
            file = textBox4.Text;
            pass = textBox3.Text;
            user = textBox2.Text;
            server = textBox1.Text;
            string uri = "ftp://" + server + file;
            FTPserver rqs = new FTPserver();
            rqs.downloadFile(uri, user, pass);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FTPserver rq = new FTPserver();
            rq.UploadFile(textBox2.Text,textBox3.Text, "ftp://" + textBox1.Text);
            MessageBox.Show("Файл отправлен");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            if (o.ShowDialog() == DialogResult.OK)
            {
                string file = o.FileName;
                textBox4.Text = file;
            }
        }
    }
}
