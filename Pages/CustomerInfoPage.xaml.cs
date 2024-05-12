using SGSC.Frames;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SGSC.Pages
{
    /// <summary>
    /// Lógica de interacción para DatosDeCliente.xaml
    /// </summary>
    public partial class CustomerInfoPage : Page
    {
        private int? CustomerId = null;

        public CustomerInfoPage(int? customerId = null)
        {
            InitializeComponent();
            CustomerId = customerId;
            if (CustomerId != null)
            {
                getCustomerInfo(CustomerId.Value);
            }

            StepsSidebarFrame.Content = new CustomerRegisterStepsSidebar("PersonalInfo");
            UserSessionFrame.Content = new UserSessionFrame();

            // Clear the error labels
            lbName.Content = "";
            lbFirstSurname.Content = "";
            lbSecondSurname.Content = "";
            lbCurp.Content = "";
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            lbName.Content = "";
            lbFirstSurname.Content = "";
            lbSecondSurname.Content = "";
            lbCurp.Content = "";

            bool valid = true;
            if (string.IsNullOrEmpty(tbName.Text))
            {
                valid = false;
                lbName.Content = "Por favor introduzca el nombre";
            }
            if (string.IsNullOrEmpty(tbFirstSurname.Text))
            {
                valid = false;
                lbFirstSurname.Content = "Por favor introduzca el apellido paterno";
            }
            if (string.IsNullOrEmpty(tbSecondSurname.Text))
            {
                valid = false;
                lbSecondSurname.Content = "Por favor introduzca el apellido materno";
            }
            if (string.IsNullOrEmpty(tbCURP.Text))
            {
                valid = false;
                lbCurp.Content = "Por favor introduzca el CURP";
            }
            if (!Utils.TextValidator.ValidateCURP(tbCURP.Text))
            {
                valid = false;
                lbCurp.Content = "Por favor introduzca un CURP válido";
            }
            if (!valid)
            {
                return;
            }
            if (CustomerId == null)
            {
                registerCustomer();
            }
            else
            {
                updateCustomer();
            }
        }   

        private void registerCustomer()
        {
            using (sgscEntities db = new sgscEntities())
            {
                try
                {
                    Customer customerToRegister = new Customer();
                    customerToRegister.Curp = tbCURP.Text;
                    customerToRegister.Name = tbName.Text;
                    customerToRegister.FirstSurname = tbFirstSurname.Text;
                    customerToRegister.SecondSurname = tbSecondSurname.Text;
                    customerToRegister = db.Customers.Add(customerToRegister);

                    db.SaveChanges();
                    MessageBox.Show("Cliente registrado exitosamente.");
                    tbCURP.Text = "";
                    tbName.Text = "";
                    tbFirstSurname.Text = "";
                    tbSecondSurname.Text = "";
                    
                    App.Current.MainFrame.Content = new AddressInformationPage(customerToRegister.CustomerId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error al intentar registrar el cliente: " + ex.Message);
                    MessageBox.Show("Ocurrió un error al intentar actualizar el cliente.");
                }
            }
        }

        private void updateCustomer()
        {
            using (sgscEntities db = new sgscEntities())
            {
                try
                {
                    Customer customerToUpdate = db.Customers.Find(CustomerId);
                    customerToUpdate.Curp = tbCURP.Text;
                    customerToUpdate.Name = tbName.Text;
                    customerToUpdate.FirstSurname = tbFirstSurname.Text;
                    customerToUpdate.SecondSurname = tbSecondSurname.Text;

                    db.SaveChanges();
                    MessageBox.Show("Cliente actualizado exitosamente.");
                    
                    App.Current.MainFrame.Content = new AddressInformationPage(CustomerId.Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error al intentar actualizar el cliente.");
                    Console.WriteLine("Ocurrió un error al intentar actualizar el cliente: " + ex.Message);
                }
            }
        }

        private void tbCURP_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (tbCURP.Text.Length >= 18 || System.Text.RegularExpressions.Regex.IsMatch(tbCURP.Text + e.Text, @"[^a-zA-Z0-9]+$"))
            {
                e.Handled = true;
            }
        }

        private void getCustomerInfo(int idCustomer)
        {
            using (sgscEntities db = new sgscEntities())
            {
                Customer customer = db.Customers.Find(idCustomer);
                if (customer == null)
                {
                    MessageBox.Show("No se encontró el cliente seleccionado");
                    App.Current.MainFrame.GoBack();
                    return;
                }
                tbCURP.Text = customer.Curp;
                tbName.Text = customer.Name;
                tbFirstSurname.Text = customer.FirstSurname;
                tbSecondSurname.Text = customer.SecondSurname;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainFrame.GoBack();
        }
    }
}
