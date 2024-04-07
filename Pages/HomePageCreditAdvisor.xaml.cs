using SGSC.Utils;
using System.Windows;
using System.Windows.Controls;

namespace SGSC.Pages
{
    public partial class HomePageCreditAdvisor : Page
    {
        public HomePageCreditAdvisor()
        {
            InitializeComponent();
        }

        private void ButtonClicNuevo_Cliente(object sender, RoutedEventArgs e)
        {            
            var customerInfoPage = new CustomerInfoPage();
            if (NavigationService != null)
            {
                NavigationService.Navigate(customerInfoPage);
            }
            
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserSession.LogOut();
        }
    }
}
