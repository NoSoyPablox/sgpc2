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
using static SGSC.Pages.CollectionEfficienciesPage;

namespace SGSC.Pages
{
    /// <summary>
    /// Lógica de interacción para CustomerBankAccounts.xaml
    /// </summary>
    public partial class CustomerBankAccounts : Page
    {
        private int customerId;
        private int? tansferAccountId = null;
        private int? transferAccountBankId = null;
        private int? directDebitAccountId = null;
        private int? directDebitAccountBankId = null;

        //credit requestID in case of coming from credit request edit
        private int creditRequestId = -1;

        public CustomerBankAccounts(int customerId, int creditRequestId)
        {
            InitializeComponent();
            this.customerId = customerId;
            clearErrors();
            getBankAccounts();
            this.creditRequestId = creditRequestId;
            StepsSidebarFrame.Content = new CustomerRegisterStepsSidebar("BankAccounts");
            UserSessionFrame.Content = new UserSessionFrame();
        }

        private void clearErrors()
        {
            lbTansAccCardNumberError.Content = "";
            lbTansAccInterbankCodeError.Content = "";
            lbDomAccBankInterbankCodeError.Content = "";
            lbDomAccBankCardNumberError.Content = "";
        }

        private void getBankAccounts()
        {
            try
            {
                using (sgscEntities db = new sgscEntities())
                {
                    BankAccount transferAccount = db.BankAccounts.Where(ba => ba.CustomerId == customerId && ba.AccountType == (int)BankAccount.AccountTypes.TransferAccount).FirstOrDefault();
                    if (transferAccount != null)
                    {
                        tbTansAccCardNumber.Text = transferAccount.CardNumber;
                        tbTansAccBank.Text = transferAccount.Bank.Name;
                        tbTansAccInterbankCode.Text = transferAccount.InterbankCode;
                        tansferAccountId = transferAccount.BankAccountId;
                        transferAccountBankId = transferAccount.Bank.BankId;
                        //select the card type in the combobox
                        cbTransferCardType.SelectedIndex = transferAccount.CardType.Value;
                    }

                    BankAccount directDebitAccount = db.BankAccounts.Where(ba => ba.CustomerId == customerId && ba.AccountType == (int)BankAccount.AccountTypes.DirectDebitAccount).FirstOrDefault();
                    if (directDebitAccount != null)
                    {
                        tbDomAccBankCardNumber.Text = directDebitAccount.CardNumber;
                        tbDomAccBank.Text = directDebitAccount.Bank.Name;
                        tbDomAccBankInterbankCode.Text = directDebitAccount.InterbankCode;
                        this.directDebitAccountId = directDebitAccount.BankAccountId;
                        directDebitAccountBankId = directDebitAccount.Bank.BankId;
                        //select the card type in the combobox
                        cbDirectDebitCardType.SelectedIndex = directDebitAccount.CardType.Value;
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

            if (transferAccountBankId == null)
            {
                valid = false;
                lbTansAccInterbankCodeError.Content = "Por favor introduzca una CLABE válida";
            }

            if (directDebitAccountBankId == null)
            {
                valid = false;
                lbDomAccBankInterbankCodeError.Content = "Por favor introduzca una CLABE válida";
            }
            
            if(cbTransferCardType.SelectedIndex == -1)
            {
                valid = false;
                lbTansAccInterbankCodeError.Content = "Por favor seleccione un tipo de cuenta";
            }

            if(cbDirectDebitCardType.SelectedIndex == -1)
            {
                valid = false;
                lbDomAccBankInterbankCodeError.Content = "Por favor seleccione un tipo de cuenta";
            }

            if (!valid)
            {
                return;
            }

            try
            {
                using (sgscEntities context = new sgscEntities())
                {
                    var transferAccount = new BankAccount
                    {
                        CardNumber = tbTansAccCardNumber.Text,
                        BankBankId = transferAccountBankId,
                        InterbankCode = tbTansAccInterbankCode.Text,
                        AccountType = (int)BankAccount.AccountTypes.TransferAccount,
                        //obtain the selected card type
                        CardType = cbTransferCardType.SelectedIndex,
                        CustomerId = customerId
                    };

                    if (tansferAccountId != null)
                    {
                        transferAccount.BankAccountId = tansferAccountId.Value;
                    }

                    var directDebitAccount = new BankAccount
                    {
                        CardNumber = tbDomAccBankCardNumber.Text,
                        BankBankId = directDebitAccountBankId,
                        InterbankCode = tbDomAccBankInterbankCode.Text,
                        AccountType = (int)BankAccount.AccountTypes.DirectDebitAccount,
                        CardType = cbDirectDebitCardType.SelectedIndex,
                        CustomerId = customerId
                    };

                    if (directDebitAccountId != null)
                    {
                        directDebitAccount.BankAccountId = directDebitAccountId.Value;
                    }

                    context.BankAccounts.AddOrUpdate(transferAccount);
                    context.BankAccounts.AddOrUpdate(directDebitAccount);
                    context.SaveChanges();

                    MessageBox.Show("Cuentas bancarias guardadas exitosamente.");
                    //aqui implementar que si es llamado de nuevo cliente vuelva al homepage o consulta de clientes 
                    // y si es llamado desde corregir cliente en credit request que vuelva a la pagina de credit request usando edit bool

                    switch (creditRequestId)
                    {
                        case -2: // Solo se quiere registrar el cliente
                            App.Current.MainFrame.Content = new SearchCustomerPage();
                            break;

                        case -1: // Registrar una solicitud de credito nueva
                            App.Current.MainFrame.Content = new RegisterCreditRequest(customerId, -1);
                            break;

                        default: // Registrar una solicitud de crédito con un valor válido de creditRequestId

                            App.Current.MainFrame.Content = new RegisterCreditRequest(customerId, creditRequestId);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar las cuentas bancarias del cliente: " + ex.Message);
            }

        }


        private void cbUseTheSameAccount_Checked(object sender, RoutedEventArgs e)
        {
            // Disable debit account fields
            tbDomAccBankCardNumber.IsEnabled = false;
            tbDomAccBankInterbankCode.IsEnabled = false;

            // Copy transfer account fields to debit account fields
            tbDomAccBankCardNumber.Text = tbTansAccCardNumber.Text;
            tbDomAccBank.Text = tbTansAccBank.Text;
            tbDomAccBankInterbankCode.Text = tbTansAccInterbankCode.Text;
            directDebitAccountBankId = transferAccountBankId;
        }

        private void cbUseTheSameAccount_Unchecked(object sender, RoutedEventArgs e)
        {
            // Enable debit account fields
            tbDomAccBankCardNumber.IsEnabled = true;
            tbDomAccBankInterbankCode.IsEnabled = true;
        }

        private void CancelRegister(object sender, RoutedEventArgs e)
        {
            var result = System.Windows.Forms.MessageBox.Show("Está seguro que desea cancelar el registro?\nSi decide cancelarlo puede retomarlo más tarde.", "Cancelar registro", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                App.Current.MainFrame.Content = new HomePageCreditAdvisor();
            }
        }


        private void tbTansAccInterbankCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            var code = tbTansAccInterbankCode.Text;
            if (code.Length >= 3)
            {
                var bank = Bank.BankFromInterbankCodePrefix(code.Substring(0, 3));
                if (bank != null)
                {
                    tbTansAccBank.Text = bank.Name;
                    transferAccountBankId = bank.BankId;
                }
                else
                {
                    tbTansAccBank.Text = "Banco Desconocido";
                    transferAccountBankId = null;
                }

                if (transferAccountBankId == null)
                {
                    lbTansAccInterbankCodeError.Content = "Por favor introduzca una CLABE válida";
                }
                else
                {
                    lbTansAccInterbankCodeError.Content = "";
                }
            }
            else
            {
                tbTansAccBank.Text = "";
                transferAccountBankId = null;
                lbTansAccInterbankCodeError.Content = "";
            }
        }
        private void tbDomAccBank_TextChanged(object sender, TextChangedEventArgs e)
        {
            var code = tbDomAccBankInterbankCode.Text;
            if (code.Length >= 3)
            {
                var bank = Bank.BankFromInterbankCodePrefix(code.Substring(0, 3));
                if (bank != null)
                {
                    tbDomAccBank.Text = bank.Name;
                    directDebitAccountBankId = bank.BankId;
                }
                else
                {
                    tbDomAccBank.Text = "Banco Desconocido";
                    directDebitAccountBankId = null;
                }

                if (directDebitAccountBankId == null)
                {
                    lbDomAccBankInterbankCodeError.Content = "Por favor introduzca una CLABE válida";
                }
                else
                {
                    lbDomAccBankInterbankCodeError.Content = "";
                }
            }
            else
            {
                tbDomAccBank.Text = "";
                directDebitAccountBankId = null;
                lbDomAccBankInterbankCodeError.Content = "";
            }
        }

    }
}
