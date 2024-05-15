using SGSC.Frames;
using SGSC.Utils;
using System;
using System.Collections.Generic;
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
    using System.Globalization;


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
        }

        private void NextPageRequest(object sender, RoutedEventArgs e)
        {
            try
            {
                currentPage++;
                if (currentPage < 0)
                {
                    currentPage = 0;
                }
                GetCreditRequests();
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
                MessageBox.Show("Ocurrió un error al intentar obtener las solicitudes de crédito. Por favor, inténtelo de nuevo más tarde.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
    }
