using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Security;
namespace CryptoStayPR
{
    class RSADecEnc
    {
        private static FileStream fs;
        byte[] EncryptedData;
        byte[] DecryptedData;
        string publicxml = "";
        string privatexml = "";
        string filename = "";
        public void generateKeys()
        {
            try
            {
                RSACryptoServiceProvider RsaKey = new RSACryptoServiceProvider();
                string publickey = RsaKey.ToXmlString(false);
                string privatekey = RsaKey.ToXmlString(true);
                File.WriteAllText("private.xml", privatekey, Encoding.UTF8);
                File.WriteAllText("public.xml", publickey, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public void EncryptRsa(string path)
        {
            publicxml = File.ReadAllText("public.xml", Encoding.UTF8);
            byte[] data = new byte[1024];
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            try
            {
                rsa.FromXmlString(publicxml);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            try
            {
                string s = File.ReadAllText(path);
                data = Encoding.GetEncoding(1251).GetBytes(s);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
            try
            {
                EncryptedData = rsa.Encrypt(data, false);
            }
            catch (CryptographicException ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
            SaveFileDialog h = new SaveFileDialog();
            h.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            if (h.ShowDialog() == DialogResult.Cancel)
                return;
            filename = h.FileName;
            string ff = Convert.ToBase64String(EncryptedData);
            File.WriteAllText(filename, ff);
            MessageBox.Show("Файл сохранен");

        }
        public void DcryptRsa(string path)
        {
            byte[] data = new byte[1024];
            privatexml = File.ReadAllText("private.xml", Encoding.UTF8);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            try
            {
                if (privatexml.Length == 0)
                {
                    MessageBox.Show("Неправильный приватный ключ!");
                    return;
                }
                else
                {

                    rsa.FromXmlString(privatexml);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            try
            {
                string s = File.ReadAllText(path);
                data = Convert.FromBase64String(s);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
            DecryptedData = rsa.Decrypt(data, false);
            SaveFileDialog h = new SaveFileDialog();
            h.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            if (h.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = h.FileName;
            string ss = Encoding.GetEncoding(1251).GetString(DecryptedData);
            File.WriteAllText(filename,ss);
            MessageBox.Show("Файл сохранен");
        }

    }
}

