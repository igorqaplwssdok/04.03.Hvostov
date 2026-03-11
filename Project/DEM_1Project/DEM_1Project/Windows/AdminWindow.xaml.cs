using System.Windows;

namespace DEM_1Project
{
    public partial class AdminWindow : Window
    {
        private string _fullName; 
        public AdminWindow(string fullName)
        {
            InitializeComponent();
            _fullName = fullName;
            lblUserInfo.Text = $"Пользователь: {fullName}";
        }

        private void btnManage_Click(object sender, RoutedEventArgs e)
        {
            var dashboard = new DashboardWindow(_fullName, "Администратор");
            dashboard.Closed += (s, args) => this.Show();
            this.Hide();
            dashboard.Show();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();  // сначала показываем новое окно входа
            this.Close();       // затем закрываем текущее окно администратора
        }
    }
}