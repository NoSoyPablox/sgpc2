using System.Windows;
using System.Windows.Controls;

namespace SGSC.Pages
{
    /// <summary>
    /// Lógica de interacción para HomePageCreditAdvisor.xaml
    /// </summary>
    public partial class HomePageCreditAdvisor : Page
    {
        public HomePageCreditAdvisor()
        {
            InitializeComponent();
        }

        private void ButtonClicNuevo_Cliente(object sender, RoutedEventArgs e)
        {
            // Crear una instancia de la página WorkCenter
            PageWorkCenter workCenterPage = new PageWorkCenter(false, 2);
            if (NavigationService != null)
            {
                NavigationService.Navigate(workCenterPage);
            }
        }
    }
}
