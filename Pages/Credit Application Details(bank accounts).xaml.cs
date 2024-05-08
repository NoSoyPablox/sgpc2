using SGSC.Frames;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public partial class Credit_Application_Details_bank_accounts_ : Page
    {
        private int? requestId;
        public Credit_Application_Details_bank_accounts_(int? requestId)
        {
            InitializeComponent();
            StepsSidebarFrame.Content = new CreditApplicationDataStepsSidebar();
            UserSessionFrame.Content = new UserSessionFrame();
            this.requestId = requestId;

            if (requestId > 0)
            {
                getBankAccounts(requestId.Value);
            }
        }

        private void getBankAccounts(int requestId)
        {
            using (sgscEntities db = new sgscEntities())
            {
                var transferenciaAccounts = db.BankAccounts
                        .Join(
                            db.CreditRequests,
                            bank => bank.BankAccountId,
                            request => request.TransferBankAccount_BankAccountId,
                            (bank, request) => new { BankAccount = bank, Request = request }
                        )
                        .Where(joinResult => joinResult.Request.CreditRequestId == requestId)
                        .Select(joinResult => joinResult.BankAccount)
                        .ToList();

                if (transferenciaAccounts == null)
                {
                    MessageBox.Show("No se encontró la solicitud especificada.");
                    return;
                }
                else
                {
                    var firstTransferAccount = transferenciaAccounts.First();
                    lbNameBankAccountTransfer.Content = firstTransferAccount.BankName;
                    lbClabeAccountTransfer.Content = firstTransferAccount.Clabe;
                    lbTargetNumberAccountTransfer.Content = firstTransferAccount.CardNumber;
                }


                var domicializationAccount = db.BankAccounts
                       .Join(
                           db.CreditRequests,
                           bank => bank.BankAccountId,
                           request => request.DomicializationBankAccount_BankAccountId,
                           (bank, request) => new { BankAccount = bank, Request = request }
                       )
                       .Where(joinResult => joinResult.Request.CreditRequestId == requestId)
                       .Select(joinResult => joinResult.BankAccount)
                       .FirstOrDefault();

                if (domicializationAccount == null)
                {
                    MessageBox.Show("No se encontró la solicitud especificada o no hay cuenta de domiciliación asociada.");
                }
                else
                {
                    lbNameBankAccountDomicialization.Content = domicializationAccount.BankName;
                    lbClabeAccountDomicialization.Content = domicializationAccount.Clabe;
                    lbTargetNumberAccountDomicialization.Content = domicializationAccount.CardNumber;
                }





            }
        }

        private void BtnClicContinue(object sender, RoutedEventArgs e)
        {
            var creditAprovePage = new Credit_Application_Details_approve_credit_application_(requestId);

            if (NavigationService != null)
            {
                NavigationService.Navigate(creditAprovePage);
            }
        }
    }
}
