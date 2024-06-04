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
    
    public partial class CreditRequestDocument
    {
        public string FileNumber { get; set; }
        public Nullable<double> Amount { get; set; }
        public string Purpose { get; set; }
        public Nullable<double> InterestRate { get; set; }
        public Nullable<System.DateTime> CreationDateRequest { get; set; }
        public int EmployeeId { get; set; }
        public string InterbankCode { get; set; }
        public string BankName { get; set; }
        public string CustomerName { get; set; }
        public string FirstSurname { get; set; }
        public string SecondSurname { get; set; }
        public string Curp { get; set; }
        public string Rfc { get; set; }
        public string Genre { get; set; }
        public System.DateTime BirthDate { get; set; }
        public int CivilStatus { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string InternalNumber { get; set; }
        public string ExternalNumber { get; set; }
        public string Colony { get; set; }
        public string State { get; set; }
        public string CenterName { get; set; }
        public string WorkCenterStreet { get; set; }
        public string WorkCenterColony { get; set; }
        public Nullable<int> WorkCenterInnerNumber { get; set; }
        public Nullable<int> WorkCenterOutsideNumber { get; set; }
        public Nullable<int> WorkCenterZipCode { get; set; }
        public string WorkCenterPhoneNumber { get; set; }
        public string ContactName { get; set; }
        public string ContactFirstSurname { get; set; }
        public string ContactSecondSurname { get; set; }
        public string Relationship { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Email { get; set; }
        public string PromotionName { get; set; }
        public int CreditRequestId { get; set; }
        public int PaymentsInterval { get; set; }
    }
}
