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

        }

        private int currentPage = 1;
        private int pageSize = 8;
        private int totalRecords = 0;
        private int totalPages = 0;
        ObservableCollection<CreditRequestData> creditRequestsDataAux;

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
                using (sgscEntities db = new sgscEntities())
                {
                    currentPage = 0;
                }
                GetCreditRequests();
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
