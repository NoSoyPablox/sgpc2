//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SGSC
{
    using System;
    using System.Collections.Generic;
    
    public partial class CustomerAddress
    {
        public int CustomerAddressId { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string ExternalNumber { get; set; }
        public string InternalNumber { get; set; }
        public Nullable<int> CustormerId { get; set; }
    }
}
