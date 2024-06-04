using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SGSC.Pages
{
    public partial class HomePageCreditAnalyst : Page
    {
        public HomePageCreditAnalyst()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var viewCreditRequests = new ViewCreditRequests();
            if (NavigationService != null)
            {
                NavigationService.Navigate(viewCreditRequests);
            }
            /*int requestId = 6;
            var customerInfoPage = new CreditApplicationDetailsRequest(requestId);
            if (NavigationService != null)
            {
                NavigationService.Navigate(customerInfoPage);
            }*/
        }

        private void btnCreditRequest_Click(object sender, RoutedEventArgs e)
        {
            var viewCreditRequests = new ViewCreditRequests();
            if (NavigationService != null)
            {
                NavigationService.Navigate(viewCreditRequests);
            }
        }
    }
}
