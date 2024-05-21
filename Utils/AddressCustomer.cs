using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGSC.Utils
{
    public class AddressCustomer
    {
        public enum TypeAddress : short
        {
            Propietario = 0,
            Hipotecado,
            Alquiler,
            Familiar
        }

        public static string GetTypeAddress(int role)
        {
            switch (role)
            {
                case (short)TypeAddress.Propietario:
                    return "Propietario";
                case (short)TypeAddress.Hipotecado:
                    return "Hipotecado";
                case (short)TypeAddress.Alquiler:
                    return "Alquiler";
                case (short)TypeAddress.Familiar:
                    return "Familiar";
                default:
                    return "Rol no implementado";
            }
        }
    }
}
