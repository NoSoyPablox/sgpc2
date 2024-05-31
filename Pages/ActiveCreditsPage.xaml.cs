using SGSC.Frames;
using SGSC.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.Win32;
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
        private class ActiveCredit
        {
            public string CreditPageNumber { get; set; }
            public string ClientFullName { get; set; }
            public string CreditPeriod { get; set; }
            public string CreditAmount { get; set; }
            public string CreditPendingDebt { get; set; }
            public string CreditEfficiency { get; set; }
            public int CreditRequestId { get; set; }


        }

        private ObservableCollection<ActiveCredit> ActiveCredits;
        private int CurrentPage = 1;
        private int TotalPages = 1;
        private const int ItemsPerPage = 10;
        private bool UpdatingPagination = false;

        public ActiveCreditsPage()
        {
            InitializeComponent();
            UserSessionFrame.Content = new UserSessionFrame();
            collectionExecutiveSidebar.Content = new CollectionExecutiveSidebar("activeCredits");
            GetActiveCredits();
        }

        private void UpdatePagination()
        {
            UpdatingPagination = true;

            try
            {
                using (var context = new sgscEntities())
                {
                    var activeCreditsCount = context.CreditRequests.Where(request => request.FileNumber.Contains(tbPageNumberFilter.Text) &&
                        (request.Customer.Name + " " + request.Customer.FirstSurname + " " + request.Customer.SecondSurname).Contains(tbCustomerNameFilter.Text) &&
                        request.Status == 4).Count();
                    TotalPages = (int)Math.Ceiling((double)activeCreditsCount / ItemsPerPage);
                    lbCurrentPage.Content = $"Página {CurrentPage}/{TotalPages}";
                    cbPages.Items.Clear();
                    for (uint i = 1; i <= TotalPages; i++)
                    {
                        cbPages.Items.Add(i);
                    }
                    cbPages.SelectedIndex = CurrentPage - 1;

                    btnNextPage.IsEnabled = CurrentPage < TotalPages;
                    btnPreviousPage.IsEnabled = CurrentPage > 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar obtener los datos de los créditos activos: " + ex.Message);
            }

            UpdatingPagination = false;

        }

        private void GetActiveCredits()
        {
            try
            {
                using (var context = new sgscEntities())
                {
                    var activeCredits = context.CreditRequests
                        .Where(request => request.FileNumber.Contains(tbPageNumberFilter.Text) &&
                        (request.Customer.Name + " " + request.Customer.FirstSurname + " " + request.Customer.SecondSurname).Contains(tbCustomerNameFilter.Text) &&
                        request.Status == 4)
                        .OrderBy(request => request.FileNumber)
                        .Skip((CurrentPage - 1) * ItemsPerPage)
                        .Take(ItemsPerPage)
                        .ToList();

					var activeCreditsArray = activeCredits.ToList();
                    ActiveCredits = new ObservableCollection<ActiveCredit>();
                    foreach (var item in activeCredits)
                    {
                        var efficiency = GetEfficiency(item.CreditRequestId);
                        ActiveCredits.Add(new ActiveCredit
                        {
                            CreditPageNumber = item.FileNumber,
                            ClientFullName = item.Customer.FullName,
                            CreditPeriod = item.TimePeriod.HasValue ? item.TimePeriod.Value.ToString() : "N/A",
                            CreditAmount = $"$ {item.Amount}",
                            CreditPendingDebt = $"Pendiente de calculo",
                            CreditEfficiency = $"{efficiency:F2}%",
                            CreditRequestId = item.CreditRequestId
                        });
                    }
                    dgCredits.ItemsSource = ActiveCredits;
                }
                UpdatePagination();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar obtener los datos de los créditos activos: " + ex.Message);
            }
        }

        private decimal GetEfficiency(int creditRequestId)
        {
            try
            {
                using (var context = new sgscEntities())
                {
                    var totalAmount = (decimal?)context.CreditRequests
                        .Where(cr => cr.CreditRequestId == creditRequestId)
                        .Select(cr => cr.Amount)
                        .FirstOrDefault() ?? 0m;

                    var totalCharged = context.Payments
                        .Where(p => p.CreditRequestId == creditRequestId)
                        .Sum(p => (decimal?)p.AmountCharged ?? 0m);

                    var efficiency = totalAmount != 0m ? (totalCharged / totalAmount) * 100 : 0;
                    return efficiency;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving efficiency data: {ex.Message}");
                return 0;
            }
        }



        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserSession.LogOut();
        }

        private void tbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetActiveCredits();
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage++;
            GetActiveCredits();
        }

        private void btnPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage--;
            GetActiveCredits();
        }

        private void cbPages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UpdatingPagination)
            {
                return;
            }
            CurrentPage = cbPages.SelectedIndex + 1;
            GetActiveCredits();
        }

        private void GenerateCollectionLayout(object sender, RoutedEventArgs e)
        {
            if (dpStartDate.SelectedDate == null || dpEndDate.SelectedDate == null)
            {
                MessageBox.Show("Por favor, selecciona un rango de fechas válido.");
                return;
            }

            DateTime startDate = dpStartDate.SelectedDate.Value;
            DateTime endDate = dpEndDate.SelectedDate.Value;

            if (startDate > endDate)
            {
                MessageBox.Show("La fecha de inicio no puede ser mayor que la fecha de fin.");
                return;
            }

            try
            {
                using (var context = new sgscEntities())
                {
                    var activeCredits = from lc in context.LayoutPayments
                                        where lc.Status == 4 && lc.PaymentDate >= startDate && lc.PaymentDate <= endDate
                                        select new
                                        {
                                            lc.FileNumber,
                                            lc.PaymentDate,
                                            lc.Amount,
                                            lc.Name,
                                            lc.InterbankCode
                                        };

                    string csvFilePath = "C:\\Users\\wero1\\Documents\\Layout\\ActiveCreditsCollectionLayout.csv";

                    using (var writer = new StreamWriter(csvFilePath))
                    {
                        writer.WriteLine("Folio,ImporteACobrar,FechaPago,Cuenta,Banco");

                        foreach (var credit in activeCredits)
                        {
                            string folio = credit.FileNumber;
                            string importeACobrar = credit.Amount.HasValue ? credit.Amount.Value.ToString("F2") : "0.00";
                            string fechaPago = credit.PaymentDate.HasValue ? credit.PaymentDate.Value.ToString("yyyy-MM-dd") : "0.00";
                            string cuenta = credit.InterbankCode;
                            string banco = credit.Name;

                            writer.WriteLine($"{folio},{importeACobrar},{fechaPago},{cuenta},{banco}");
                        }
                    }

                    MessageBox.Show("El layout de cobro ha sido generado exitosamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar generar el layout de cobro: " + ex.Message);
            }
        }

    }
}
  
    }
   }
  
