﻿using SGSC.Frames;
using SGSC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
using System.Xml.Linq;

namespace SGSC.Pages
{
    /// <summary>
    /// Interaction logic for CustomerBankAccountsPage.xaml
    /// </summary>
    public partial class CustomerBankAccountsPage : Page
    {
        private int customerId;
        private int creditRequestId;
        private int? tansferAccountId = null;
        private int? transferAccountBankId = null;
        private int? directDebitAccountId = null;
		private int? directDebitAccountBankId = null;
		private bool edit = false;

        public CustomerBankAccountsPage(int customerId, int creditRequestID, bool edit)
        {
            InitializeComponent();
            this.customerId = customerId;
            this.creditRequestId = creditRequestID;
            this.edit = edit;
            creditAdvisorSidebar.Content = new CreditAdvisorSidebar("");
            UserSessionFrame.Content = new UserSessionFrame();
            clearErrors();
            getBankAccounts();
            if(edit)
            {
                retrieveCurrentAccounts();
            }
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
            if(edit != true)
            {
                CreditCardWithNames newCreditCard = new CreditCardWithNames();
                cbTransferAccount.Items.Add(newCreditCard);
                cbDirectDebitAccount.Items.Add(newCreditCard);
            }
            try
            {
                using (sgscEntities db = new sgscEntities())
                {
                    BankAccount transferAccount = db.BankAccounts.Where(ba => ba.CustomerId == customerId && ba.AccountType == (int)BankAccount.AccountTypes.TransferAccount).FirstOrDefault();
                    if(transferAccount != null)
                    {
                        Bank bank = db.Banks.Where(b => b.BankId == transferAccount.BankBankId).FirstOrDefault();
                        CreditCardWithNames newCreditCard = new CreditCardWithNames();
                        newCreditCard.CardNumber = transferAccount.CardNumber;
                        newCreditCard.bankName = bank.Name;
                        newCreditCard.InterbankCode = transferAccount.InterbankCode;
                        newCreditCard.AssociationName = "Cliente";
                        newCreditCard.BankAccountId = transferAccount.BankAccountId;
                        newCreditCard.BankBankId = (int)transferAccount.BankBankId;
                        cbTransferAccount.Items.Add(newCreditCard);
					}

                    BankAccount directDebitAccount = db.BankAccounts.Where(ba => ba.CustomerId == customerId && ba.AccountType == (int)BankAccount.AccountTypes.DirectDebitAccount).FirstOrDefault();
                    if (directDebitAccount != null)
                    {
                        Bank bank = db.Banks.Where(b => b.BankId == directDebitAccount.BankBankId).FirstOrDefault();
                        CreditCardWithNames newCreditCard = new CreditCardWithNames();
                        newCreditCard.CardNumber = directDebitAccount.CardNumber;
                        newCreditCard.bankName = bank.Name;
                        newCreditCard.InterbankCode = directDebitAccount.InterbankCode;
                        newCreditCard.AssociationName = "Cliente";
                        newCreditCard.BankAccountId = directDebitAccount.BankAccountId;
                        newCreditCard.BankBankId = (int)directDebitAccount.BankBankId;
                        cbDirectDebitAccount.Items.Add(newCreditCard);
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

            if (cbTransferAccount.SelectedIndex == -1)
            {
                valid = false;
                MessageBox.Show("Por favor seleccione una cuenta de transferencia");
            }

            if (cbDirectDebitAccount.SelectedIndex == -1) //Osea que nada este seleccionado
            {
                valid = false;
                MessageBox.Show("Por favor seleccione una cuenta de débito");
            }


			if (!valid)
            {
                return;
            }

            try
            {
                if (edit)
                {
                    registerExistingTransferAccount();
                    registerExistingDirectAccount();
                    MessageBox.Show("Cuentas bancarias guardadas exitosamente.");
                    App.Current.MainFrame.Content = new DocumentsManagerPage(creditRequestId);
                }
                else
                {
                    registerNewRequestTransfer();
                    registerNewRequestDirectAccount();
                    App.Current.MainFrame.Content = new DocumentsManagerPage(creditRequestId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar las cuentas bancarias del cliente: " + ex.Message);
            }
        }

        private void registerExistingTransferAccount()
        {
            int indexTransfer = cbTransferAccount.SelectedIndex;
            using (sgscEntities context = new sgscEntities())
            {
                var creditRequest = context.CreditRequests.Where(cr => cr.CreditRequestId == creditRequestId).FirstOrDefault();
                switch (indexTransfer)
                {
                    case 0: // usar el modificar cuenta del cliente y actualizar tarjeta transferencia de la solicitud
                        var customerTransferAccount = context.BankAccounts.Where(ba => ba.CustomerId == customerId && ba.AccountType == (int)BankAccount.AccountTypes.TransferAccount).FirstOrDefault();
                        if (customerTransferAccount != null)
                        {
                            customerTransferAccount.CardNumber = tbTansAccCardNumber.Text;
                            customerTransferAccount.InterbankCode = tbTansAccInterbankCode.Text;
                            var bank = Bank.BankFromInterbankCodePrefix(tbTansAccInterbankCode.Text.Substring(0, 3));
                            customerTransferAccount.BankBankId = bank.BankId;
                            customerTransferAccount.AccountType = (int)BankAccount.AccountTypes.TransferAccount;

                            //Obtener la cuenta de transferencia actual
                            var currentRequestTransferAccount = context.BankAccounts.Where(ba => ba.BankAccountId == creditRequest.TransferBankAccount.BankAccountId).FirstOrDefault();
                            //Copiar los datos de la cuenta del cliente para la asociada a la solicitud
                            currentRequestTransferAccount.CardNumber = customerTransferAccount.CardNumber;
                            currentRequestTransferAccount.InterbankCode = customerTransferAccount.InterbankCode;
                            currentRequestTransferAccount.BankBankId = customerTransferAccount.BankBankId;
                            currentRequestTransferAccount.AccountType = customerTransferAccount.AccountType;
                            currentRequestTransferAccount.CardType = customerTransferAccount.CardType;
                            currentRequestTransferAccount.CustomerId = customerTransferAccount.CustomerId;
                           


                            context.SaveChanges();
                            
                        }

                        break;
                    case 1: // modificar cuenta asociada con la solicitud
                        var requestTransferAccount = context.BankAccounts.Where(ba => ba.BankAccountId == creditRequest.TransferBankAccount.BankAccountId).FirstOrDefault();
                        if (requestTransferAccount != null)
                        {
                            requestTransferAccount.CardNumber = tbTansAccCardNumber.Text;
                            requestTransferAccount.InterbankCode = tbTansAccInterbankCode.Text;
                            var bank = Bank.BankFromInterbankCodePrefix(tbTansAccInterbankCode.Text.Substring(0, 3));
                            requestTransferAccount.BankBankId = bank.BankId;
                            requestTransferAccount.AccountType = (int)BankAccount.AccountTypes.TransferAccount;

                            context.SaveChanges();
                        }
                        break;
                }
            }
        }

        private void registerExistingDirectAccount()
        {
            int indexDirect = cbDirectDebitAccount.SelectedIndex;
            using(sgscEntities context = new sgscEntities())
            {
                var creditRequest = context.CreditRequests.Where(cr => cr.CreditRequestId == creditRequestId).FirstOrDefault();
                switch (indexDirect)
                {
                    case 0: // usar, modificar cuenta del cliente y actualizar tarjeta transferencia de la solicitud
                        var customerDirectAccount = context.BankAccounts.Where(ba => ba.CustomerId == customerId && ba.AccountType == (int)BankAccount.AccountTypes.DirectDebitAccount).FirstOrDefault();
                        if (customerDirectAccount != null)
                        {
                            customerDirectAccount.CardNumber = tbDomAccBankCardNumber.Text;
                            customerDirectAccount.InterbankCode = tbDomAccBankInterbankCode.Text;
                            var bank = Bank.BankFromInterbankCodePrefix(tbDomAccBankInterbankCode.Text.Substring(0, 3));
                            customerDirectAccount.BankBankId = bank.BankId;
                            customerDirectAccount.AccountType = (int)BankAccount.AccountTypes.DirectDebitAccount;

                            //Obtener la cuenta directa actual
                            var currentRequestDirectAccount = context.BankAccounts.Where(ba => ba.BankAccountId == creditRequest.DirectDebitBankAccount.BankAccountId).FirstOrDefault();
                            //Copiar los datos de la cuenta del cliente para la asociada a la solicitud
                            currentRequestDirectAccount.CardNumber = customerDirectAccount.CardNumber;
                            currentRequestDirectAccount.InterbankCode = customerDirectAccount.InterbankCode;
                            currentRequestDirectAccount.BankBankId = customerDirectAccount.BankBankId;
                            currentRequestDirectAccount.AccountType = customerDirectAccount.AccountType;
                            currentRequestDirectAccount.CardType = customerDirectAccount.CardType;
                            currentRequestDirectAccount.CustomerId = customerDirectAccount.CustomerId;


                            context.SaveChanges();
                        }

                        break;
                    case 1: // modificar cuenta asociada con la solicitud
                        var requestDirectAccount = context.BankAccounts.Where(ba => ba.BankAccountId == creditRequest.DirectDebitBankAccount.BankAccountId).FirstOrDefault();
                        if (requestDirectAccount != null)
                        {
                            requestDirectAccount.CardNumber = tbDomAccBankCardNumber.Text;
                            requestDirectAccount.InterbankCode = tbDomAccBankInterbankCode.Text;
                            var bank = Bank.BankFromInterbankCodePrefix(tbDomAccBankInterbankCode.Text.Substring(0, 3));
                            requestDirectAccount.BankBankId = bank.BankId;
                            requestDirectAccount.AccountType = (int)BankAccount.AccountTypes.DirectDebitAccount;
                            //update database
                            
                            context.SaveChanges();
                        }
                        break;
                }
            }
        }

        private void registerNewRequestTransfer()
        {
            int indexTransfer = cbTransferAccount.SelectedIndex;
            using(sgscEntities context = new sgscEntities())
            {
                var creditRequest = context.CreditRequests.Where(cr => cr.CreditRequestId == creditRequestId).FirstOrDefault();
                switch (indexTransfer)
                {
                    case 0: //Crear nueva cuenta y asociarla a la solicitud
                        BankAccount transferAccount = new BankAccount();
                        transferAccount.CardNumber = tbTansAccCardNumber.Text;
                        transferAccount.InterbankCode = tbTansAccInterbankCode.Text;
                        var bank = Bank.BankFromInterbankCodePrefix(tbTansAccInterbankCode.Text.Substring(0, 3));
                        transferAccount.BankBankId = bank.BankId;
                        transferAccount.AccountType = (int)BankAccount.AccountTypes.TransferAccount;
                        transferAccount.CardType = (int)BankAccount.CardTypes.Debit;
                        transferAccount.CustomerId = customerId;
                        creditRequest.TransferBankAccount = transferAccount;
                        context.SaveChanges();
                        break;

                    case 1: //Usar cuenta del cliente y modificarla si es necesario
                        BankAccount customerTransferAccount = context.BankAccounts.Where(ba => ba.CustomerId == customerId && ba.AccountType == (int)BankAccount.AccountTypes.TransferAccount).FirstOrDefault();
                        if (customerTransferAccount != null)
                        {
                            //Modificarlo con los datos introducidos
                            customerTransferAccount.CardNumber = tbTansAccCardNumber.Text;
                            customerTransferAccount.InterbankCode = tbTansAccInterbankCode.Text;
                            bank = Bank.BankFromInterbankCodePrefix(tbTansAccInterbankCode.Text.Substring(0, 3));
                            customerTransferAccount.BankBankId = bank.BankId;
                            customerTransferAccount.AccountType = (int)BankAccount.AccountTypes.TransferAccount;

                            BankAccount copyForRequest = new BankAccount();
                            copyForRequest.CardNumber = customerTransferAccount.CardNumber;
                            copyForRequest.InterbankCode = customerTransferAccount.InterbankCode;
                            copyForRequest.BankBankId = customerTransferAccount.BankBankId;
                            copyForRequest.AccountType = customerTransferAccount.AccountType;
                            copyForRequest.CardType = customerTransferAccount.CardType;
                            copyForRequest.CustomerId = customerTransferAccount.CustomerId;
                            creditRequest.TransferBankAccount = copyForRequest;
                            //update database
                            context.SaveChanges();
                        }
                        break;  

                }
            }
        }

        private void registerNewRequestDirectAccount()
        {
            int indexDirect = cbDirectDebitAccount.SelectedIndex;
            using (sgscEntities context = new sgscEntities())
            {
                var creditRequest = context.CreditRequests.Where(cr => cr.CreditRequestId == creditRequestId).FirstOrDefault();
                switch (indexDirect)
                {
                    case 0: //Crear cuenta nueva y asociarla a la solicitud
                        BankAccount directDebitAccount = new BankAccount();
                        directDebitAccount.CardNumber = tbDomAccBankCardNumber.Text;
                        directDebitAccount.InterbankCode = tbDomAccBankInterbankCode.Text;
                        //obtain bank name from interbank code
                        var bank = Bank.BankFromInterbankCodePrefix(tbDomAccBankInterbankCode.Text.Substring(0, 3));
                        directDebitAccount.BankBankId = bank.BankId;
                        directDebitAccount.AccountType = (int)BankAccount.AccountTypes.DirectDebitAccount;
                        directDebitAccount.CardType = (int)BankAccount.CardTypes.Debit;
                        directDebitAccount.CustomerId = customerId;
                        creditRequest.DirectDebitBankAccount = directDebitAccount;
                        context.SaveChanges();
                        break;

                    case 1: //Usar cuenta del cliente y modificar si es necesario
                        BankAccount customerDirectAccount = context.BankAccounts.Where(ba => ba.CustomerId == customerId && ba.AccountType == (int)BankAccount.AccountTypes.DirectDebitAccount).FirstOrDefault();
                        if (customerDirectAccount != null)
                        {
                            //Modificarlo con los datos introducidos
                            customerDirectAccount.CardNumber = tbDomAccBankCardNumber.Text;
                            customerDirectAccount.InterbankCode = tbDomAccBankInterbankCode.Text;
                            bank = Bank.BankFromInterbankCodePrefix(tbDomAccBankInterbankCode.Text.Substring(0, 3));
                            customerDirectAccount.BankBankId = bank.BankId;
                            customerDirectAccount.AccountType = (int)BankAccount.AccountTypes.DirectDebitAccount;

                            BankAccount copyForRequest = new BankAccount();
                            copyForRequest.CardNumber = customerDirectAccount.CardNumber;
                            copyForRequest.InterbankCode = customerDirectAccount.InterbankCode;
                            copyForRequest.BankBankId = customerDirectAccount.BankBankId;
                            copyForRequest.AccountType = customerDirectAccount.AccountType;
                            copyForRequest.CardType = customerDirectAccount.CardType;
                            copyForRequest.CustomerId = customerDirectAccount.CustomerId;
                            creditRequest.DirectDebitBankAccount = copyForRequest;

                            context.SaveChanges();
                            
                        }
                        break;
                }
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
            tbDomAccBankInterbankCode.IsEnabled = false;

            // Copy transfer account fields to debit account fields
            tbDomAccBankCardNumber.Text = tbTansAccCardNumber.Text;
            tbDomAccBank.Text = tbTansAccBank.Text;
            tbDomAccBankInterbankCode.Text = tbTansAccInterbankCode.Text;
            directDebitAccountBankId = transferAccountBankId;

            // Disable debit account combobox
            cbDirectDebitAccount.IsEnabled = false;
        }

        private void cbUseTheSameAccount_Unchecked(object sender, RoutedEventArgs e)
        {
            // Enable debit account fields
            tbDomAccBankCardNumber.IsEnabled = true;
            tbDomAccBankInterbankCode.IsEnabled = true;
            cbDirectDebitAccount.IsEnabled = true;
        }

		private void tbTansAccInterbankCode_TextChanged(object sender, TextChangedEventArgs e)
		{
            var code = tbTansAccInterbankCode.Text;
            if(code.Length >= 3)
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

        private void cbTransferAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbTransferAccount.SelectedIndex != -1)
            {

                CreditCardWithNames cbTransferAccountSelected = (CreditCardWithNames)cbTransferAccount.SelectedItem;
                if (cbTransferAccountSelected.BankAccountId == 0)
                {
                    tbTansAccCardNumber.Text = "";
                    tbTansAccBank.Text = "";
                    tbTansAccInterbankCode.Text = "";
                    tansferAccountId = null;
                    transferAccountBankId = null;
                }
                else
                {
                    CreditCardWithNames transferAccountSelected = (CreditCardWithNames)cbTransferAccount.SelectedItem;
                    tbTansAccCardNumber.Text = transferAccountSelected.CardNumber;
                    tbTansAccBank.Text = transferAccountSelected.bankName;
                    tbTansAccInterbankCode.Text = transferAccountSelected.InterbankCode;
                    tansferAccountId = transferAccountSelected.BankAccountId;

                }

            }
        }

        private void cbDirectDebitAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbDirectDebitAccount.SelectedIndex != -1)
            {
                CreditCardWithNames cbDirectDebitAccountSelected = (CreditCardWithNames)cbDirectDebitAccount.SelectedItem;
                if (cbDirectDebitAccountSelected.BankAccountId == 0)
                {
                    tbDomAccBankCardNumber.Text = "";
                    tbDomAccBank.Text = "";
                    tbDomAccBankInterbankCode.Text = "";
                    directDebitAccountId = null;
                    directDebitAccountBankId = null;
                }
                else
                {
                    CreditCardWithNames directDebitAccountSelected = (CreditCardWithNames)cbDirectDebitAccount.SelectedItem;
                    tbDomAccBankCardNumber.Text = directDebitAccountSelected.CardNumber;
                    tbDomAccBank.Text = directDebitAccountSelected.bankName;
                    tbDomAccBankInterbankCode.Text = directDebitAccountSelected.InterbankCode;
                    directDebitAccountId = directDebitAccountSelected.BankAccountId;
                }

            }

        }

        private void retrieveCurrentAccounts()
        {
            try
            {
                using (sgscEntities db = new sgscEntities())
                {
                    var creditRequest = db.CreditRequests.Where(cr => cr.CreditRequestId == creditRequestId).FirstOrDefault();
                    if (creditRequest != null)
                    {
                        if (creditRequest.TransferBankAccount != null)
                        {
                            CreditCardWithNames transferAccountFromRequest = new CreditCardWithNames();
                            transferAccountFromRequest.BankAccountId = creditRequest.TransferBankAccount.BankAccountId;
                            transferAccountFromRequest.InterbankCode = creditRequest.TransferBankAccount.InterbankCode;
                            transferAccountFromRequest.CardNumber = creditRequest.TransferBankAccount.CardNumber;
                            transferAccountFromRequest.bankName = creditRequest.TransferBankAccount.Bank.Name;
                            transferAccountFromRequest.AssociationName = "Solicitud";
                            cbTransferAccount.Items.Add(transferAccountFromRequest);
                            //set combobox to this account
                            cbTransferAccount.SelectedIndex = 1;

                        }

                        if (creditRequest.DirectDebitBankAccount != null)
                        {
                            CreditCardWithNames directDebitAccountFromRequest = new CreditCardWithNames();
                            directDebitAccountFromRequest.BankAccountId = creditRequest.DirectDebitBankAccount.BankAccountId;
                            directDebitAccountFromRequest.InterbankCode = creditRequest.DirectDebitBankAccount.InterbankCode;
                            directDebitAccountFromRequest.CardNumber = creditRequest.DirectDebitBankAccount.CardNumber;
                            directDebitAccountFromRequest.bankName = creditRequest.DirectDebitBankAccount.Bank.Name;
                            directDebitAccountFromRequest.AssociationName = "Solicitud";
                            cbDirectDebitAccount.Items.Add(directDebitAccountFromRequest);
                            //set combobox to this account
                            cbDirectDebitAccount.SelectedIndex = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener las cuentas bancarias del cliente: " + ex.Message);
            }
        }

    }

}
