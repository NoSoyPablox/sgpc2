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
        }

        private void retrieveCustomerData()
        {
            using (var context = new sgscEntities())
            {
                var customer = context.Customers.Find(idCustomer);
                var customerContactInfo = context.CustomerContactInfoes.Where(c => c.CustomerId == idCustomer).FirstOrDefault();
                if (customer != null && customerContactInfo != null)
                {
                    lbName.Content = customer.Name;
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
                lbTimePeriod.Content = selectedPromotion.TimePeriod.ToString();
                lbInterestRate.Content = selectedPromotion.InterestRate.ToString();
            }
        }

        private void tbAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void tbAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Calcula el monto total
            if (cbCreditPromotions.SelectedIndex != -1)
            {
                var selectedPromotion = (CreditPromotion)cbCreditPromotions.SelectedItem;
                var amountIntroduced = 0.0;
                // Convierte el texto ingresado a un valor numérico
                if (double.TryParse(tbAmount.Text, out amountIntroduced))
                {
                    //aqui obtenemos el interes mensual
                    double monthlyInterest = (double)(selectedPromotion.InterestRate / 100 / 12);
                    //aqui obtenemos las semanas en meses
                    double weeksInMonths = (double)(selectedPromotion.TimePeriod / 4.33);
                    //aqui multiplicamos el interes mensual por las semanas en meses
                    var totalInterest = monthlyInterest * weeksInMonths;
                    //aqui calculamos el monto total
                    var totalAmount = amountIntroduced + (amountIntroduced * totalInterest);
                    //format to only 2 decimals
                    lbTotalAmount.Content = totalAmount.ToString("0.00");
                }
            }
        }
    }
}
