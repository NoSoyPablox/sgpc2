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
    
    public partial class Localization
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Localization()
        {
            this.CustomerAddresses = new HashSet<CustomerAddresses>();
        }
    
        public int LocalizationId { get; set; }
        public string Colony { get; set; }
        public string ZipCode { get; set; }
        public string Municipality { get; set; }
        public string State { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerAddresses> CustomerAddresses { get; set; }
    }
}
