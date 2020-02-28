using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AesCrypt
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static bool? emptyFileBool;
        internal static bool encStateBool;
        public MainWindow()
        {
            InitializeComponent();
            emptyProjectCheckBox.IsChecked = true;
            emptyFileBool = true;
            encStateBool = true;
        }

        private void DecryptionChecked(object sender, RoutedEventArgs e)
        {
            encStateBool = false;
            if (emptyProjectCheckBox.IsChecked.GetValueOrDefault()) 
                return;
            passCheckField.Visibility = Visibility.Hidden;
            passCheckLabel.Visibility = Visibility.Hidden;
            openButton.Visibility = Visibility.Visible;
        }
        private void DecryptionUnchecked(object sender, RoutedEventArgs e)
        {
            encStateBool = true;
            if (emptyProjectCheckBox.IsChecked.GetValueOrDefault()) 
                return;
            passCheckField.Visibility = Visibility.Visible;
            passCheckLabel.Visibility = Visibility.Visible;
            openButton.Visibility = Visibility.Hidden;
        }

        private void EmptyProjectChecked(object sender, RoutedEventArgs e)
        {
            this.Height = 320;

            emptyFileBool = true;
            passLabel.Visibility = Visibility.Collapsed;
            passField.Visibility = Visibility.Collapsed;
            passCheckLabel.Visibility = Visibility.Collapsed;
            passCheckField.Visibility = Visibility.Collapsed;
            saveButton.Visibility = Visibility.Collapsed;
            setLocationButton.Visibility = Visibility.Hidden;
            createButton.Visibility = Visibility.Visible;

            var bc = new BrushConverter();
            locationField.Text = null;
            passField.Password = null;
            passCheckField.Password = null;
            locationField.Background = (Brush)bc.ConvertFrom("#BBBBBB");
        }
        private void EmptyProjectUnchecked(object sender, RoutedEventArgs e)
        {
            this.Height = 500;

            emptyFileBool = false;
            passLabel.Visibility = Visibility.Visible;
            passField.Visibility = Visibility.Visible;
            saveButton.Visibility = Visibility.Visible;
            setLocationButton.Visibility = Visibility.Visible;
            createButton.Visibility = Visibility.Collapsed;

            if(decryptionRadio.IsChecked.GetValueOrDefault())
            {
                passCheckLabel.Visibility = Visibility.Hidden;
                passCheckField.Visibility = Visibility.Hidden;
                openButton.Visibility = Visibility.Visible;
            }
            if (!decryptionRadio.IsChecked.GetValueOrDefault())
            {
                passCheckLabel.Visibility = Visibility.Visible;
                passCheckField.Visibility = Visibility.Visible;
                openButton.Visibility = Visibility.Hidden;
            }

            var bc = new BrushConverter();
            locationField.Background = (Brush)bc.ConvertFrom("#EEEEEE");
        }
    
        private void SetFileLocation(object sender, RoutedEventArgs e)
        {
            locationField.Text = CrudFile.SetFileLocation();
        }

        private void CreateNewFile(object sender, RoutedEventArgs e)
        {
            CrudFile.CreateNewFile();
        }

        private void SaveConvertedFile(object sender, RoutedEventArgs e)
        {
            if (locationField.Text.Equals(""))
            {
                MessageBox.Show("Filelocation is empty!");
                return;
            }
            if (passField.Password.Equals(""))
            {
                MessageBox.Show("Passfiled is empty!");
                return;
            }
            if (!encStateBool)
            {
                CrudFile.SaveDecryptedFile(File.ReadAllBytes(locationField.Text), passField.Password);
            }
            else
            {
                if (passField.Password.Equals(passCheckField.Password))
                {
                    CrudFile.SaveEncryptedFile(File.ReadAllText(locationField.Text), passField.Password);
                }
                else
                    MessageBox.Show("Passwords are not same!");
            }
            CleanVariables();
        }
        private void OpenFile(object sender, RoutedEventArgs e)
        {
            if (locationField.Text.Equals(""))
            {
                MessageBox.Show("You haven't chosen a file!");
                return;
            }
            if (passField.Password.Equals(""))
            {
                MessageBox.Show("You haven't input a password!");
                return;
            }
            CrudFile.OpenFile(locationField.Text, passField.Password);
            CleanVariables();
        }

        private void CleanVariables()
        {
            locationField.Text = "";
            passField.Password = "";
            passCheckField.Password = "";
        }
    }
}
