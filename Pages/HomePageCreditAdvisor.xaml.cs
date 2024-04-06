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
            /*
            PageContactInformation contactInformationPage = new PageContactInformation(true, 1);
            if (NavigationService != null)
            {
                NavigationService.Navigate(contactInformationPage);
            }*/

            
            PageWorkCenter workCenterPage = new PageWorkCenter(false, 1);
            if (NavigationService != null)
            {
                NavigationService.Navigate(workCenterPage);
            }
            
        }
    }
}
