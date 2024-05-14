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
    /// Lógica de interacción para CreditPromotions.xaml
    /// </summary>
    public partial class CreditPromotions : Page
    {
        public CreditPromotions()
        {
            InitializeComponent();
            retrievePromotions();
        }

        public void retrievePromotions()
        {
            using (sgscEntities db = new sgscEntities())
            {
                var promotions = db.CreditPromotions.ToList();
                dgPromotions.ItemsSource = promotions;
            }
        }

        private void btnSeeDetails_Click(object sender, RoutedEventArgs e)
        {
            CreditPromotion selectedPromotion = (CreditPromotion)dgPromotions.SelectedItem;
            if (selectedPromotion != null)
            {
                CreditPromotionDetails promotionDetails = new CreditPromotionDetails(selectedPromotion.CreditPromotionId);
                this.NavigationService.Navigate(promotionDetails);
            }
            else
            {
                MessageBox.Show("Por favor seleccione una promoción", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                searchPromotionByName();
            }
        }

        private void tbSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            using (sgscEntities db = new sgscEntities())
            {
                searchPromotionByName();
            }
        }

        private void searchPromotionByName()
        {
            using (sgscEntities db = new sgscEntities())
            {
                string searchCriteria = tbSearch.Text.Replace(" ", "").ToUpper();
                var promotions = db.CreditPromotions.Where(p => p.Name.Contains(searchCriteria)).ToList();
                dgPromotions.ItemsSource = promotions;
            }
        }

        private void btnAddPromotion_Click(object sender, RoutedEventArgs e)
        {
            CreditPromotionDetails promotionDetails = new CreditPromotionDetails(-1);
            this.NavigationService.Navigate(promotionDetails);
        }
    }
}
