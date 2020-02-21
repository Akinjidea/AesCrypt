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
        public Cryption()
        {
            InitializeComponent();
            if (!MainWindow.emptyFileBool.GetValueOrDefault())
            {
                saveNewFileButton.Visibility = Visibility.Visible;
                showPassPanelButton.Visibility = Visibility.Collapsed;
            }
            if (!MainWindow.encStateBool)
            {
                passCheckLabel.Visibility = Visibility.Collapsed;
                passCheckField.Visibility = Visibility.Collapsed;
            }
        }

        private void SaveDataToFile(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.encStateBool)
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
            passField.Password = "";
            passCheckField.Password = "";
        }
        private void OpenDataLocal(object sender, RoutedEventArgs e)
        {

        }
        private void BackToMainMenu(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void ShowPassPanel(object sender, RoutedEventArgs e)
        {
            passPanel.Visibility = Visibility.Visible;
            showPassPanelButton.Visibility = Visibility.Collapsed;
            hidePassPanelButton.Visibility = Visibility.Visible;
        }
        private void HidePassPanel(object sender, RoutedEventArgs e)
        {
            passPanel.Visibility = Visibility.Collapsed;
            showPassPanelButton.Visibility = Visibility.Visible;
            hidePassPanelButton.Visibility = Visibility.Collapsed;
        }
        private void SaveNewFile(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Successfully!");
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
