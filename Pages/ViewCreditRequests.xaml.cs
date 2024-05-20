    using SGSC.Frames;
    using SGSC.Utils;
    using System;
    using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
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
    using System.Windows.Threading;
    

namespace SGSC.Pages
{
    /// <summary>
    /// Interaction logic for ViewCreditRequests.xaml
    /// </summary>
    public partial class ViewCreditRequests : Page
    {
        private class CreditRequestData
        {
            public int CreditRequestId { get; set; }
            public string Status { get; set; }
            public DateTime CreationDate { get; set; }
            public string CustomerName { get; set; }
            public string Rfc { get; set; }
            public string FileNumber { get; set; }
            public double Amount { get; set; }
            public string AmountString { get; set; }
            public decimal InterestRate { get; set; }
            public string InterestRateString { get; set; }
            public int TimePeriod { get; set; }

            public string TimePeriodString { get; set; }

        }

        private int currentPage = 1;
        private int pageSize = 8;
        private int totalRecords = 0;
        private int totalPages = 0;
        ObservableCollection<CreditRequestData> creditRequestsDataAux;

        public ViewCreditRequests()
        {
            InitializeComponent();
            UserSessionFrame.Content = new UserSessionFrame();
            GetCreditRequests();
            GetAllCreditRequests();
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop(); 
            SearchCreditRequests();
        }

