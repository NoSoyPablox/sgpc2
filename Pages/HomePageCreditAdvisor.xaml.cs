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
            var customerInfoPage = new CustomerInfoPage(1);
            if (NavigationService != null)
            {
                NavigationService.Navigate(customerInfoPage);
            }
            
        }



        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserSession.LogOut();
        }

        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        private void ViewRequestsButton_Click(object sender, RoutedEventArgs e)
        {
         var viewCreditRequests = new ViewCreditRequests();
            if (NavigationService != null)
            {
                NavigationService.Navigate(viewCreditRequests);
            }
        }
    }
}
