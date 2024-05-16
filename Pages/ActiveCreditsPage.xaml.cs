﻿using SGSC.Frames;
using SGSC.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static SGSC.Pages.ActiveCreditsPage;

namespace SGSC.Pages
{
    /// <summary>
    /// Interaction logic for ActiveCreditsPage.xaml
    /// </summary>
    public partial class ActiveCreditsPage : Page
    {
        public class ActiveCredit
        {
            public string CreditPageNumber { get; set; }
            public string ClientFullName { get; set; }
            public string CreditPeriod { get; set; }
            public string CreditAmount { get; set; }
            public string CreditPendingDebt { get; set; }
            public string CreditEfficiency { get; set; }
        }

        private ObservableCollection<ActiveCredit> ActiveCredits;
        private uint CurrentPage = 1;
        private uint TotalPages = 1;
        private const uint ItemsPerPage = 10;

        public ActiveCreditsPage()
        {
            InitializeComponent();
            UserSessionFrame.Content = new UserSessionFrame();
            GetActiveCredits();
        }

        private void UpdatePagination()
        {
            try
            {
                using (var context = new sgscEntities())
                {
                    var activeCreditsCount = context.CreditRequests.Count();
                    TotalPages = (uint)Math.Ceiling((double)activeCreditsCount / ItemsPerPage);
                    lbCurrentPage.Content = $"Página {CurrentPage}/{TotalPages}";
                    for(uint i = 0; i < TotalPages; i++)
                    {
                        cbPages.Items.Add(i);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar obtener los datos de los créditos activos: " + ex.Message);
            }
        }

        private void GetActiveCredits()
        {
            try
            {
                using (var context = new sgscEntities())
                {
                    var activeCredits = context.CreditRequests.ToList();
                    var activeCreditsArray = activeCredits.ToList();
                    ActiveCredits = new ObservableCollection<ActiveCredit>();
                    foreach (var item in activeCreditsArray)
                    {
                        ActiveCredits.Add(new ActiveCredit
                        {
                            CreditPageNumber = item.FileNumber,
                            ClientFullName = item.Customer.FullName,
                            CreditPeriod = item.TimePeriod.Value.ToString(),
                            CreditAmount = item.Amount.ToString(),
                            CreditPendingDebt = "0",
                            CreditEfficiency = "0%"
                        });
                    }
                    dgCredits.ItemsSource = ActiveCredits;
                }
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar obtener los datos de los créditos activos: " + ex.Message);
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserSession.LogOut();
        }

        private void AddAddressInformation(object sender, RoutedEventArgs e)
        {

        }
    }
}
