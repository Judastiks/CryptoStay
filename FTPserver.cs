using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;

namespace CryptoStayPR
{
    class FTPserver
    {
        struct FtpSetting
        {
            public string Server { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string FileName { get; set; }
            public string FullName { get; set; }

        }
        FtpSetting inputParametr;

        public void UploadFile(string Username, string pass, string ServerIP)
        {
             OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(ofd.FileName);
         
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", ServerIP, fi.Name)));
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential(Username, pass);
                    Stream ftpStream = request.GetRequestStream();
                    FileStream fs = File.OpenRead(inputParametr.FullName);
                    byte[] buff = new byte[1024];
                    double total = (double)fs.Length;
                    int byteread;
                    double read = 0;
                    do
                    {
                        byteread = fs.Read(buff, 0, 1024);
                        ftpStream.Write(buff, 0, byteread);
                        read += (double)byteread;
                    }
                    while (byteread != 0);
                    fs.Close();
                    ftpStream.Close();
                }
        }
        public void downloadFile(string Uri, string user, string pass)
        {
            WebClient request = new WebClient();
            {
                request.Credentials = new NetworkCredential(user, pass);
                byte[] fileData = request.DownloadData(Uri);
                SaveFileDialog h = new SaveFileDialog();
                h.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
                if (h.ShowDialog() == DialogResult.Cancel)
                    return;
                string filename = h.FileName;
                using (FileStream file = File.Create(filename))
                {
                    file.Write(fileData, 0, fileData.Length);
                    file.Close();
                }
                MessageBox.Show("Скачивание завершено!");
            }
        }
    }
}

