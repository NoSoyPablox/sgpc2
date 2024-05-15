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
            public DateTime TimePeriod { get; set; }

        }

        private int currentPage = 1;
        private int pageSize = 8;
        private int totalRecords = 0;
        private int totalPages = 0;

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
            tbRfc.TextChanged += tbRfc_TextChanged;
            tbCustomerName.TextChanged += tbCustomerName_TextChanged;
            tbStatus.TextChanged += tbStatus_TextChanged;
        }

        private void NextPageRequest(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentPage < totalPages)
                {
                    currentPage++;
                    GetCreditRequests();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al intentar obtener las SIGUIENTES solicitudes de crédito. Por favor, inténtelo de nuevo más tarde.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        }

        private void PreviousPageRequest(object sender, RoutedEventArgs e)
        {
            try
            {
                currentPage--;
                if (currentPage < 0)
                {
                    var creditRequests = (from cr in db.CreditRequests
                                          join c in db.Customers on cr.CustomerId equals c.CustomerId
                                          where c.Name.Contains(searchText1) &&
                                                c.FirstSurname.Contains(searchText2) &&
                                                c.SecondSurname.Contains(searchText3)
                                          select new
                                          {
                                              cr.CreditRequestId,
                                              cr.Status,
                                              cr.CreationDate,
                                              CustomerName = c.Name + " " + c.FirstSurname + " " + c.SecondSurname,
                                              c.Rfc,
                                          }).Skip(currentPage * pageSize).Take(pageSize).ToList();

                    if (creditRequests.Any())
                    {
                        creditRequestsDataGrid.ItemsSource = creditRequests;
                    }
                    else
                    {
                        currentPage--;
                    }
                }
            }
            catch (Exception ex)
            {
                currentPage = (int)cbPages.SelectedItem;
                GetCreditRequests();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            timer.Stop(); 
            timer.Start();
        }


        private void GetCreditRequests()
        {

            try
            {
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
                                              cr.FileNumber,
                                              cr.Amount,
                                              cr.InterestRate,
                                              cr.TimePeriod,
                                          })
                                          .OrderBy(cr => cr.CreditRequestId)
                                          .Skip((currentPage - 1) * pageSize)
                                          .Take(pageSize)
                                          .ToList();
                    if (creditRequests.Any())
                    {
                        creditRequestsDataGrid.ItemsSource = creditRequests;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Sex Ocurrió un error al intentar obtener las solicitudes de crédito. Por favor, inténtelo de nuevo más tarde.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    var creditRequests = (from cr in db.CreditRequests
                                          join c in db.Customers on cr.CustomerId equals c.CustomerId
                                          select new
                                          {
                                              cr.CreditRequestId,
                                              cr.Status,
                                              cr.CreationDate,
                                              CustomerName = c.Name + " " + c.FirstSurname + " " + c.SecondSurname,
                                              c.Rfc,
                                          }).Skip(currentPage * pageSize).Take(pageSize).ToList();

                    if (creditRequests.Any())
                    {
                        creditRequestsDataGrid.ItemsSource = creditRequests;
                    }
                    else
                    {
                        currentPage--;
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

        private void tbRfc_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFilteredResults();
        }

        private void tbCustomerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFilteredResults();
        }

        private void tbStatus_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFilteredResults();
        }

        private void UpdateFilteredResults()
        {
            try
            {
                using (sgscEntities db = new sgscEntities())
                {
                    var filteredRequests = (from cr in db.CreditRequests
                                            join c in db.Customers on cr.CustomerId equals c.CustomerId
                                            where (string.IsNullOrEmpty(tbRfc.Text) || c.Rfc.Like(tbRfc.Text))
                                            && (string.IsNullOrEmpty(tbCustomerName.Text) || (c.Name + " " + c.FirstSurname + " " + c.SecondSurname).Like(tbCustomerName.Text))
                                            && (string.IsNullOrEmpty(tbStatus.Text) || cr.Status.ToString().Like(tbStatus.Text))
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

                    if (filteredRequests.Any())
                    {
                        ObservableCollection<CreditRequestData> creditRequestsData = new ObservableCollection<CreditRequestData>();
                        foreach (var cr in filteredRequests)
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
                                InterestRate = cr.InterestRate.HasValue ? cr.InterestRate.Value : 0.0m,
                                TimePeriod = cr.TimePeriod.Value,
                            });
                        }

                        creditRequestsDataGrid.ItemsSource = creditRequestsData;
                    }
                    else
                    {
                        creditRequestsDataGrid.ItemsSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al intentar obtener las solicitudes de crédito filtradas. Por favor, inténtelo de nuevo más tarde.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {

        }


        private void HomePageCreditAdvisorMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePageCreditAdvisor());
        }
    }
    }
