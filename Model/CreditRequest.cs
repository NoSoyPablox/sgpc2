﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGSC
{
    public partial class CreditRequest
    {


        public enum RequestStatus
        {
            Captured,
            Pending,
            Rejected,
            WaitingForCorrection,
            Authorized,
            Paid,
            
        }

        public static string RequestStatusToString(RequestStatus requestStatus)
        {
            switch (requestStatus)
            {
                case RequestStatus.Paid:
                    return "Pagado";
                case RequestStatus.Authorized:
                    return "Aprobada";
                case RequestStatus.Rejected:
                    return "Rechazada";
                case RequestStatus.Captured:
                    return "Capturada";
                case RequestStatus.WaitingForCorrection:
                    return "En espera de corrección";
                 case RequestStatus.Pending:
                    return "Pendiente";
                default:
                    return "Estado de solicitud desconocido";
            }
        }

    }
}
