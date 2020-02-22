using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Security.Cryptography;
using System.IO;

namespace AesCrypt
{
    class CrudFile
    {
        internal static void CreateNewFile()
        {
            Cryption cryption = new Cryption();
            cryption.Show();
            App.Current.MainWindow.Close();

        }
        internal static void OpenFile(string location, string pass)
        {
            byte[] data;
            Cryption cryption = new Cryption();
            try
            {
                data = File.ReadAllBytes(location);
            }
            catch(FileNotFoundException)
            {
                MessageBox.Show("File not found! Please, try again.");
                return;
            }

            DataCrypto dataCrypto = new DataCrypto();
            cryption.dataContent.Text = dataCrypto.OpenSSLDecrypt(data, pass);
            data = null;
            dataCrypto = null;
            pass = "";

            if(cryption.dataContent.Text.Equals(""))
            {
                cryption = null;
                return;
            }


            cryption.Show();
            App.Current.MainWindow.Close();

        }
        internal static void SaveEncryptedFile(string data, string pass)
        {
            DataCrypto dataCrypto = new DataCrypto();
            byte[] text = dataCrypto.OpenSSLEncrypt(data, pass);
            data = "";
            dataCrypto = null;
            pass = "";

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "aes files (*.aes)|*.txt|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllBytes(saveFileDialog.FileName, text);
                text = null;

            }
        }    
        
        internal static void SaveDecryptedFile(byte[] data, string pass)
        {
            DataCrypto dataCrypto = new DataCrypto();
            string text = dataCrypto.OpenSSLDecrypt(data, pass);
            data = null;
            dataCrypto = null;
            pass = "";

            if(text.Equals(""))
                return;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, text);
                text = null;

            }
        }
        internal static string SetFileLocation()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                return openFileDialog.FileName;
            else return null;
        }
    }
}
