using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoStayPR
{
    public partial class Form1 : Form
    {
        public byte[] encryptFile, decryptFile, keyByte;
        int d = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void сохранитьToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Encrypt encryts;
            byte[] ByteFile = encryptFile;
            if ((keyByte == null) || (keyByte.Length != 32))
                MessageBox.Show("Введите ключ правильного размера(256 бит/32 байт!)");
            else
            {
                encryts = new Encrypt(ByteFile, keyByte);
                encryptFile = encryts.GetEncryptFile;
                MessageBox.Show("Файл успешно зашифрован!");
                d = 0;
            }
            textBox1.Text += DateTime.Now + " Файл зашифрован -  " + txtin.Text + "\r\n";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (d==0)
            {
                SaveFileDialog h = new SaveFileDialog();
                h.Filter = "All files(*.*)|*.*";
                if (h.ShowDialog() == DialogResult.Cancel)
                    return;
                string filename = h.FileName;
                File.WriteAllBytes(filename, encryptFile);
                txtout.Text = filename;
                MessageBox.Show("Файл успешно сохранён!");
            }
            else if (d==1)
            {
                SaveFileDialog h = new SaveFileDialog();
                h.Filter = "All files(*.*)|*.*";
                if (h.ShowDialog() == DialogResult.Cancel)
                    return;
                string filename = h.FileName;
                File.WriteAllBytes(filename, decryptFile);
                txtout.Text = filename;
            }
            textBox1.Text += DateTime.Now + " Файл сохранён -  " + txtout.Text + "\r\n";
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Decrypt decrypt;
            byte[] ByteFile = decryptFile;
            if ((keyByte == null) || (keyByte.Length != 32))
                MessageBox.Show("Введите ключ правильного размера(256 бит/32 байт!)");
            else
            {
                decrypt = new Decrypt(ByteFile, keyByte);
                decryptFile = decrypt.GetDecryptFile;
                MessageBox.Show("Файл успешно расшифрован!");
                d = 1;
            }
            textBox1.Text += DateTime.Now + " Файл расшифрован -  " + txtin.Text + "\r\n";
        }

        private void файлToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            if (o.ShowDialog() == DialogResult.OK)
            {
                string file = o.FileName;
                StreamReader sr = new StreamReader(file);
                string ss = sr.ReadToEnd();
                keyByte =  Encoding.GetEncoding(1251).GetBytes(ss);
            }
            textBox1.Text += DateTime.Now + " Ключ задан из файла" + "\r\n";
        }

        private void сгенерироватьКлючToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[32];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
            }
            keyByte = data;
            textBox1.Text += DateTime.Now + " Ключ сгенерирован" + "\r\n";
            DialogResult result = MessageBox.Show("Сохранить ключ в файл?","Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,  MessageBoxOptions.DefaultDesktopOnly);
            if (result == DialogResult.Yes)
            {
                SaveFileDialog h = new SaveFileDialog();
                h.Filter = "All files(*.*)|*.*";
                if (h.ShowDialog() == DialogResult.Cancel)
                    return;
                string filename = h.FileName;
                string ss = Encoding.GetEncoding(1251).GetString(keyByte);
                File.WriteAllText(filename, ss);
                MessageBox.Show("Файл успешно сохранён!");
            }
            textBox1.Text += DateTime.Now + " Ключ сохранён!"+ "\r\n";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String host = Dns.GetHostName();
            // Получение ip-адреса.
            IPAddress ip = Dns.GetHostByName(host).AddressList[0];
            textBox2.Text = ip.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                SendMessageFromSocket(11000);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            byte[] bytes = new byte[1024];
            // Соединяемся с удаленным устройством
            // Устанавливаем удаленную точку для сокета
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.100.2"), 11000);
            Socket senders = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Соединяем сокет с удаленной точкой
            senders.Connect(ipEndPoint);
            byte[] msg = { 0 };
            // Отправляем данные через сокет
            senders.Send(msg);
            // Получаем ответ от сервера
            int bytesRec = senders.Receive(bytes);
            textBox6.Text = Convert.ToString(Encoding.UTF8.GetString(bytes, 0, bytesRec))+"\r\n";
            // Освобождаем сокет
            senders.Shutdown(SocketShutdown.Both);
            senders.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            if (o.ShowDialog() == DialogResult.OK)
            {
                string file = o.FileName;
                textBox5.Text = file;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FileDeploy rq = new FileDeploy();
            string ip = textBox4.Text;
            string path = textBox5.Text;
            rq.Upload(ip, path);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите открыть порт и получить файл?  ", "Принятие файла", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                FileDownload rq = new FileDownload();
                rq.ReceiveFile();
                textBox7.Text = rq.reciveName();
            }
    }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Form3 rq = new Form3();
            rq.ShowDialog();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Form2 rq = new Form2();
            List<string> sd = new List<string>();
            {
                try
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://192.168.100.2");
                    request.Method = WebRequestMethods.Ftp.ListDirectory;

                    request.Credentials = new NetworkCredential("FTP-User", "12345");
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string names = reader.ReadToEnd();

                    reader.Close();
                    response.Close();

                    names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    rq.ss(names);
                    rq.ShowDialog();

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Form3 rq = new Form3();
            rq.ShowDialog();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            RSADecEnc rq = new RSADecEnc();
            rq.generateKeys();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            RSADecEnc rq = new RSADecEnc();
            rq.EncryptRsa(textBox15.Text);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            if (o.ShowDialog() == DialogResult.OK)
            {
                string file = o.FileName;
               textBox15.Text = file;
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            RSADecEnc rq = new RSADecEnc();
            rq.DcryptRsa(textBox15.Text);
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            if (o.ShowDialog() == DialogResult.OK)
            {
                string file = o.FileName;
            }
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            Form3 rq = new Form3();
            rq.ShowDialog();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            Form3 rq = new Form3();
            rq.ShowDialog();
        }

        private void toolStripSplitButton2_ButtonClick(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            String host = Dns.GetHostName();
            // Получение ip-адреса.
            IPAddress ip = Dns.GetHostByName(host).AddressList[0];
            textBox2.Text = ip.ToString();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                SendMessageFromSocket(11000);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            FileDeploy rq = new FileDeploy();
            string ip = textBox4.Text;
            string path = textBox5.Text;
            rq.Upload(ip, path);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            byte[] bytes = new byte[1024];
            // Соединяемся с удаленным устройством
            // Устанавливаем удаленную точку для сокета
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.100.2"), 11000);
            Socket senders = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Соединяем сокет с удаленной точкой
            senders.Connect(ipEndPoint);
            byte[] msg = { 0 };
            // Отправляем данные через сокет
            senders.Send(msg);
            // Получаем ответ от сервера
            int bytesRec = senders.Receive(bytes);
            textBox6.Text = Convert.ToString(Encoding.UTF8.GetString(bytes, 0, bytesRec)) + "\r\n";
            // Освобождаем сокет
            senders.Shutdown(SocketShutdown.Both);
            senders.Close();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите открыть порт и получить файл?  ", "Принятие файла", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                FileDownload rq = new FileDownload();
                rq.ReceiveFile();
                textBox7.Text = rq.reciveName();
            }
        }

        private void серверКлючейToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            if (o.ShowDialog() == DialogResult.OK)
            {
                string file = o.FileName;
                encryptFile = File.ReadAllBytes(file);
                txtin.Text = file;
            }
            while (((double)encryptFile.Length / 64 % 1 == 0) == false)
            {
                Array.Resize(ref encryptFile, encryptFile.Length + 1);
            }
            decryptFile = encryptFile;
            textBox1.Text += DateTime.Now + " Открыт файл -  " + txtin.Text+ "\r\n";
        }
        public void SendMessageFromSocket(int port)
        {
            // Буфер для входящих данных
            byte[] bytes = new byte[1024];
            // Соединяемся с удаленным устройством
            // Устанавливаем удаленную точку для сокета
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.100.2"), port);
            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);
            string message = textBox2.Text;
            byte[] msg = Encoding.UTF8.GetBytes(message);
            // Отправляем данные через сокет
            sender.Send(msg);
            // Получаем ответ от сервера
            int bytesRec = sender.Receive(bytes);
            textBox3.Text = Convert.ToString(Encoding.UTF8.GetString(bytes, 0, bytesRec));
            // Освобождаем сокет
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
    }
}

