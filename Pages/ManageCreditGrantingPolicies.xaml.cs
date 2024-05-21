using SGSC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SGSC.Pages
{
    public partial class ManageCreditGrantingPolicies : Page
    {

        private int customerId;
        public List<CreditPolicyWithStatus> CreditPolicies { get; set; }
        public ManageCreditGrantingPolicies()
        {
            InitializeComponent();
            DataContext = this;
            ShowCreditPolicies();
        }

        public void ShowCreditPolicies()
        {
            try
            {
                using (SGSCEntities db = new SGSCEntities())
                {
                    var creditPoliciesFromDb = db.CreditPolicies.ToList();

                    var creditPolicies = creditPoliciesFromDb
                        .Select(policy => new CreditPolicyWithStatus(
                            policy.CreditPolicyId,
                            policy.Name,
                            policy.Description,
                            policy.EffectiveDate ?? DateTime.MinValue, // Asegurarse de manejar fechas nulas
                            policy.EffectiveDate > DateTime.Now ? "Vigente" : "Vencida"
                        ))
                        .ToList();

                    CreditPolicies = creditPolicies;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las políticas de crédito: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnClickAddCreditPolicie(object sender, RoutedEventArgs e)
        {
            // Navega a la página AddCreditPolicy para agregar una nueva política de crédito
            NavigationService.Navigate(new AddCreditPolicy(null));
        }


        private void BtnUpdatePolicyCredit(object sender, RoutedEventArgs e)
        {
            // Verifica si hay una fila seleccionada en el DataGrid
            if (dataGridCreditPolicies.SelectedItem != null)
            {
                // Obtiene la política de crédito seleccionada
                CreditPolicyWithStatus selectedPolicy = (CreditPolicyWithStatus)dataGridCreditPolicies.SelectedItem;
                // Navega a la página AddCreditPolicy para modificar la política de crédito seleccionada
                NavigationService.Navigate(new AddCreditPolicy(selectedPolicy));
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una política de crédito para modificar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