        private void SearchCreditRequests()
        {
            try
            {
                currentPage++;
                if (currentPage < 0)
                {
                    currentPage++;
                    GetCreditRequests();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al intentar buscar las solicitudes de crédito. Por favor, inténtelo de nuevo más tarde.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void GetCreditRequests()
        {

            try
            {
                currentPage--;
                if (currentPage < 0)
                {
                    currentPage = 0;
                }
                GetCreditRequests();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al intentar obtener las solicitudes de crédito. Por favor, inténtelo de nuevo más tarde.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserSession.LogOut();
        }

        private void NextPageRequest(object sender, RoutedEventArgs e)
        {

            try
            {
                currentPage++;

                using (sgscEntities db = new sgscEntities())
                {
                    int totalRecords = db.CreditRequests.Count();
                    totalPages = (totalRecords + pageSize - 1) / pageSize;

                    var creditRequests = (from cr in db.CreditRequests
                                          join c in db.Customers on cr.CustomerId equals c.CustomerId
                                          select new
                                          {
                                              cr.CreditRequestId,
                                              cr.Status,
                                              cr.CreationDate,
                                              CustomerName = c.Name + " " + c.FirstSurname + " " + c.SecondSurname,
                                              c.Rfc,
                                          }).Take(pageSize).ToList();
                    if (creditRequests.Any())
                    {
                        ObservableCollection<CreditRequestData> creditRequestsData = new ObservableCollection<CreditRequestData>();
                        foreach (var cr in creditRequests)
                        {
                            creditRequestsData.Add(new CreditRequestData
                            {
                                CreditRequestId = cr.CreditRequestId,
                                Status = CreditRequest.RequestStatusToString((CreditRequest.RequestStatus)cr.Status),
                                CreationDate = cr.CreationDate.Value,
                                CustomerName = cr.CustomerName,
                                Rfc = cr.Rfc,
                                FileNumber = cr.FileNumber,
                                Amount = cr.Amount.HasValue ? cr.Amount.Value : 0.0,
                                AmountString = cr.Amount.HasValue ? cr.Amount.Value.ToString("C2") : "$0.00",
                                InterestRate = cr.InterestRate.HasValue ? cr.InterestRate.Value : 0.0m,
                                InterestRateString = cr.InterestRate.HasValue ? $"{cr.InterestRate.Value}%" : "0.0%",
                                TimePeriod = cr.TimePeriod.HasValue ? cr.TimePeriod.Value : 0,
                                TimePeriodString = cr.TimePeriod.HasValue ? $"{cr.TimePeriod.Value} Quincenas" : "Quincenas"
                            });
                        }

                        creditRequestsDataGrid.ItemsSource = creditRequestsData;

                    }

                    tbCurrentPage.Text = $"Página {currentPage} de {totalPages}";
                    cbPages.ItemsSource = Enumerable.Range(1, totalPages).ToList();
                    cbPages.SelectedItem = currentPage;

                    btnPreviousPage.IsEnabled = currentPage > 1;
                    btnNextPage.IsEnabled = currentPage < (totalRecords + pageSize - 1) / pageSize;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al intentar obtener las solicitudes de crédito. Por favor, inténtelo de nuevo más tarde.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void GetAllCreditRequests()
        {
            try
            {
                using (sgscEntities db = new sgscEntities())
                {

                    var creditRequests = (from cr in db.CreditRequests
                                          join c in db.Customers on cr.CustomerId equals c.CustomerId
                                          select new
                                          {
                                              cr.CreditRequestId,
                                              cr.Status,
                                              cr.CreationDate,
                                              CustomerName = c.Name + " " + c.FirstSurname + " " + c.SecondSurname,
                                              c.Rfc,
                                              cr.FileNumber,
                                              cr.Amount,
                                              cr.InterestRate,
                                              cr.TimePeriod,
                                          })
                                          .OrderBy(cr => cr.CreditRequestId)
                                          .ToList();
                    if (creditRequests.Any())
                    {
                        ObservableCollection<CreditRequestData> creditRequestsData = new ObservableCollection<CreditRequestData>();
                        foreach (var cr in creditRequests)
                        {
                            creditRequestsData.Add(new CreditRequestData
                            {
                                CreditRequestId = cr.CreditRequestId,
                                Status = CreditRequest.RequestStatusToString((CreditRequest.RequestStatus)cr.Status),
                                CreationDate = cr.CreationDate.Value,
                                CustomerName = cr.CustomerName,
                                Rfc = cr.Rfc,
                                FileNumber = cr.FileNumber,
                                Amount = cr.Amount.HasValue ? cr.Amount.Value : 0.0,
                                AmountString = cr.Amount.HasValue ? cr.Amount.Value.ToString("C2") : "$0.00",
                                InterestRate = cr.InterestRate.HasValue ? cr.InterestRate.Value : 0.0m,
                                InterestRateString = cr.InterestRate.HasValue ? $"{cr.InterestRate.Value}%" : "0.0%",
                                TimePeriod = cr.TimePeriod.Value,
                                TimePeriodString = cr.TimePeriod.Value.ToString()
                            });
                        }

                        creditRequestsDataAux = creditRequestsData;
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al intentar obtener las solicitudes de crédito. Por favor, inténtelo de nuevo más tarde.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_CreditRequestSearchStatus(object sender, EventArgs e)
        {
            string textSearch = tbStatus.Text;
            FilterCreditRequests(textSearch, "Status");
        }

        private void TextBox_CreditRequestSearchCustomerName(object sender, EventArgs e)
        {
            string textSearch = tbCustomerName.Text;
            FilterCreditRequests(textSearch, "Customer Name");
        }

        private void TextBox_CreditRequestSearchCustomerRfc(object sender, EventArgs e)
        {
            string textSearch = tbRfc.Text;
            FilterCreditRequests(textSearch, "RFC");
        }


        private void FilterCreditRequests(string textSearch, string type)
        {
            List<CreditRequestData> filteredRequests = null;
            switch (type)
            {
                case "Status":
                    filteredRequests = creditRequestsDataAux
                    .Where(c => c.Status.ToLower().Contains(textSearch.ToLower()))
                    .ToList();
                    break;
                case "Customer Name":
                    filteredRequests = creditRequestsDataAux
                    .Where(c => c.CustomerName.ToLower().Contains(textSearch.ToLower()))
                    .ToList();

                    break;
                case "RFC":
                    filteredRequests = creditRequestsDataAux
                    .Where(c => c.Rfc.ToLower().Contains(textSearch.ToLower()))
                    .ToList();
                    break;
            }

            ObservableCollection<CreditRequestData> creditRequestsDataAux2 = new ObservableCollection<CreditRequestData>(filteredRequests);
            creditRequestsDataGrid.ItemsSource = creditRequestsDataAux2;


        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ViewCreditRequestsDetails(object sender, RoutedEventArgs e)
        {
            CreditRequestData creditRequestData = (CreditRequestData)creditRequestsDataGrid.SelectedItem;
            if (creditRequestData != null)
            {
                /*var creditRequestDetails = new CreditRequestDetails(creditRequestData.CreditRequestId);
                 if (NavigationService != null)
                 {
                     NavigationService.Navigate(creditRequestDetails);
                 }*/
                //Pendiente agregar la página CreditRequestDetails
            }
        }

        private void CorrectCredtiRequests(object sender, RoutedEventArgs e)
        {
            CreditRequestData selectedCreditRequest = (CreditRequestData)creditRequestsDataGrid.SelectedItem;
            if (selectedCreditRequest != null)
            {
                NavigationService.Navigate(new CollectionEfficienciesPage(selectedCreditRequest.CreditRequestId));
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una solicitud de crédito para Consultar .", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }




            /*var creditRequestDetails = new correctCreditRequest(creditRequestData.CreditRequestId);
             if (NavigationService != null)
             {
                 NavigationService.Navigate(correctCreditRequest);
             }*/
            //Pendiente agregar la página para Corregir solicitudes de crédito

        }
    }
}