using SGSC.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ActiveCreditsPage.xaml
    /// </summary>
    public partial class ActiveCreditsPage : Page
    {
        public class ActiveCredit
        {
            public string ClientPageNumber { get; set; }
            public string CreditAmount { get; set; }
            public string CreditDate { get; set; }
            public string CreditPeriod { get; set; }
        }

        private ObservableCollection<ActiveCredit> ActiveCreditsCollection;
        private List<ActiveCredit> ActiveCredits { get; set; }


        public ActiveCreditsPage()
        {
            InitializeComponent();
            ActiveCredits = new List<ActiveCredit>();
            ActiveCreditsCollection = new ObservableCollection<ActiveCredit>();
            GetActiveCredits();
        }

        private void GetActiveCredits()
        {
            try
            {
                using (var context = new sgscEntities())
                {
                    var activeCredits = context.CreditRequests.Where(c => c.Status.Value == (int)CreditRequest.CreditStatus.Approved).ToList();
                    var activeCreditsArray = activeCredits.ToList();
                    foreach (var item in activeCreditsArray)
                    {
                        ActiveCredits.Add(new ActiveCredit
                        {
                            ClientPageNumber = item.FileNumber,
                            CreditAmount = item.Amount.Value.ToString(),
                            CreditDate = item.CreationDate.Value.ToString(),
                            CreditPeriod = item.TimePeriod.Value.ToString()
                        });
                    }
                    foreach (var p in ActiveCredits)
                    {
                        ActiveCreditsCollection.Add(p);
                    }
                }
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar obtener los datos de los créditos activos: " + ex.Message);
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserSession.LogOut();
        }
    }
}
