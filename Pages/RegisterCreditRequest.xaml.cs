using SGSC.Frames;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace SGSC.Pages
{
    /// <summary>
    /// Lógica de interacción para RegisterCreditRequest.xaml
    /// </summary>
    public partial class RegisterCreditRequest : Page
    {
        private int idCustomer = -1;
        private int idCreditRequest = -1;
        private double totalAmount = 0.0;
        private CreditRequest CreditRequest;
        private string fileNumber = "";
        public RegisterCreditRequest(int idCustomer, int idCreditRequest)
        {
            InitializeComponent();
            btnModifyCustomer.IsEnabled = false;
            btnModifyCustomer.Visibility = Visibility.Hidden;
            btnModifyCustomerAccounts.IsEnabled = false;
            btnModifyCustomerAccounts.Visibility = Visibility.Hidden;
            UserSessionFrame.Content = new UserSessionFrame();

            this.idCustomer = idCustomer;
            this.idCreditRequest = idCreditRequest;
            retrieveCreditPromotions();
            if (idCreditRequest != -1)
            {
                retrieveCreditRequestData();
                retrieveCredidPromotionSelectedIfAvailable();
                btnModifyCustomer.IsEnabled = true;
                btnModifyCustomer.Visibility = Visibility.Visible;
                btnModifyCustomerAccounts.IsEnabled = true;
                btnModifyCustomerAccounts.Visibility = Visibility.Visible;
                btnCancel.Content = "Volver";
                btnRegister.Content = "Actualizar";
            }

            retrieveCustomerData();
            lbAmountError.Content = "";
            lbPromotionError.Content = "";
            lbPurposeError.Content = "";
        }

        private void retrieveCreditRequestData()
        {
            using (var context = new sgscEntities())
            {
                var creditRequest = context.CreditRequests.Find(idCreditRequest);
                if (creditRequest != null)
                {
                    this.CreditRequest = creditRequest;
                    tbPurpose.Text = creditRequest.Purpose;
                    tbAmount.Text = creditRequest.AmountRequested.ToString();
                    fileNumber = creditRequest.FileNumber;
                }
            }
        }

        private void retrieveCredidPromotionSelectedIfAvailable()
        {
            using (var context = new sgscEntities())
            {
                var creditPromotion = context.CreditPromotions
                    .Where(cp => cp.InterestRate == CreditRequest.InterestRate && cp.TimePeriod == CreditRequest.TimePeriod)
                    .FirstOrDefault();
                if (creditPromotion != null)
                {
                    foreach (var item in cbCreditPromotions.Items)
                    {
                        var cp = (CreditPromotion)item;
                        if (cp.InterestRate == creditPromotion.InterestRate && cp.TimePeriod == creditPromotion.TimePeriod)
                        {
                            cbCreditPromotions.SelectedItem = item;
                        }
                    }
                }

                if (cbCreditPromotions.SelectedIndex == -1)
                {
                    MessageBox.Show("La promoción de esta soliitud ha cambiado, ya no esta existe o ha caducado, deberá seleccionar una vigente");
                }
            }
        }

        private void retrieveCustomerData()
        {
            using (var context = new sgscEntities())
            {
                var customer = context.Customers.Find(idCustomer);
                var customerContactInfo = context.CustomerContactInfoes.Where(c => c.CustomerId == idCustomer).FirstOrDefault();
                if (customer != null && customerContactInfo != null)
                {
                    lbName.Content = customer.Name + " " + customer.FirstSurname + " " + customer.SecondSurname ;
                    lbCurp.Content = customer.Curp;
                    var today = DateTime.Today;
                    var age = today.Year - customer.BirthDate.Year;
                    if (customer.BirthDate > today.AddYears(-age)) age--;
                    lbAge.Content = age.ToString()+" años";
                }else
                {
                    MessageBox.Show("Error al recuperar la información del cliente");
                }
            }
        }

        private void retrieveCreditPromotions()
        {
            using (var context = new sgscEntities())
            {
                var currentDate = DateTime.Now;
                var creditPromotions = context.CreditPromotions
                    .Where(cp => cp.StartDate <= currentDate && cp.EndDate >= currentDate)
                    .ToList();
                if (creditPromotions != null)
                {
                    foreach (var creditPromotion in creditPromotions)
                    {
                        cbCreditPromotions.Items.Add(creditPromotion);
                    }
                }
            }
            cbCreditPromotions.DisplayMemberPath = "Name";
        }

        private void cbCreditPromotions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCreditPromotions.SelectedIndex != -1)
            {
                var selectedPromotion = (CreditPromotion)cbCreditPromotions.SelectedItem;
                if(selectedPromotion.Interval == 1)
                {
                    lbTimePeriod.Content = selectedPromotion.TimePeriod+" Quincenas";
                }
                else if(selectedPromotion.Interval == 2)
                {
                    lbTimePeriod.Content = selectedPromotion.TimePeriod+" Meses";
                }
                lbInterestRate.Content = selectedPromotion.InterestRate.ToString() + "%";

                //aqui que calcule el monto
                calculateTotalAmount();
            }
        }

        private void tbAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void tbAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tbAmount.Text.Length < 1)
            {
                lbTotalAmount.Content = "0.0";
            }
            else
            {
            calculateTotalAmount();
            }
            
        }

        private void calculateTotalAmount()
        {
            if (cbCreditPromotions.SelectedIndex != -1)
            {
                var selectedPromotion = (CreditPromotion)cbCreditPromotions.SelectedItem;
                var amountIntroduced = 0.0;
                var timePeriodInMonths = 0.0;

                if (double.TryParse(tbAmount.Text, out amountIntroduced))
                {
                    if (selectedPromotion.Interval == 1)
                    {
                        timePeriodInMonths = (double)(selectedPromotion.TimePeriod / 2);
                    }
                    if (selectedPromotion.Interval == 2)
                    {
                        timePeriodInMonths = (double)selectedPromotion.TimePeriod;
                    }

                    //aqui obtenemos el interes mensual
                    double monthlyInterest = (double)(selectedPromotion.InterestRate / 100 / 12);
                    //aqui multiplicamos el interes mensual por las semanas en meses
                    var totalInterest = monthlyInterest * timePeriodInMonths;
                    //aqui calculamos el monto total
                    var totalAmount = amountIntroduced + (amountIntroduced * totalInterest);
                    this.totalAmount = totalAmount;
                    //format to only 2 decimals
                    lbTotalAmount.Content = totalAmount.ToString("0.00");
                }
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            lbPurposeError.Content = "";
            lbAmountError.Content = "";
            lbPromotionError.Content = "";

            bool isValid = true;
            if (cbCreditPromotions.SelectedIndex == -1)
            {
                lbPromotionError.Content= "Seleccione una promoción";
                isValid = false;
            }
            if (tbAmount.Text.Length < 1)
            {
                lbAmountError.Content = "Introduzca un monto";
                isValid = false;
            }
            if (tbPurpose.Text.Length < 1)
            {
                lbPurposeError.Content = "Introduzca un propósito";
                isValid = false;
            }
            if (isValid)
            {
                registerCreditRequest();
            }
            
        }

        private void registerCreditRequest()
        {
            var selectedPromotion = (CreditPromotion)cbCreditPromotions.SelectedItem;
            using (var context = new sgscEntities())
            {
                var creditRequest = new CreditRequest();
                var filenumber = "CR" + DateTime.Now.ToString("yyyyMMddHHmmss");
                creditRequest.FileNumber = filenumber;
                creditRequest.AmountRequested = double.Parse(tbAmount.Text);
                creditRequest.Amount = this.totalAmount;
                creditRequest.Status = 0;
                creditRequest.TimePeriod = selectedPromotion.TimePeriod;
                creditRequest.Purpose = tbPurpose.Text;
                creditRequest.InterestRate = selectedPromotion.InterestRate;
                creditRequest.CreationDate = DateTime.Now;
                creditRequest.EmployeeId = Utils.UserSession.Instance.Id;
                creditRequest.Employee = context.Employees.Find(Utils.UserSession.Instance.Id);
                creditRequest.CustomerId = idCustomer;
                creditRequest.PaymentsInterval = selectedPromotion.Interval;
                creditRequest.Description = "";

                
                if(idCreditRequest != -1)
                {
                    creditRequest.CreditRequestId = idCreditRequest;
                    creditRequest.FileNumber = CreditRequest.FileNumber;
                    //context.CreditRequests.Attach(creditRequest); ver que hace esto
                    creditRequest.CreationDate = CreditRequest.CreationDate;
                    filenumber = CreditRequest.FileNumber;
                }

                context.CreditRequests.AddOrUpdate(creditRequest);
                try
                {
                    context.SaveChanges();
                    var cr = context.CreditRequests.Where(c => c.FileNumber == filenumber).FirstOrDefault();

                    if (idCreditRequest == -1)
                    {
                        MessageBox.Show("Solicitud de crédito registrada exitosamente");
                        if (selectedPromotion.Interval == 1)
                        {
                            registerFortnightPayments(filenumber);
                        }
                        if (selectedPromotion.Interval == 2)
                        {
                            registerMonthlyPayments(filenumber);
                        }
                        App.Current.MainFrame.Content = new CustomerBankAccountsPage(idCustomer, cr.CreditRequestId, false);
                    }
                    else
                    {
                        MessageBox.Show("Solicitud de crédito actualizada exitosamente");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar la solicitud de crédito: " + ex.Message);
                    throw;
            }
        }
    }   

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainFrame.Content = new HomePageCreditAdvisor();
        }

        private void btnModifyCustomer_Click(object sender, RoutedEventArgs e)
        {
            var customerInfoPage = new CustomerInfoPage(idCreditRequest, idCustomer); //Agregar que se mande el id de la solicitud
            if (NavigationService != null)
            {
                NavigationService.Navigate(customerInfoPage);

            }
        }

        private void btnModifyCustomerAccounts_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainFrame.Content = new CustomerBankAccountsPage(idCustomer, idCreditRequest, true);
        }

        private void registerMonthlyPayments(string fileNumberAddPayments)
        {
            using (var context = new sgscEntities())
            {
                var creditRequest = context.CreditRequests.Where(cr => cr.FileNumber == fileNumberAddPayments).FirstOrDefault();
                MessageBox.Show("Proposito: " + creditRequest.Purpose);
                if (creditRequest != null)
                {
                    var payments = new List<Payment>();
                    var totalAmount = creditRequest.Amount;
                    var amountPerPayment = totalAmount / creditRequest.TimePeriod;
                    var currentDate = DateTime.Now;
                    var nextPaymentDate = currentDate.AddDays(15);
                    for (int i = 0; i < creditRequest.TimePeriod; i++)
                    {
                        var payment = new Payment();
                        payment.Amount = (decimal?)amountPerPayment;
                        payment.PaymentDate = nextPaymentDate;
                        payment.CreditRequestId = creditRequest.CreditRequestId;
                        payment.FileNumber = creditRequest.FileNumber;
                        //add the navigation column creditRequest_creditRequestId to the payment
                        payment.CreditRequests = creditRequest;

                        payments.Add(payment);
                        nextPaymentDate = nextPaymentDate.AddMonths(1);
                    }
                    context.Payments.AddRange(payments);
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error al registrar los pagos");
                    }
                }
            }

        }

        private void registerFortnightPayments(string fileNumberAddPayments)
        {
            using (var context = new sgscEntities())
            {
                var creditRequest = context.CreditRequests.Where(cr => cr.FileNumber == fileNumberAddPayments).FirstOrDefault();
                MessageBox.Show("Proposito: " + creditRequest.Purpose);
                if (creditRequest != null)
                {
                    var payments = new List<Payment>();
                    var totalAmount = creditRequest.Amount;
                    var amountPerPayment = totalAmount / creditRequest.TimePeriod;
                    var currentDate = DateTime.Now;
                    currentDate = currentDate.AddDays(15);
                    var nextPaymentDate = currentDate;
                    for (int i = 0; i < creditRequest.TimePeriod; i++)
                    {
                        var payment = new Payment();
                        payment.Amount = (decimal?)amountPerPayment;
                        payment.PaymentDate = nextPaymentDate;
                        payment.CreditRequestId = creditRequest.CreditRequestId;
                        payment.FileNumber = creditRequest.FileNumber;
                        //add the navigation column creditRequest_creditRequestId to the payment
                        payment.CreditRequests = creditRequest;

                        payments.Add(payment);
                        nextPaymentDate = nextPaymentDate.AddDays(15);
                    }
                    context.Payments.AddRange(payments);
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error al registrar los pagos");
                    }
                }
            }
        }

    }
}
