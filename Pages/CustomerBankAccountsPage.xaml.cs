using SGSC.Frames;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
    /// Interaction logic for CustomerBankAccountsPage.xaml
    /// </summary>
    public partial class CustomerBankAccountsPage : Page
    {
        private int customerId;
        private int? tansferAccountId = null;
        private int? directDebitAccountId = null;
        private bool edited = false;

        public CustomerBankAccountsPage(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;

            StepsSidebarFrame.Content = new CustomerRegisterStepsSidebar("BankAccounts");
            UserSessionFrame.Content = new UserSessionFrame();

            clearErrors();
            getBankAccounts();
        }

        private void clearErrors()
        {
            lbTansAccError.Content = "";
            lbTansAccCardNumberError.Content = "";
            lbTansAccInterbankCodeError.Content = "";
            lbDomAccBankInterbankCodeError.Content = "";
            lbDomAccBankError.Content = "";
            lbDomAccBankCardNumberError.Content = "";
        }

        private void getBankAccounts()
        {
            try
            {
                using (SGSCEntities db = new SGSCEntities())
                {
                    BankAccounts transferAccount = db.BankAccounts.Where(ba => ba.CustomerId == customerId && ba.AccountType == "transfer").FirstOrDefault();
                    if(transferAccount != null)
                    {
                        tbTansAccCardNumber.Text = transferAccount.CardNumber;
                        //tbTansAccBank.Text = transferAccount.Name;
                        tbTansAccInterbankCode.Text = transferAccount.InterbankCode;
                        tansferAccountId = transferAccount.BankAccountId;
                    }

                    BankAccounts directDebitAccount = db.BankAccounts.Where(ba => ba.CustomerId == customerId && ba.AccountType == "directDebit").FirstOrDefault();
                    if (directDebitAccount != null)
                    {
                        tbDomAccBankCardNumber.Text = directDebitAccount.CardNumber;
                        //tbDomAccBank.Text = directDebitAccount.BankName;
                        tbDomAccBankInterbankCode.Text = directDebitAccount.InterbankCode;
                        directDebitAccountId = directDebitAccount.BankAccountId;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener las cuentas bancarias del cliente: " + ex.Message);
            }
        }

        private void SaveBankAccounts(object sender, RoutedEventArgs e)
        {
            bool valid = true;

            clearErrors();

            if (string.IsNullOrEmpty(tbTansAccCardNumber.Text))
            {
                valid = false;
                lbTansAccCardNumberError.Content = "Por favor introduzca el número de tarjeta";
            }

            // Check if cardnumber is valid
            if (!Utils.TextValidator.ValidateCardNumber(tbTansAccCardNumber.Text))
            {
                valid = false;
                lbTansAccCardNumberError.Content = "Por favor introduzca un número de tarjeta válido";
            }

            if (string.IsNullOrEmpty(tbTansAccBank.Text))
            {
                valid = false;
                lbTansAccError.Content = "Por favor introduzca el nombre del banco";
            }

            if (string.IsNullOrEmpty(tbTansAccInterbankCode.Text))
            {
                valid = false;
                lbTansAccInterbankCodeError.Content = "Por favor introduzca el código interbancario";
            }
            
            // Check if interbank code is valid
            if (!Utils.TextValidator.ValidateTextNumeric(tbTansAccInterbankCode.Text, 20))
            {
                valid = false;
                lbTansAccInterbankCodeError.Content = "Por favor introduzca un código interbancario válido";
            }

            if (string.IsNullOrEmpty(tbDomAccBankCardNumber.Text))
            {
                valid = false;
                lbDomAccBankCardNumberError.Content = "Por favor introduzca el número de tarjeta";
            }

            // Check if cardnumber is valid
            if (!Utils.TextValidator.ValidateCardNumber(tbDomAccBankCardNumber.Text))
            {
                valid = false;
                lbDomAccBankCardNumberError.Content = "Por favor introduzca un número de tarjeta válido";
            }

            if (string.IsNullOrEmpty(tbDomAccBank.Text))
            {
                valid = false;
                lbDomAccBankError.Content = "Por favor introduzca el nombre del banco";
            }

            if (string.IsNullOrEmpty(tbDomAccBankInterbankCode.Text))
            {
                valid = false;
                lbDomAccBankInterbankCodeError.Content = "Por favor introduzca el código interbancario";
            }

            // Check if interbank code is valid
            if (!Utils.TextValidator.ValidateTextNumeric(tbDomAccBankInterbankCode.Text, 20))
            {
                valid = false;
                lbDomAccBankInterbankCodeError.Content = "Por favor introduzca un código interbancario válido";
            }

            if (!valid)
            {
                return;
            }

            try
            {
                using (SGSCEntities context = new SGSCEntities())
                {
                    var transferAccounts = new BankAccounts
                    {
                        CardNumber = tbTansAccCardNumber.Text,
                        //BankName = tbTansAccBank.Text,
                        InterbankCode = tbTansAccInterbankCode.Text,
                        AccountType = "Transferencia",
                        CustomerId = customerId
                    };

                    if (tansferAccountId != null)
                    {
                        transferAccounts.BankAccountId = tansferAccountId.Value;
                    }

                    var directDebitAccount = new BankAccounts
                    {
                        CardNumber = tbDomAccBankCardNumber.Text,
                        //BankName = tbDomAccBank.Text,
                        InterbankCode = tbDomAccBankInterbankCode.Text,
                        AccountType = "Domicialización",
                        CustomerId = customerId
                    };

                    if (directDebitAccountId != null)
                    {
                        directDebitAccount.BankAccountId = directDebitAccountId.Value;
                    }

                    context.BankAccounts.AddOrUpdate(transferAccounts);
                    context.BankAccounts.AddOrUpdate(directDebitAccount);
                    context.SaveChanges();

                    MessageBox.Show("Cuentas bancarias guardadas exitosamente.");
                    App.Current.MainFrame.Content = new PageWorkCenter(customerId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar las cuentas bancarias del cliente: " + ex.Message);
            }
        }

        private void CancelRegister(object sender, RoutedEventArgs e)
        {
            var result = System.Windows.Forms.MessageBox.Show("Está seguro que desea cancelar el registro?\nSi decide cancelarlo puede retomarlo más tarde.", "Cancelar registro", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                App.Current.MainFrame.Content = new HomePageCreditAdvisor();
            }
        }

        private void cbUseTheSameAccount_Checked(object sender, RoutedEventArgs e)
        {
            // Disable debit account fields
            tbDomAccBankCardNumber.IsEnabled = false;
            tbDomAccBank.IsEnabled = false;
            tbDomAccBankInterbankCode.IsEnabled = false;

            // Copy transfer account fields to debit account fields
            tbDomAccBankCardNumber.Text = tbTansAccCardNumber.Text;
            tbDomAccBank.Text = tbTansAccBank.Text;
            tbDomAccBankInterbankCode.Text = tbTansAccInterbankCode.Text;
        }

        private void cbUseTheSameAccount_Unchecked(object sender, RoutedEventArgs e)
        {
            // Enable debit account fields
            tbDomAccBankCardNumber.IsEnabled = true;
            tbDomAccBank.IsEnabled = true;
            tbDomAccBankInterbankCode.IsEnabled = true;
        }
    }
}
