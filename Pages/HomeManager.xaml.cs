using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SGSC.Pages
{
    /// <summary>
    /// Lógica de interacción para Home_manager.xaml
    /// </summary>
    public partial class Home_manager : Page
    {
        public List<string> CreditPolicy;
        public Home_manager()
        {
            InitializeComponent();
        }

        private void BtnClickManagerPolicyCredit(object sender, RoutedEventArgs e)
        {
            var ManageCreditPoliciesPage = new ManageCreditGrantingPolicies ();
            if (NavigationService != null)
            {
                NavigationService.Navigate(ManageCreditPoliciesPage);
            }
        }

        
    }
}
