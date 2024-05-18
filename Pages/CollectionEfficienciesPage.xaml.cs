using SGSC.Utils;
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
using static SGSC.Pages.CollectionEfficienciesPage;

namespace SGSC.Pages
{
    /// <summary>
    /// Interaction logic for CollectionEfficienciesPage.xaml
    /// </summary>
    public partial class CollectionEfficienciesPage : Page
    {
        public int CreditRequestId { get; set; }

        public class CreditRequest
        {
            public int CreditRequestID { get; set; }
            public string Folio { get; set; }
            public string ClientName { get; set; }
            public int Term { get; set; } // Plazo en quincenas
            public decimal TotalAmount { get; set; }
            public string TotalAmountString { get; set; }
            public decimal OutstandingBalance { get; set; }
            public decimal Efficiency { get; set; }
        }

        public class Payment
        {
            public int PaymentID { get; set; }
            public int CreditRequestID { get; set; }
            public string FileNumber { get; set; }
            public DateTime PaymentDate { get; set; }
            public decimal Discount { get; set; } // El descuento que corresponde a pagar en ese pago
            public decimal Charge { get; set; } // Lo que se pagaría en esa quincena
            public decimal Efficiency { get; set; } // El % calculado de todos los porcentajes
            public bool IsTotalRow { get; set; } // Indica si es una fila de totales
        }

        public CollectionEfficienciesPage(int creditRequestId)
        {
            InitializeComponent();
            CreditRequestId = creditRequestId;
            this.Loaded += CollectionEfficienciesPage_Loaded;
        }

        private void CollectionEfficienciesPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Load the CreditRequest details based on the provided CreditRequestId
            var creditRequest = GetCreditRequestById(CreditRequestId);
            if (creditRequest != null)
            {
                DisplayCreditRequestDetails(creditRequest);
                DisplayPaymentSchedule(creditRequest);
            }
            else
            {
                MessageBox.Show("Solcitud de crédito no encontrada.");
            }
        }



        private CreditRequest GetCreditRequestById(int creditRequestId)
        {
            try
            {
                using (sgscEntities db = new sgscEntities())
                {
                    var creditRequest = (from cr in db.CreditRequests
                                         join c in db.Customers on cr.CustomerId equals c.CustomerId
                                         where cr.CreditRequestId == creditRequestId
                                         select new
                                         {
                                             cr.CreditRequestId,
                                             cr.FileNumber,
                                             ClientName = c.Name + " " + c.FirstSurname + " " + c.SecondSurname,
                                             cr.TimePeriod,
                                             cr.Amount,
                                             InterestRate = cr.InterestRate ?? 0.0m // Handle null interest rate
                                         }).FirstOrDefault();

                    if (creditRequest != null)
                    {
                        decimal totalAmount = creditRequest.Amount.HasValue ? Convert.ToDecimal(creditRequest.Amount.Value) : 0.0m;
                        decimal outstandingBalance = totalAmount * (1 - creditRequest.InterestRate / 100);

                        CreditRequest mappedCreditRequest = new CreditRequest
                        {
                            CreditRequestID = creditRequest.CreditRequestId,
                            Folio = creditRequest.FileNumber,
                            ClientName = creditRequest.ClientName,
                            Term = creditRequest.TimePeriod ?? 0,
                            TotalAmount = totalAmount,
                            TotalAmountString = totalAmount.ToString("C"),
                            OutstandingBalance = outstandingBalance,
                            Efficiency = 0.0m
                        };

                        return mappedCreditRequest;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving Credit Request: {ex.Message}");
                return null;
            }
        }

        private void DisplayCreditRequestDetails(CreditRequest creditRequest)
        {
            lblCustomerName.Content = creditRequest.ClientName;
            lblFileNumber.Content = creditRequest.Folio;
            lblCreditAmount.Content = creditRequest.TotalAmountString;
        }


        public List<Payment> GeneratePaymentSchedule(CreditRequest creditRequest)
        {
            List<Payment> paymentSchedule = new List<Payment>();
            decimal biweeklyInterestRate = 0.05m; 
            decimal biweeklyPayment = creditRequest.TotalAmount / creditRequest.Term;

            for (int i = 1; i <= creditRequest.Term; i++)
            {
                Payment payment = new Payment
                {
                    CreditRequestID = creditRequest.CreditRequestID,
                    FileNumber = i.ToString(),
                    PaymentDate = DateTime.Now.AddDays(15 * i), 
                    Discount = biweeklyPayment,
                    Charge = biweeklyPayment + (creditRequest.TotalAmount * biweeklyInterestRate),
                    Efficiency = (biweeklyPayment + (creditRequest.TotalAmount * biweeklyInterestRate)) / biweeklyPayment * 100
                };

                paymentSchedule.Add(payment);
            }

            return paymentSchedule;
        }

        private void DisplayPaymentSchedule(CreditRequest creditRequest)
        {
            var paymentSchedule = GeneratePaymentSchedule(creditRequest);
            AddTotalRow(paymentSchedule);

            creditRequestsDataGrid.ItemsSource = paymentSchedule;
        }

        private void AddTotalRow(List<Payment> paymentSchedule)
        {
            var totalDiscount = paymentSchedule.Sum(p => p.Discount);
            var totalCharge = paymentSchedule.Sum(p => p.Charge);
            var totalEfficiency = totalCharge / totalDiscount; 

            Payment totalRow = new Payment
            {
                FileNumber = "Cobro total",
                Discount = totalDiscount,
                Charge = totalCharge,
                Efficiency = totalEfficiency * 100, 
                IsTotalRow = true
            };

            paymentSchedule.Add(totalRow);
        }


        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserSession.LogOut();
        }


        private void HomePageCreditAdvisorMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePageCreditAdvisor());
        }

    }
}
