//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SGSC
{
    using System;
    using System.Collections.Generic;
    
    public partial class CreditRequestCreditPolicy
    {
        public int IdCreditRequestCreditPolicy { get; set; }
        public Nullable<int> CreditPolicyId { get; set; }
        public int CreditRequestId { get; set; }
    
        public virtual CustomerContactInfo CustomerContactInfo { get; set; }
        public virtual Contact Contact { get; set; }
    }
}
