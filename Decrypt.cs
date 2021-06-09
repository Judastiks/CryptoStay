using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoStayPR
{
    class Decrypt
    {
        byte[] decripttFile; uint[] newintKey; ulong[] newulongFile;
        public Decrypt(byte[] file, byte[] key)
        {
            newintKey = UintKeyArray(key);
            newulongFile = UlongFileArray(file);
            decripttFile = ConvertToByte(DecryptFile());
        }
        public byte[] GetDecryptFile
        {
            get
            {
                return decripttFile;
            }
        }

        private ulong[] DecryptFile()
        {
            EncryptDecrypt[] K = new EncryptDecrypt[8];
            ulong[] ulongEncrFile = new ulong[newulongFile.Length];
            for (int k = 0; k < newulongFile.Length; k++)
            {
                ulongEncrFile[k] = newulongFile[k];
                for (int i = 0; i < K.Length; i++)
                {
                    K[i] = new EncryptDecrypt(ulongEncrFile[k], newintKey[i]);
                    ulongEncrFile[k] = K[i].AllEncrypt(false);
                }
                for (int j = 0; j < 3; j++)
                {
                    for (int i = K.Length - 1; i >= 0; i--)
                    {
                        K[i] = new EncryptDecrypt(ulongEncrFile[k], newintKey[i]);

                        if ((j == 2) && (i == 0))
                            ulongEncrFile[k] = K[i].AllEncrypt(true);
                        else
                            ulongEncrFile[k] = K[i].AllEncrypt(false);
                    }
                }
            }
            return ulongEncrFile;
        }


        public uint[] UintKeyArray(byte[] byteKey)
        {
            uint[] key = new uint[8];
            for (int i = 0; i < key.Length; i++)
            {
                key[i] = BitConverter.ToUInt32(byteKey, i * 4);
            }
            return key;
        }
        public ulong[] UlongFileArray(byte[] byteFile)
        {
            ulong[] data;
            data = new ulong[byteFile.Length / 8];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = BitConverter.ToUInt64(byteFile, i * 8);
            }
            return data;
        }
        public byte[] ConvertToByte(ulong[] fl)
        {
            byte[] temp = new byte[8];
            byte[] encrByteFile = new byte[fl.Length * 8];

            for (int i = 0; i < fl.Length; i++)
            {
                temp = BitConverter.GetBytes(fl[i]);

                for (int j = 0; j < temp.Length; j++)
                    encrByteFile[j + i * 8] = temp[j];
            }

            return encrByteFile;
        }
    }

}

