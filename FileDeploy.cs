using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoStayPR
{
    class FileDeploy
    {
        private static FileStream fs;
        public void Upload(string ip, string path)
        {
            try
            {
                // Получаем удаленный IP-адрес и создаем IPEndPoint
                fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                // Отправляем сам файл
                SendFile(ip);
            }
            catch (Exception eR)
            {
                MessageBox.Show(eR.ToString());
            }
        }

        private static void SendFile(string ip)
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12000);
            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Создаем файловый поток и переводим его в байты
            Byte[] bytes = new Byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            MessageBox.Show("Отправка файла размером " + fs.Length + " байт");
            sender.Connect(ipEndPoint);
            // Отправляем файл
            sender.Send(bytes);
            int bytesRec = sender.Receive(bytes);
            MessageBox.Show(Convert.ToString(Encoding.UTF8.GetString(bytes, 0, bytesRec)));
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
    }
}