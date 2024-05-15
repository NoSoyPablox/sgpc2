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
        private int currentPage = 0;
        private int pageSize = 7;
        private DispatcherTimer timer;

        }

        private int currentPage = 0;
        private int pageSize = 7;

        public ViewCreditRequests()
        {
            InitializeComponent();
            UserSessionFrame.Content = new UserSessionFrame();
            GetCreditRequests();
            tbRfc.TextChanged += tbRfc_TextChanged;
            tbCustomerName.TextChanged += tbCustomerName_TextChanged;
            tbStatus.TextChanged += tbStatus_TextChanged;
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
                                Rfc = cr.Rfc
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

        private void tbRfc_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFilteredResults();
        }

        private void tbCustomerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFilteredResults();
        }



        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
    }
