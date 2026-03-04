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

namespace DEM_1Project
{
    public partial class ManagerWindow : Window
    {
        public ManagerWindow(string fullName)
        {
            InitializeComponent();
            lblUserInfo.Text = $"Пользователь: {fullName}";
        }

        private void btnLogout_Click(object Sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
