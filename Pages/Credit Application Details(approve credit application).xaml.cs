using System;
using System.Collections;
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
            List<string> creditPolicies = GetCreditPoliciesForRequest();

            if (creditPolicies.Count == 0)
            {
                // La lista está vacía, puedes manejar este caso aquí
            }
            else
            {
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

        public void SaveCreditPolicies()
        {
            List<int> creditPolicyIds = new List<int>();

            if (cbxCreditPolicyOne.IsChecked == true)
            {
                creditPolicyIds.Add(1);
            }

            if (cbxCreditPolicyTwo.IsChecked == true)
            {
                creditPolicyIds.Add(2);
            }

            if (cbxCreditPolicyThree.IsChecked == true)
            {
                creditPolicyIds.Add(3);
            }

            if (cbxCreditPolicyFour.IsChecked == true)
            {
                creditPolicyIds.Add(4);
            }

            if (cbxCreditPolicyFive.IsChecked == true)
            {
                creditPolicyIds.Add(5);
            }
            if (cbxCreditPolicySix.IsChecked == true)
            {
                creditPolicyIds.Add(6);
            }
            if (cbxCreditPolicySeven.IsChecked == true)
            {
                creditPolicyIds.Add(7);
            }

            List<int> previousSelectedPolicyIds = GetCreditPoliciesForRequestIds();

            if (creditPolicyIds != null && creditPolicyIds.Count > 0)
            {
                using (sgscEntities db = new sgscEntities())
                {
                    // Agrega nuevas asociaciones
                    foreach (int policyId in creditPolicyIds)
                    {
                        // Verifica si la política de crédito ya estaba seleccionada previamente
                        if (!previousSelectedPolicyIds.Contains(policyId))
                        {
                            CreditRequestCreditPolicy newAssociation = new CreditRequestCreditPolicy
                            {
                                CreditRequestId = RequestId,
                                CreditPolicyId = policyId
                            };

                            db.CreditRequestCreditPolicy.Add(newAssociation);
                        }
                    }

                    // Elimina las asociaciones que ya no están seleccionadas
                    foreach (int previousPolicyId in previousSelectedPolicyIds)
                    {
                        // Verifica si la política de crédito ya no está seleccionada
                        if (!creditPolicyIds.Contains(previousPolicyId))
                        {
                            CreditRequestCreditPolicy associationToRemove = db.CreditRequestCreditPolicy.FirstOrDefault(x => x.CreditRequestId == RequestId && x.CreditPolicyId == previousPolicyId);

                            if (associationToRemove != null)
                            {
                                db.CreditRequestCreditPolicy.Remove(associationToRemove);
                            }
                        }
                    }

                    db.SaveChanges();
                }
            }
        }

        public List<int> GetCreditPoliciesForRequestIds()
        {
            List<int> policyIds = new List<int>();

            using (sgscEntities db = new sgscEntities())
            {
                // Consulta las asociaciones de políticas de crédito para la solicitud de crédito actual
                var associations = db.CreditRequestCreditPolicy
                                     .Where(x => x.CreditRequestId == RequestId)
                                     .Select(x => x.CreditPolicyId.Value) // Convierte int? a int
                                     .ToList();

                policyIds.AddRange(associations);
            }

            return policyIds;
        }



        public void SaveDescription()
        {
            using (sgscEntities db = new sgscEntities())
            {
                // Busca el objeto CreditRequest con el RequestId dado
                var solicitud = db.CreditRequests.FirstOrDefault(cr => cr.CreditRequestId == RequestId);

                if (solicitud != null)
                {
                    // Modifica la descripción de la solicitud
                    solicitud.Description = txtObservations.Text;

                    // Guarda los cambios en la base de datos
                    db.SaveChanges();
                }
                else
                {
                    // Maneja el caso donde no se encontró la solicitud
                    Console.WriteLine("No se encontró ninguna solicitud con el ID proporcionado.");
                }
            }
        }

        public void SaveStateRequestCredit()
        {
            String state = "";

            if (rbtAutorize.IsChecked== true)
            {
                state = rbtAutorize.Content.ToString();
                DisableFields();
            }
            if (rbtCorrect.IsChecked == true)
            {
                state = rbtCorrect.Content.ToString();
            }
            if (rbtReject.IsChecked == true)
            {
                state = rbtReject.Content.ToString();
            }


            using (sgscEntities db = new sgscEntities())
            {
                // Busca el objeto CreditRequest con el RequestId dado
                var solicitud = db.CreditRequests.FirstOrDefault(cr => cr.CreditRequestId == RequestId);

                if (solicitud != null)
                {
                    // Modifica la descripción de la solicitud
                    solicitud.Status = state;

                    // Guarda los cambios en la base de datos
                    db.SaveChanges();
                }
                else
                {
                    // Maneja el caso donde no se encontró la solicitud
                    Console.WriteLine("No se encontró ninguna solicitud con el ID proporcionado.");
                }
            }
        }


        private void BtnClicAcept(object sender, RoutedEventArgs e)
        {
            SaveCreditPolicies();
            SaveDescription();
            SaveStateRequestCredit();
        }
    }
}
