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
    /// Interaction logic for SearchClientPage.xaml
    /// </summary>
    public partial class SearchCustomerPage : Page
    {
        public SearchCustomerPage()
        {
            InitializeComponent();
            creditAdvisorSidebar.Content = new Frames.CreditAdvisorSidebar("searchCustomer");
		}

		private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
	}
}
