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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DEM_1Project
{
    public partial class MainWindow : Window
    {   
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password.Trim();

            var (success, role, fullName) = DatabaseHelper.CheckUser(login, password);

            if (success)
            {
                OpenRoleWindow(role, fullName);
                this.Hide();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenRoleWindow(string role, string fullName)
        {
            Window roleWindow = null;
            switch (role)
            {
                case "Администратор":
                    roleWindow = new AdminWindow(fullName);
                    break;
                case "Менеджер":
                    roleWindow = new ManagerWindow(fullName);
                    break;
                case "Авторизированный клиент":
                    roleWindow = new ClientWindow(fullName);
                    break;
                default:
                    MessageBox.Show("Неизвестная роль.");
                    return;
            }
            roleWindow.Show();
        }

        private void btnGuest_Click(object sender, RoutedEventArgs e)
        {
            GuestWindow guestWindow = new GuestWindow();
            guestWindow.Show();
            this.Hide();
        }
    }
}
