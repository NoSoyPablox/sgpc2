using System;
using System.Collections.Generic;
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
        public RegisterCreditRequest(int idCustomer)
        {
            InitializeComponent();
            this.idCustomer = idCustomer;
            retrieveCustomerData();
            retrieveCreditPromotions();
            lbAmountError.Content = "";
            lbPromotionError.Content = "";
            lbPurposeError.Content = "";
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
                    lbEmail.Content = customerContactInfo.Email;
                }
            }
        }

        private void retrieveCreditPromotions()
        {
            using (var context = new sgscEntities())
            {
                var creditPromotions = context.CreditPromotions.ToList();
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

            //check if all fields are filled
            var amountIntroduced = 0.0;
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
            
        }

    }
}
