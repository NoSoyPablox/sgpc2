using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGSC
{
    public partial class BankAccount
    {
        public enum AccountTypes : Int32
        {
            TransferAccount,
            DirectDebitAccount
        }

        public enum CardTypes : Int32
		{
            Credit,
			Debit
		}

        public static string AccountTypeToString(AccountTypes accountType)
        {
            switch (accountType)
            {
                case AccountTypes.TransferAccount:
                    return "Cuenta de transferencia";
                case AccountTypes.DirectDebitAccount:
                    return "Cuenta de domiciliación";
                default:
                    return "Tipo de cuenta desconocido";
            }
        }
    }
}
