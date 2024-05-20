using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SGSC.Customer;

namespace SGSC
{
    public partial class Customer
    {
        public enum CivilStatuses : Int32
        {
            Single = 0,
            Married,
            Divorced,
            Widowed
        }

        public string FullName 
        {
            get
            {
                return $"{Name} {FirstSurname} {SecondSurname}";
            }
        }

        public static string GetCivilStatusString(CivilStatuses status)
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
                default:
                    return "Desconocido";
            }
        }
    }
}
