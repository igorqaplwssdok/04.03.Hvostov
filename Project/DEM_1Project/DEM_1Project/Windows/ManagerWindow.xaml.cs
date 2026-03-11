using System;
using System.Windows;

namespace DEM_1Project
{
    public partial class ManagerWindow : Window
    {
        private string _fullName;

        public ManagerWindow(string fullName)
        {
            InitializeComponent();
            _fullName = fullName;
            lblUserInfo.Text = $"Пользователь: {fullName}";
        }

        private void btnManage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dashboard = new DashboardWindow(_fullName, "Менеджер");
                dashboard.Closed += (s, args) => this.Show();
                this.Hide();
                dashboard.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\n\n{ex.StackTrace}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}