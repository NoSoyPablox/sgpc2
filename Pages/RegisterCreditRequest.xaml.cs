using System;
using System.Collections.Generic;
using System.Globalization;
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
            if (System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"[^0-9]+"))
            {
                e.Handled = true;
            }
        }

        private void tbAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbAmount.TextChanged -= tbAmount_TextChanged;

            string text = tbAmount.Text.Replace(",", "");
            if (double.TryParse(text, out double amount))
            {
                tbAmount.Text = amount.ToString("N0", CultureInfo.CurrentCulture);
                tbAmount.CaretIndex = tbAmount.Text.Length;
            }

            tbAmount.TextChanged += tbAmount_TextChanged;
        }
    }
}
