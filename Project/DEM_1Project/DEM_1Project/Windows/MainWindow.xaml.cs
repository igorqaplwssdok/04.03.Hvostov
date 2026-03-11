using System;
using System.Windows;

namespace DEM_1Project
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
            roleWindow.Show(); // просто показываем окно роли
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password.Trim();

            var (success, role, fullName) = DatabaseHelper.CheckUser(login, password);

            if (success)
            {
                OpenRoleWindow(role, fullName);
                this.Close(); // закрываем окно входа
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnGuest_Click(object sender, RoutedEventArgs e)
        {
            GuestWindow guestWindow = new GuestWindow();
            guestWindow.Closed += (s, args) => this.Show();
            this.Hide();
            guestWindow.Show();
        }
    }
}