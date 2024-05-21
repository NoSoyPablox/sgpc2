using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGSC.Utils
{
    public class CivilStatus
    {
        public enum State : short
        {
            Soltero  = 0,
            Casado,
            Divorciado,
            Viudo,
            UnionLibre
        }

        public static string GetTypeAddress(short role)
        {
            switch (role)
            {
                case (short)State.Soltero:
                    return "Soltero(a)";
                case (short)State.Casado:
                    return "Casado(a)";
                case (short)State.Divorciado:
                    return "Divorciado(a)";
                case (short)State.Viudo:
                    return "Viudo(a)";
                case (short)State.UnionLibre:
                    return "UnionLibre";
                default:
                    return "Rol no implementado";
            }
        }
    }
}
