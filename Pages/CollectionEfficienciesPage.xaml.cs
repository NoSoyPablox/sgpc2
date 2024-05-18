using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static SGSC.Pages.CollectionEfficienciesPage;

namespace SGSC.Pages
{
    /// <summary>
    /// Interaction logic for CollectionEfficienciesPage.xaml
    /// </summary>
    public partial class CollectionEfficienciesPage : Page
    {
        public int CreditRequestId { get; set; }

        public class CreditRequest
        {
            public int CreditRequestID { get; set; }
            public string Folio { get; set; }
            public string ClientName { get; set; }
            public int Term { get; set; } // Plazo en quincenas
            public decimal TotalAmount { get; set; }
            public string TotalAmountString { get; set; }
            public decimal OutstandingBalance { get; set; }
            public decimal Efficiency { get; set; }
        }

        public class Payment
        {
            public int PaymentID { get; set; }
            public int CreditRequestID { get; set; }
            public string FileNumber { get; set; }
            public DateTime PaymentDate { get; set; }
            public decimal Discount { get; set; } // El descuento que corresponde a pagar en ese pago
            public decimal Charge { get; set; } // Lo que se pagaría en esa quincena
            public decimal Efficiency { get; set; } // El % calculado de todos los porcentajes
            public bool IsTotalRow { get; set; } // Indica si es una fila de totales
        }

        public CollectionEfficienciesPage(int creditRequestId)
        {
            public int CreditRequestID { get; set; }
            public string Folio { get; set; }
            public string ClientName { get; set; }
            public int Term { get; set; }
            public decimal TotalAmount { get; set; }
            public string TotalAmountString { get; set; }
            public decimal OutstandingBalance { get; set; }
            public decimal Efficiency { get; set; }
        }

        public class Payment
        {
            public int PaymentID { get; set; }
            public int CreditRequestID { get; set; }
            public string FileNumber { get; set; }
            public DateTime PaymentDate { get; set; }
            public decimal Discount { get; set; }
            public decimal Amount { get; set; }
            public decimal Efficiency { get; set; }
            public bool IsTotalRow { get; set; }

            public string EfficiencyString => $"{Efficiency:F2}%";
        }

        public CollectionEfficienciesPage(int creditRequestId)
        {
            InitializeComponent();
        }
    }
}
