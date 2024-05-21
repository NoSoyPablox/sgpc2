using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGSC.Utils
{
    public class CreditRequestStatus
    {
        public enum State : short
        {
            Autorizado = 0,
            Corregir,
            Rechazado,
            EnRevision,
        }

        public static string GetRequestStatus(int role)
        {
            switch (role)
            {
                case (short)State.Autorizado:
                    return "Autorizar";
                case (short)State.Corregir:
                    return "Corregir";
                case (short)State.Rechazado:
                    return "Rechazar";
                case (short)State.EnRevision:
                    return "En revisión";
                default:
                    return "Estatus no implementado";
            }
        }
    }
}
