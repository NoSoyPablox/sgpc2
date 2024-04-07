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
            CreditAdvisorName.Text = UserSession.Instance.FullName;
        }

        private void ButtonClicNuevo_Cliente(object sender, RoutedEventArgs e)
        {            
            PageWorkCenter workCenterPage = new PageWorkCenter(false, 1);
            if (NavigationService != null)
            {
                NavigationService.Navigate(workCenterPage);
            }
            
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserSession.LogOut();
        }
    }
}
