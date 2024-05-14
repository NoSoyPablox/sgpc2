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
    /// Lógica de interacción para CreditPromotionDetails.xaml
    /// </summary>
    public partial class CreditPromotionDetails : Page
    {
        public CreditPromotionDetails(int idPromotion)
        {
            InitializeComponent();
            tbName.Focus();

            if (idPromotion != -1)
            {
                retrievePromotionDetails(idPromotion);
            }
        }

        public void retrievePromotionDetails(int idPromotion)
        {
            using (sgscEntities db = new sgscEntities())
            {
                var promotion = db.CreditPromotions.Where(p => p.CreditPromotionId == idPromotion).FirstOrDefault();
                if (promotion != null)
                {
                    tbName.Text = promotion.Name;
                    tbTimePeriod.Text = promotion.TimePeriod.ToString();
                    tbInterestRate.Text = promotion.InterestRate.ToString();

                    tbName.IsReadOnly = true;
                    tbTimePeriod.IsReadOnly = true;
                    tbInterestRate.IsReadOnly = true;
                }
            }
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            tbName.IsReadOnly = false;
            tbTimePeriod.IsReadOnly = false;
            tbInterestRate.IsReadOnly = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
