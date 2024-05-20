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
            public int Term { get; set; }
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
            public decimal Discount { get; set; }
            public decimal Amount { get; set; }
            public decimal Efficiency { get; set; }
            public bool IsTotalRow { get; set; }

            public string EfficiencyString => $"{Efficiency:F2}%";
        }

        public CollectionEfficienciesPage(int creditRequestId)
        {
            InitializeComponent();
            CreditRequestId = creditRequestId;
            UserSessionFrame.Content = new UserSessionFrame();
            this.Loaded += CollectionEfficienciesPage_Loaded;
            public int CreditRequestID { get; set; }
            public string Folio { get; set; }
            public string ClientName { get; set; }
            public int Term { get; set; }
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
            public decimal Discount { get; set; }
            public decimal Amount { get; set; }
            public decimal Efficiency { get; set; }
            public bool IsTotalRow { get; set; }

            var creditRequest = GetCreditRequestById(CreditRequestId);
            if (creditRequest != null)
            {
                DisplayCreditRequestDetails(creditRequest);
                DisplayPaymentSchedule(creditRequest);
            }
            else
            {
                MessageBox.Show("Solicitud de crédito no encontrada.");
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
                                             InterestRate = cr.InterestRate ?? 0.0m,
                                             Payments = db.Payments
                                                          .Where(p => p.CreditRequestId == cr.CreditRequestId)
                                                          .Select(p => new
                                                          {
                                                              p.PaymentId,
                                                              p.CreditRequestId,
                                                              p.FileNumber,
                                                              p.PaymentDate,
                                                              p.Amount
                                                          }).ToList()
                                         }).FirstOrDefault();

                    if (creditRequest != null)
                    {
                        decimal totalAmount = creditRequest.Amount.HasValue ? Convert.ToDecimal(creditRequest.Amount.Value) : 0.0m;
                        decimal outstandingBalance = totalAmount * (1 - creditRequest.InterestRate / 100);

                        var payments = creditRequest.Payments.Select(p => new Payment
                        {
                            PaymentID = p.PaymentId,
                            CreditRequestID = p.CreditRequestId ?? 0,
                            FileNumber = p.FileNumber,
                            PaymentDate = p.PaymentDate ?? DateTime.MinValue,
                            Amount = !string.IsNullOrEmpty(p.Amount) ? Convert.ToDecimal(p.Amount) : 0.0m,
                            Discount = 0.0m,
                            Efficiency = 0.0m,
                            IsTotalRow = false
                        }).ToList();

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
            decimal biweeklyPayment = creditRequest.TotalAmount / creditRequest.Term;

            for (int i = 1; i <= creditRequest.Term; i++)
            {
                Payment payment = new Payment
                {
                    CreditRequestID = creditRequest.CreditRequestID,
                    FileNumber = i.ToString(),
                    PaymentDate = DateTime.Now.AddDays(15 * i),
                    Discount = biweeklyPayment,
                    Amount = biweeklyPayment, 
                    Efficiency = 0.0m 
                };

                paymentSchedule.Add(payment);
            }

            return paymentSchedule;
        }

        private void DisplayPaymentSchedule(CreditRequest creditRequest)
        {
            var paymentSchedule = GeneratePaymentSchedule(creditRequest);
            CalculateEfficiency(paymentSchedule);

            creditRequestsDataGrid.ItemsSource = paymentSchedule;
        }

        private void CalculateEfficiency(List<Payment> paymentSchedule)
        {
            foreach (var payment in paymentSchedule)
            {
                if (payment.Discount != 0)
                {
                    payment.Efficiency = (payment.Amount / payment.Discount) * 100;
                }
                else
                {
                    payment.Efficiency = 0.0m;
                }
            }

            AddTotalRow(paymentSchedule);
        }

        private void AddTotalRow(List<Payment> paymentSchedule)
        {
            var totalDiscount = paymentSchedule.Sum(p => p.Discount);
            var totalAmount = paymentSchedule.Sum(p => p.Amount);
            var totalEfficiency = paymentSchedule.Average(p => p.Efficiency);

            Payment totalRow = new Payment
            {
                FileNumber = "Cobro total",
                Discount = totalDiscount,
                Amount = totalAmount,
                Efficiency = totalEfficiency,
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
            InitializeComponent();
        }
    }
}
