using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace SGSC.Pages
{
    public partial class Credit_Application_Details_approve_credit_application_ : Page
    {
        private int? RequestId;
        private string Status;

        public Credit_Application_Details_approve_credit_application_(int? requestId)
        {
            InitializeComponent();
            this.RequestId = requestId;
            GetApplicationStatus();
            if(Status == "Autorizar" ||  Status =="Corregir" || Status == "Rechazar")
            {
                ShowInformation();
                GetObservation();
                GetStatus();
                switch (Status)
                {
                    case "Autorizar":
                        DisableFields();
                        break;
                    case "Rechazar":
                        DisableFields();
                        break;
                }
            }
        }

        public void GetObservation()
        {
            using (sgscEntities db = new sgscEntities())
            {
                var comment = db.CreditRequests
                                .Where(cr => cr.CreditRequestId == RequestId)
                                .Select(cr => cr.Description)
                                .FirstOrDefault();
                txtObservations.Text = comment;
            }
        }




        public void GetApplicationStatus()
        {
            using (sgscEntities db = new sgscEntities())
            {
                var status = db.CreditRequests
                              .Where(cr => cr.CreditRequestId == RequestId)
                              .Select(cr => cr.Status)
                              .FirstOrDefault();

                if (status != null)
                {
                    Status = status.ToString();
                }
                else
                {
                    Status = "No se encontró ninguna solicitud con el ID proporcionado.";
                }
            }
        }

        public void ShowInformation()
        {
            ShowCreditPolicies();
        }

        private void MarkCheckbox(CheckBox checkbox)
        {
            checkbox.IsChecked = true;
        }


        public List<string> GetCreditPoliciesForRequest()
        {
            List<string> policies = new List<string>();

            using (sgscEntities db = new sgscEntities())
            {
                var creditPolicies = (from cr in db.CreditRequests
                                      join crcp in db.CreditRequestCreditPolicy
                                      on cr.CreditRequestId equals crcp.CreditRequestId
                                      join cp in db.CreditPolicy
                                      on crcp.CreditPolicyId equals cp.CreditPolicyId
                                      where cr.CreditRequestId == RequestId
                                      select cp.Description).ToList();

                policies.AddRange(creditPolicies);
            }

            return policies;
        }

        public void GetStatus()
        {
            using (sgscEntities db = new sgscEntities())
            {
                var status = db.CreditRequests
                             .Where(cr => cr.CreditRequestId == 3)
                             .Select(cr => cr.Status)
                             .FirstOrDefault();

                switch (status)
                {
                    case "Autorizar":
                        rbtAutorize.IsChecked = true;
                        break;
                    case "Corregir":
                         rbtCorrect.IsChecked = true;
                        break;
                    case "Rechazar":
                        rbtReject.IsChecked = true;
                        break;
                }


            }


        }

        public void ShowCreditPolicies()
        {
            // Obtener las políticas de crédito para la solicitud actual
            List<string> creditPolicies = GetCreditPoliciesForRequest();

            // Verificar si la lista está vacía
            if (creditPolicies.Count == 0)
            {
                // La lista está vacía, puedes manejar este caso aquí
            }
            else
            {
                // Marcar los checkboxes correspondientes a las políticas de crédito recuperadas
                foreach (string policy in creditPolicies)
                {
                    switch (policy)
                    {
                        case "No se encuentra en lista negra":
                            MarkCheckbox(cbxCreditPolicyOne);
                            break;
                        case "Capacidad de pago":
                            MarkCheckbox(cbxCreditPolicyTwo);
                            break;
                        case "Estabilidad laboral":
                            MarkCheckbox(cbxCreditPolicyThree);
                            break;
                        case "Verificación de identidad":
                            MarkCheckbox(cbxCreditPolicyFour);
                            break;
                        case "Cumplimiento de Requisitos Legales":
                            MarkCheckbox(cbxCreditPolicyFive);
                            break;
                        case "Propósito del Crédito coherente":
                            MarkCheckbox(cbxCreditPolicySix);
                            break;
                        case "Ser mexicano":
                            MarkCheckbox(cbxCreditPolicySeven);
                            break;
                    }
                }
            }
        }

        public void DisableFields()
        {
            btnAcept.IsEnabled = false;
            btnCancel.IsEnabled = false;
            cbxCreditPolicyOne.IsEnabled = false;
            cbxCreditPolicyTwo.IsEnabled = false;
            cbxCreditPolicyThree.IsEnabled = false;
            cbxCreditPolicyFour.IsEnabled = false;
            cbxCreditPolicyFive.IsEnabled = false;
            cbxCreditPolicySix.IsEnabled = false;
            cbxCreditPolicySeven.IsEnabled = false;
        }

        private void BtnClicAcept(object sender, RoutedEventArgs e)
        {

        }
    }
}
