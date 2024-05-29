﻿using SGSC.Utils;
using System.Windows;
using System.Windows.Controls;

namespace SGSC.Pages
{
    public partial class HomePageCreditAdvisor : Page
    {
        public HomePageCreditAdvisor()
        {
            InitializeComponent();
            creditAdvisorSidebar.Content = new Frames.CreditAdvisorSidebar("home");
        }

        private void ButtonClicNuevo_Cliente(object sender, RoutedEventArgs e)
        {
            var customerInfoPage = new CustomerInfoPage();
            if (NavigationService != null)
            {
                NavigationService.Navigate(customerInfoPage);
            }

        }

        private void btnNewRequest_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainFrame.Content = new Pages.SearchCustomerPage();
        }
    }
}
