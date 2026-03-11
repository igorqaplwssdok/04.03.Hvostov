using System.Windows;

namespace DEM_1Project
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow(string fullName, string role)
        {
            InitializeComponent();
            lblUserInfo.Text = $"Пользователь: {fullName} ({role})";
        }

        private void btnProducts_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProductsPage());
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OrdersPage());
        }

        private void btnBackToMain_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // При закрытии вызывающее окно (Admin/Manager) будет показано через обработчик Closed
        }
    }
}