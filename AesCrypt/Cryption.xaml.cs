using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AesCrypt
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Cryption : Window
    {
        internal bool encStateBool;
        public Cryption()
        {
            InitializeComponent();
            CustomizationView();
        }
        private void CustomizationView()
        {
            if (!MainWindow.emptyFileBool)
            {
                passPanel.Visibility = Visibility.Collapsed;
            }
            if (!encStateBool)
            {
                this.Title = "Decryption";
                passCheckLabel.Visibility = Visibility.Collapsed;
                passCheckField.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.Title = "Encryption";
                passCheckLabel.Visibility = Visibility.Visible;
                passCheckField.Visibility = Visibility.Visible;
            }
        }

        //FIRST MENUITEM - FILE
        private void SetNewFile(object sender, RoutedEventArgs e)
        {
            dataContent.Text = "";
            passField.Password = "";
            passCheckField.Password = "";
        } //Exist

        private void OpenEncryptedFile(object sender, RoutedEventArgs e)
        {
            string file = CrudFile.SetFileLocation();
            dataContent.Text = Encoding.Default.GetString(CrudFile.CallOpenFileDialogB(file));
        }
        private void OpenDecryptedFile(object sender, RoutedEventArgs e)
        {
            string file = CrudFile.SetFileLocation();
            dataContent.Text = CrudFile.CallOpenFileDialogS(file);
        }

        private void SaveContextToFile(object sender, RoutedEventArgs e)
        {
            if (!encStateBool) //decrypted
            {
                CrudFile.CallSaveFileDialog(dataContent.Text);
            }
            else //encrypted
            {
                CrudFile.CallSaveFileDialog(dataContent.Text);
            }
            dataContent.Text = "";
            MessageBox.Show("Successfully!");
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        } //Exist, Need check

        private void ExitEverywhere(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        } //Exist

        //SECOND MENUITEM - PASS PANEL
        //Visibilty Pass Panel
        private void ShowPassPanel(object sender, RoutedEventArgs e)
        {
            passPanel.Visibility = Visibility.Visible;
        } //Exist
        private void HidePassPanel(object sender, RoutedEventArgs e)
        {
            passPanel.Visibility = Visibility.Collapsed;
        } //Exist

        //Cryption Mode
        private void SetEncryptionMode(object sender, RoutedEventArgs e)
        {
            this.Title = "Encryption";
            passCheckLabel.Visibility = Visibility.Visible;
            passCheckField.Visibility = Visibility.Visible;
            encStateBool = true;
        } //Exist
        private void SetDecryptionMode(object sender, RoutedEventArgs e)
        {
            this.Title = "Decryption";
            passCheckLabel.Visibility = Visibility.Collapsed;
            passCheckField.Visibility = Visibility.Collapsed;
            encStateBool = false;
        } //Exist

        //PASS PANEL CONTROLLERS
        private void OpenDataLocal(object sender, RoutedEventArgs e)
        {
            if (dataContent.Text == "" || passField.Password == "")
            {
                MessageBox.Show("Text is empty!");
                dataContent.Text = "";
                passField.Password = "";
                passCheckField.Password = "";
                return;
            }
            DataCrypto dataCrypto = new DataCrypto();
            if (!encStateBool) //Decryption
            {
                string text = dataCrypto.OpenSSLDecrypt(Encoding.ASCII.GetBytes(dataContent.Text), passField.Password);
                if(text.Equals(""))
                {
                    passField.Password = "";
                    passCheckField.Password = "";
                    dataCrypto = null;
                    text = null;
                    MessageBox.Show("Error while trying to decrypt data!");
                    return;
                }
                MainWindow.emptyFileBool = false;
                encStateBool = true;
                CustomizationView();
                dataContent.Text = text;
                text = null;
            }
            else //Encryption
            {
                if (passField.Password.Equals(passCheckField.Password))
                {
                    byte[] data = dataCrypto.OpenSSLEncrypt(dataContent.Text, passField.Password);
                    MainWindow.emptyFileBool = false;
                    encStateBool = false;
                    CustomizationView();
                    dataContent.Text = Encoding.ASCII.GetString(data);
                    data = null;
                }
                else
                    MessageBox.Show("Passwords are not same!");
            }
            passField.Password = "";
            passCheckField.Password = "";
            dataCrypto = null;
        } //Exist, Need check
        private void SaveDataToFile(object sender, RoutedEventArgs e)
        {
            if (dataContent.Text == "" || passField.Password == "")
            {
                MessageBox.Show("Text is empty!");
                dataContent.Text = "";
                passField.Password = "";
                passCheckField.Password = "";
                return;
            }

            if (!encStateBool)
            {
                CrudFile.SaveDecryptedFile(Encoding.ASCII.GetBytes(dataContent.Text), passField.Password);
            }
            else
            {
                if (passField.Password.Equals(passCheckField.Password))
                {
                    CrudFile.SaveEncryptedFile(dataContent.Text, passField.Password);
                }
                else
                    MessageBox.Show("Passwords are not same!");
            }
            MessageBox.Show("Completed!");

            dataContent.Text = "";
            passField.Password = "";
            passCheckField.Password = "";
            Application.Current.Shutdown();
        } //Exist, Need check
        private void BackToMainMenu(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        } //Exist
    }
}
