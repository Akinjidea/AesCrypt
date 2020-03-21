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
        private bool encStateBool;
        private bool crypTypeStandardBool = true;
        private bool toBitConverterResult = true;
        private bool fromBitConverterResult = true;

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
        public static byte[] GetBytes(string value)
        {
            byte[] result = null;
            try
            {
                result = value.Split('-').Select(s => byte.Parse(s, System.Globalization.NumberStyles.HexNumber)).ToArray();
                return result;
            }
            catch(FormatException)
            {
                return result;
            }
        }

        //FIRST MENUITEM - FILE. Everyone works.
        private void SetNewFile(object sender, RoutedEventArgs e)
        {
            dataContent.Text = "";
            passField.Password = "";
            passCheckField.Password = "";
        } //Doesn't need to change

        private void OpenEncryptedFileByteByte(object sender, RoutedEventArgs e)
        {
            string file = CrudFile.SetFileLocation();
            byte[] data;
            try
            {
                data = GetBytes(CrudFile.CallOpenFileDialogS(file));
                if (data != null)
                {
                    dataContent.Text = BitConverter.ToString(data);
                }
                else if (fromBitConverterResult)
                    dataContent.Text = BitConverter.ToString(CrudFile.CallOpenFileDialogB(file));
                else
                    dataContent.Text = Encoding.ASCII.GetString(CrudFile.CallOpenFileDialogB(file));
                crypTypeStanMenuItem.IsChecked = true;
            }
            catch (ArgumentNullException)
            {
                return;
            }
            catch (FormatException)
            {
                MessageBox.Show("Incorrect input value!");
            }
        } //Exist, Correct!
        private void OpenEncryptedFileByteBase64(object sender, RoutedEventArgs e)
        {

            string file = CrudFile.SetFileLocation();
            byte[] data;
            try
            {
                data = GetBytes(CrudFile.CallOpenFileDialogS(file));
                if (data != null)
                {
                    dataContent.Text = Convert.ToBase64String(data);
                }
               else dataContent.Text = Convert.ToBase64String(CrudFile.CallOpenFileDialogB(file));
               crypTypeBase64MenuItem.IsChecked = true;
            }
            catch (ArgumentNullException)
            {
                return;
            }

        } //Exist, Correct!
        private void OpenEncryptedFileBase64Byte(object sender, RoutedEventArgs e)
        {
            string file = CrudFile.SetFileLocation();
            try
            {
                if (fromBitConverterResult)
                    dataContent.Text = BitConverter.ToString(Convert.FromBase64String(CrudFile.CallOpenFileDialogS(file)));
                else dataContent.Text = Encoding.ASCII.GetString(Convert.FromBase64String(CrudFile.CallOpenFileDialogS(file)));
                crypTypeStanMenuItem.IsChecked = true;
            }
            catch(ArgumentNullException)
            {
                return;
            }
            catch(FormatException)
            {
                MessageBox.Show("Incorrect input data!");
                return;
            }

        } //Exist, Correct!
        private void OpenEncryptedFileBase64Base64(object sender, RoutedEventArgs e)
        {
            string file = CrudFile.SetFileLocation();
            byte[] data;
            try
            {
                data = Convert.FromBase64String(CrudFile.CallOpenFileDialogS(file));
                dataContent.Text = Convert.ToBase64String(data);
                crypTypeBase64MenuItem.IsChecked = true;
            }
            catch (ArgumentNullException)
            {
                return;
            }
            catch(FormatException)
            {
                MessageBox.Show("Incorrect input data!");
                return;
            }

        } //Exist, Correct!
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
            if (!toBitConverterResult)
                CrudFile.CallSaveFileDialog(dataContent.Text);
            else
            {
                byte[] data = GetBytes(dataContent.Text);
                if (data == null)
                {
                    MessageBox.Show("Incorrect context! Can't save as hex data.");
                    data = null;
                    return;
                }
                CrudFile.CallSaveFileDialog(data);
                data = null;
            } 
            MessageBox.Show("Successfully!");
        } //Exist

        private void ConvertLocalToHex(object sender, RoutedEventArgs e)
        {
            string data;
            try
            {
                data = BitConverter.ToString(Convert.FromBase64String(dataContent.Text));
                dataContent.Text = data;
                crypTypeStanMenuItem.IsChecked = true;
                crypTypeStandardBool = true;
                fromBitConverterCheckBox.IsChecked = true;
                fromBitConverterResult = true;
            }
            catch(FormatException)
            {
                MessageBox.Show("Can't convert current text to decimal!");
            }
        }
        private void ConvertLocalToBase64(object sender, RoutedEventArgs e)
        {
            string data;
            try
            {
                data = Convert.ToBase64String(GetBytes(dataContent.Text));
                dataContent.Text = data;
                crypTypeBase64MenuItem.IsChecked = true;
                crypTypeStandardBool = false;
                fromBitConverterCheckBox.IsChecked = false;
                fromBitConverterResult = false;
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Can't convert current text to Base64!");
                return;
            }
        }

        private void ExitEverywhere(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        } //Exist, Doesn't need to change


        //SECOND MENUITEM - PASS PANEL. Still works.
        //Visibilty Pass Panel
        private void ShowPassPanel(object sender, RoutedEventArgs e)
        {
            passPanel.Visibility = Visibility.Visible;
        } //Exist, Doesn't need to change
        private void HidePassPanel(object sender, RoutedEventArgs e)
        {
            passPanel.Visibility = Visibility.Collapsed;
        } //Exist, Doesn't need to change

        //THIRD PART - CRYPTION MODES AND TYPES. Seems like all of it works.
        //Cryption Mode
        private void SetEncryptionMode(object sender, RoutedEventArgs e)
        {
            this.Title = "Encryption";
            passCheckLabel.Visibility = Visibility.Visible;
            passCheckField.Visibility = Visibility.Visible;
            encStateBool = true;
        } //Exist, Doesn't need to change
        private void SetDecryptionMode(object sender, RoutedEventArgs e)
        {
            this.Title = "Decryption";
            passCheckLabel.Visibility = Visibility.Collapsed;
            passCheckField.Visibility = Visibility.Collapsed;
            encStateBool = false;
        } //Exist, Doesn't need to change

        //Cryption type
        private void CryptionTypeStandard(object sender, RoutedEventArgs e)
        {
            crypTypeStandardBool = true;
        } //Exist, Doesn't need to change
        private void CryptionTypeBase64(object sender, RoutedEventArgs e)
        {
            crypTypeStandardBool = false;
            toBitConverterCheckBox.IsChecked = false;
            toBitConverterResult = false;
            fromBitConverterCheckBox.IsChecked = false;
            fromBitConverterResult = false;
        } //Exist, Doesn't need to change

        private void ConvertingResultFromBitsChecked(object sender, RoutedEventArgs e)
        {
            fromBitConverterResult = true;
            crypTypeStanMenuItem.IsChecked = true;
            crypTypeStandardBool = true;
        }
        private void ConvertingResultFromBitsUnchecked(object sender, RoutedEventArgs e)
        {
            fromBitConverterResult = false;
        }

        private void ConvertingResultToBitsChecked(object sender, RoutedEventArgs e)
        {
            toBitConverterResult = true;
            crypTypeStanMenuItem.IsChecked = true;
            crypTypeStandardBool = true;
        }
        private void ConvertingResultToBitsUnchecked(object sender, RoutedEventArgs e)
        {
            toBitConverterResult = false;
        }


        //PASS PANEL CONTROLLERS
        private void OpenDataLocal(object sender, RoutedEventArgs e)
        {
            if (dataContent.Text == "" || passField.Password == "")
            {
                MessageBox.Show("Text is empty!");
                CleanPassWithContext(true);
                return;
            }

            DataCrypto dataCrypto = new DataCrypto();

            if (!encStateBool) //Decryption
            {
                string text = "";
                if (!crypTypeStandardBool) //From Base64
                    text = dataCrypto.OpenSSLDecrypt(Convert.FromBase64String(dataContent.Text), passField.Password);
                else if (crypTypeStandardBool && fromBitConverterResult) //From Hex
                {
                    var data = GetBytes(dataContent.Text);
                    if (data == null)
                    {
                        MessageBox.Show("Can't get bytes! Incorrect input data!");
                        CleanPassWithContext(false);
                        return;
                    }
                    text = dataCrypto.OpenSSLDecrypt(data, passField.Password);
                    data = null;
                }
                else if (crypTypeStandardBool && !fromBitConverterResult) //From standard
                {
                    MessageBox.Show("This mode doesn't work correct. Please, change it and try again!");
                    CleanPassWithContext(false);
                    return;
                }

                if (text.Equals(""))
                {
                    CleanPassWithContext(false);
                    dataCrypto = null;
                    text = null;
                    MessageBox.Show("Error while trying to decrypt data!");
                    return;
                }

                if (toBitConverterResult) //to something
                    dataContent.Text = BitConverter.ToString(Encoding.ASCII.GetBytes(text));
                else dataContent.Text = text;

                text = null;
                encStateBool = true;
                CustomizationView();
            }
            else //Encryption
            {
                if (passField.Password.Equals(passCheckField.Password))
                {
                    byte[] data = dataCrypto.OpenSSLEncrypt(dataContent.Text, passField.Password);
                    if (!crypTypeStandardBool) //To Base64
                        dataContent.Text = Convert.ToBase64String(data);
                    else if (crypTypeStandardBool && !toBitConverterResult) //To Standard
                        dataContent.Text = Encoding.ASCII.GetString(data);
                    else if (crypTypeStandardBool && toBitConverterResult) //To Hex
                        dataContent.Text = BitConverter.ToString(data);
                    data = null;
                }
                else
                {
                    MessageBox.Show("Passwords are not same!");
                    return;
                }
                encStateBool = false;
                CustomizationView();
            }
            CleanPassWithContext(false);
            dataCrypto = null;
        } //Exist
        private void SaveDataToFile(object sender, RoutedEventArgs e)
        {
            if (dataContent.Text == "" || passField.Password == "")
            {
                MessageBox.Show("Text is empty!");
                CleanPassWithContext(true);
                return;
            }
            if (!encStateBool) //Decr
            {
                if (!crypTypeStandardBool) //
                    CrudFile.SaveDecryptedFile(Convert.FromBase64String(dataContent.Text), passField.Password);
                else if (crypTypeStandardBool && toBitConverterResult)
                {
                    var data = GetBytes(dataContent.Text);
                    if (data == null)
                    {
                        MessageBox.Show("Can't get bytes! Incorrect input data!");
                        CleanPassWithContext(false);
                        return;
                    }
                    CrudFile.SaveDecryptedFile(data, passField.Password);
                    data = null;
                }
                else if (crypTypeStandardBool && !toBitConverterResult)
                {
                    MessageBox.Show("This mode doesn't work correct. Please, change it and try again!");
                    CleanPassWithContext(false);
                    return;
                }
                else MessageBox.Show("Something gone wrong...");

            }
            else //Encr
            {
                if (passField.Password.Equals(passCheckField.Password))
                {
                    CrudFile.SaveEncryptedFile(dataContent.Text, passField.Password, crypTypeStandardBool, toBitConverterResult);
                }
                else
                    MessageBox.Show("Passwords are not same!");
            }
            MessageBox.Show("Completed!");

            dataContent.Text = "";
            CleanPassWithContext(false);
        } //Exist, Need check
        private void BackToMainMenu(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        } //Exist

        private void CleanPassWithContext(bool all)
        {
            passField.Password = "";
            passCheckField.Password = "";
            if (all) dataContent.Text = "";
        }
    }
}
