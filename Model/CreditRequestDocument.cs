using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGSC
{
    public partial class CreditRequestDocument
    {
        public enum  RequestPaymentInterval
        {
            Monthly = 2,
            Fortnight = 1,
        }

        public enum CivilStatuses : Int32
        {
            Single = 0,
            Married,
            Divorced,
            Widowed,
            Concubinage
        }

        public static string RequestPaymentIntervalToString(RequestPaymentInterval requestPaymentInterval)
        {
            switch (requestPaymentInterval)
            {
                case RequestPaymentInterval.Monthly:
                    return "Mensual";
                case RequestPaymentInterval.Fortnight:
                    return "Quincenal";
                default:
                    return "Intervalo de pago desconocido";
            }
        }

        public static string RequestCivilStatusToString(CivilStatuses status)
        {
            switch (status)
            {
                case CivilStatuses.Single:
                    return "Soltero(a)";
                case CivilStatuses.Married:
                    return "Casado(a)";
                case CivilStatuses.Divorced:
                    return "Divorciado(a)";
                case CivilStatuses.Widowed:
                    return "Viudo(a)";
                case CivilStatuses.Concubinage:
                    return "Union libre";
                default:
                    return "Desconocido";
            }
        }

    }
}
