using SGSC.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    public partial class Credit_Application_Details_personal_references_ : Page
    {
        private int? requestId;
        public Credit_Application_Details_personal_references_(int? requestId)
        {
            InitializeComponent();
            StepsSidebarFrame.Content = new CreditApplicationDataStepsSidebar();
            UserSessionFrame.Content = new UserSessionFrame();
            ObtainPersonalReferences();
        }

        public void ObtainPersonalReferences()
        {
            using (sgscEntities db = new sgscEntities())
            {
                var customerId = db.CreditRequests
                                    .Where(cr => cr.CreditRequestId == 3)
                                    .Select(cr => cr.CustomerId)
                                    .FirstOrDefault();

                if (customerId == 0) // Supongamos que 0 indica que no se encontró ningún cliente
                {
                    MessageBox.Show("La solicitud especificada no fue encontrada.");
                    return;
                }

                var contacts = db.Contacts
                                            .Where(pr => pr.CustomerId == customerId)
                                            .ToList();

                if (contacts.Any())
                {
                    lbNameReferenceOne.Content = contacts.ElementAtOrDefault(0)?.Name;
                    lbRelationshipReferenceOne.Content = contacts.ElementAtOrDefault(0)?.Relationship;
                    lbPhoneReferenceOne.Content = contacts.ElementAtOrDefault(0)?.PhoneNumber;

                    lbNameReferenceTwo.Content = contacts.ElementAtOrDefault(1)?.Name;
                    lbRelationshipReferenceTwo.Content = contacts.ElementAtOrDefault(1)?.Relationship;
                    lbPhoneReferenceTwo.Content = contacts.ElementAtOrDefault(1)?.PhoneNumber;
                }
                else
                {
                    MessageBox.Show("No se encontraron referencias personales para este cliente.");
                }
            }

        }

        private void BtnClickContinue(object sender, RoutedEventArgs e)
        {
            var pageBankAccounts = new Credit_Application_Details_bank_accounts_(3);

            if (NavigationService != null)
            {
                NavigationService.Navigate(pageBankAccounts);
            }
        }
    }
}
