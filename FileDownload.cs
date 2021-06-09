using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
namespace CryptoStayPR
{
    class FileDownload
    {
        private static FileStream fs;
        string filename;
        public void ReceiveFile()
        {
            bool d = true;
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 12000);
            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sender.Bind(ipEndPoint);
                sender.Listen(20);
                while (d == true)
                {
                    byte[] a = new byte[1024];
                    // Получаем файл
                    Socket handler = sender.Accept();
                    int received = handler.Receive(a);

                    MessageBox.Show("Файл получен...Сохраняем...");
                    string reply = "Файл доставлен";
                    // Создаем временный файл с полученным расширением
                    SaveFileDialog h = new SaveFileDialog();
                    h.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
                    if (h.ShowDialog() == DialogResult.Cancel)
                        return;
                    filename = h.FileName;
                    fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                    MessageBox.Show("Файл сохранен");
                    fs.Write(a, 0, received);
                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    handler.Send(msg);      
                    d = false;

                }
            }

            catch (Exception eR)
            {
                MessageBox.Show(eR.ToString());
            }
            finally
            {
                fs.Close();
                sender.Close();
            }
        }
        public string reciveName()
        {
            return filename;
        }
    }
}
