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
using System.Xml.Linq;

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
                searchCriterias();
            }
        }

        private void tbSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            using (sgscEntities db = new sgscEntities())
            {
                searchCriterias();
            }
        }

        private void btnAddPromotion_Click(object sender, RoutedEventArgs e)
        {
            CreditPromotionDetails promotionDetails = new CreditPromotionDetails(-1);
            this.NavigationService.Navigate(promotionDetails);
        }

        private void searchCriterias()
        {
            DateTime currentDate = DateTime.Now;
            using (sgscEntities db = new sgscEntities())
            {
                if (string.IsNullOrEmpty(tbSearch.Text)){
                    switch (cbStatusSearch.SelectedIndex)
                    {
                        case 0: //Todas
                            var promotions = db.CreditPromotions.ToList();
                            dgPromotions.ItemsSource = promotions;
                            break;
                        case 1: //vigente
                            var activePromotions = db.CreditPromotions.Where(predicate: p => p.StartDate <= currentDate && p.EndDate >= currentDate).ToList();
                            dgPromotions.ItemsSource = activePromotions;
                            break;
                        case 2: //vencida
                            var inactivePromotions = db.CreditPromotions.Where(predicate: p => p.EndDate < currentDate).ToList();
                            dgPromotions.ItemsSource = inactivePromotions;
                            break;
                    }
                }
                else
                {
                    string searchCriteria = tbSearch.Text.Replace(" ", "").ToUpper();
                    switch (cbStatusSearch.SelectedIndex)
                    {
                        case 0: //Todas
                            var promotions = db.CreditPromotions.Where(p => p.Name.Contains(searchCriteria)).ToList();
                            dgPromotions.ItemsSource = promotions;
                            break;
                        case 1: //vigente
                            var activePromotions = db.CreditPromotions.Where(predicate: p => p.StartDate <= currentDate && p.EndDate >= currentDate && p.Name.Contains(searchCriteria)).ToList();
                            dgPromotions.ItemsSource = activePromotions;
                            break;
                        case 2: //vencida
                            var inactivePromotions = db.CreditPromotions.Where(predicate: p => p.EndDate < currentDate && p.Name.Contains(searchCriteria)).ToList();
                            dgPromotions.ItemsSource = inactivePromotions;
                            break;
                    }
                }
            }
        }

        private void cbStatusSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            searchCriterias();
        }
    }
}
