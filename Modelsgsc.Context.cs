﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class sgscEntities : DbContext
    {
        public sgscEntities()
            : base("name=sgscEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<CreditCondition> CreditConditions { get; set; }
        public virtual DbSet<CreditPolicy> CreditPolicies { get; set; }
        public virtual DbSet<CreditRequest> CreditRequests { get; set; }
        public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public virtual DbSet<CustomerContactInfo> CustomerContactInfoes { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<WorkCenter> WorkCenters { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Colony> Colonies { get; set; }
        public virtual DbSet<CreditRequestCreditPolicy> CreditRequestCreditPolicies { get; set; }
        public virtual DbSet<CreditPromotion> CreditPromotions { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<CreditOpeningForm> CreditOpeningForms { get; set; }
        public virtual DbSet<CreditRequestDocument> CreditRequestDocuments { get; set; }
        public virtual DbSet<DomicilePaymentsAuth> DomicilePaymentsAuths { get; set; }
        public virtual DbSet<LayoutPayment> LayoutPayments { get; set; }
    }
}
