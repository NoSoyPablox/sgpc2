using SGSC.Utils;
using System.Windows;
using System.Windows.Controls;

namespace SGSC.Pages
{
    public partial class HomePageAdmin : Page
    {
        public HomePageAdmin()
        {
            InitializeComponent();
            creditAdvisorSidebar.Content = new Frames.AdminSidebar("home");
        }

        private void btnViewActiveCredits_Click(object sender, RoutedEventArgs e)
        {
            var activeCreditsPage = new ManageEmployeesPage();
            if (NavigationService != null)
            {
                NavigationService.Navigate(activeCreditsPage);
            }

        }
    }
}
