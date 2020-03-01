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
        private bool crypTypeStandardBool = true;

        public Cryption(bool value)
        {
            InitializeComponent();
            this.encStateBool = value;
            CustomizationView();
        }
        private void CustomizationView()
        {
            if (!encStateBool)
            {
                decryptionModeMenuItem.IsChecked = true;
                this.Title = "Decryption";
                passCheckLabel.Visibility = Visibility.Collapsed;
                passCheckField.Visibility = Visibility.Collapsed;
            }
            else
            {
                encryptionModeMenuItem.IsChecked = true;
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

        private void OpenEncryptedFileByteByte(object sender, RoutedEventArgs e)
        {
            string file = CrudFile.SetFileLocation();
            try
            {
                dataContent.Text = Encoding.ASCII.GetString(CrudFile.CallOpenFileDialogB(file));
                crypTypeStanMenuItem.IsChecked = true;
            }
            catch (ArgumentNullException)
            {
                return;
            }

        } //Exist
        private void OpenEncryptedFileByteBase64(object sender, RoutedEventArgs e)
        {

            string file = CrudFile.SetFileLocation();
            try
            {
                dataContent.Text = Convert.ToBase64String(CrudFile.CallOpenFileDialogB(file));
                crypTypeBase64MenuItem.IsChecked = true;
            }
            catch (ArgumentNullException)
            {
                return;
            }

        } //Exist
        private void OpenEncryptedFileBase64Byte(object sender, RoutedEventArgs e)
        {
            string file = CrudFile.SetFileLocation();
            try
            {
                dataContent.Text = Encoding.ASCII.GetString(Convert.FromBase64String(CrudFile.CallOpenFileDialogS(file)));
                crypTypeStanMenuItem.IsChecked = true;
            }
            catch(ArgumentNullException)
            {
                return;
            }

        } //Exist
        private void OpenEncryptedFileBase64Base64(object sender, RoutedEventArgs e)
        {
            string file = CrudFile.SetFileLocation();
            try
            {
                dataContent.Text = CrudFile.CallOpenFileDialogS(file);
                crypTypeBase64MenuItem.IsChecked = true;
            }
            catch (ArgumentNullException)
            {
                return;
            }

        } //Exist
        private void OpenDecryptedFile(object sender, RoutedEventArgs e)
        {
            string file = CrudFile.SetFileLocation();
            try
            {
                dataContent.Text = CrudFile.CallOpenFileDialogS(file);
            }
            catch (ArgumentNullException)
            {
                return;
            }
        } //Exist

        private void SaveContextToFile(object sender, RoutedEventArgs e)
        {
            CrudFile.CallSaveFileDialog(dataContent.Text);
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

        //Cryption type
        private void CryptionTypeStandard(object sender, RoutedEventArgs e)
        {
            crypTypeStandardBool = true;
        } //Exist
        private void CryptionTypeBase64(object sender, RoutedEventArgs e)
        {
            crypTypeStandardBool = false;
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
                string text = "";
                if (!crypTypeStandardBool)
                {
                    text = dataCrypto.OpenSSLDecrypt(Convert.FromBase64String(dataContent.Text), passField.Password);
                    crypTypeBase64MenuItem.IsChecked = true;
                }
                else
                {
                    text = dataCrypto.OpenSSLDecrypt(Encoding.ASCII.GetBytes(dataContent.Text), passField.Password);
                    crypTypeStanMenuItem.IsChecked = true;
                }

                if (text.Equals(""))
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
                    if (!crypTypeStandardBool)
                    {
                        dataContent.Text = Convert.ToBase64String(data);
                        crypTypeBase64MenuItem.IsChecked = true;
                    }
                    else
                    {
                        crypTypeStanMenuItem.IsChecked = true;
                        dataContent.Text = Encoding.ASCII.GetString(data);
                    }
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
                if (!crypTypeStandardBool)
                    CrudFile.SaveDecryptedFile(Convert.FromBase64String(dataContent.Text), passField.Password);
                else CrudFile.SaveDecryptedFile(Encoding.ASCII.GetBytes(dataContent.Text), passField.Password);
            }
            else
            {
                if (passField.Password.Equals(passCheckField.Password))
                {
                    CrudFile.SaveEncryptedFile(dataContent.Text, passField.Password); //Saves only standart type!!!
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
