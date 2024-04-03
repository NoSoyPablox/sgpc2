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
    public partial class DatosDeCliente : Page
    {
        bool isEditable = true;
        int idCustomer = 1;
        public DatosDeCliente(/*bool isEditable, int idCliente*/)
        {
            InitializeComponent();
            if (isEditable)
            {
                getCustomerInfo(idCustomer);
            }
        }


        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            List<string> names = new List<string> {tbName.Text, tbFirstSurname.Text, tbSecondSurname.Text };
            if (!string.IsNullOrEmpty(tbCURP.Text) && !string.IsNullOrEmpty(tbName.Text) && !string.IsNullOrEmpty(tbFirstSurname.Text))
            {
                if (Validator.ValidateCURP(tbCURP.Text) && Validator.ValidateMultipleNames(names))
                {
                    registerCustomer();
                }
                else
                {
                    MessageBox.Show("Los datos introducidos son incorrectos", "Información invalida", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Los campos no pueden estar vacios", "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }   

        private void registerCustomer()
        {
            using (sgscEntities db = new sgscEntities())
            {
                Customer customerToRegister = new Customer();
                customerToRegister.Curp = tbCURP.Text;
                customerToRegister.Name = tbName.Text;
                customerToRegister.FirstSurname = tbFirstSurname.Text;
                customerToRegister.SecondSurname = tbSecondSurname.Text;
                db.Customers.Add(customerToRegister);

                try
                {
                    db.SaveChanges();
                    Console.WriteLine("Cliente registrado exitosamente.");
                    //limpiar campos
                    tbCURP.Text = "";
                    tbName.Text = "";
                    tbFirstSurname.Text = "";
                    tbSecondSurname.Text = "";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error al intentar registrar el cliente: " + ex.Message);
                }
            }
        }

        /*private void tbName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbName.Text + e.Text, @"[^a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$"))
            {
                e.Handled = true;
            }
        }*/

        private void tbFirstSurname_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbFirstSurname.Text + e.Text, @"[^a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$"))
            {
                e.Handled = true;
            }
        }

        private void tbSecondSurname_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbSecondSurname.Text + e.Text, @"[^a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$"))
            {
                e.Handled = true;
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
                tbCURP.Text = customer.Curp;
                tbName.Text = customer.Name;
                tbFirstSurname.Text = customer.FirstSurname;
                tbSecondSurname.Text = customer.SecondSurname;
            }
        }
    }
}
