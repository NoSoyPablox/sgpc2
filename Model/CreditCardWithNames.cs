using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGSC.Model
{
    internal class CreditCardWithNames
    {
        public int BankAccountId { get; set; }
        public string InterbankCode { get; set; }
        public string CardNumber { get; set; }
        public int AccountTypeId { get; set; }
        public int CardTypeId { get; set; }
        public string bankName { get; set; }
        public string AssociationName { get; set; }
        public int BankBankId { get; set; }


        //constructor

        //constructor empty
        public CreditCardWithNames()
        {
            BankAccountId = 0;
            InterbankCode = "";
            CardNumber = "";
            AccountTypeId = 0;
            CardTypeId = 0;
            bankName = "";
            AssociationName = "";
            BankBankId = 0;
        }

        public CreditCardWithNames(int bankAccountId, string interbankCode, string cardNumber, int accountTypeId, int cardTypeId, string bankName, string associationName, int bankBankId)
        {
            BankAccountId = bankAccountId;
            InterbankCode = interbankCode;
            CardNumber = cardNumber;
            AccountTypeId = accountTypeId;
            CardTypeId = cardTypeId;
            this.bankName = bankName;
            AssociationName = associationName;
            BankBankId = bankBankId;
        }

        //to string
        public override string ToString()
        {
            if (BankAccountId == 0)
            {
                return "Nueva tarjeta";
            }
            //get the last 4 digits of the card number
            string last4Digits = CardNumber.Substring(CardNumber.Length - 4);
            return AssociationName + ": " + bankName + ": *****" + last4Digits;
        }



    }
}
