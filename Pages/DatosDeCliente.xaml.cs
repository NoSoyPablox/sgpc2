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
        public DatosDeCliente()
        {
            InitializeComponent();
        }


        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbCURP.Text) || string.IsNullOrEmpty(tbName.Text) || string.IsNullOrEmpty(tbFirstSurname.Text))
            {
                MessageBox.Show("Los campos no pueden estar vacios", "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {

                if (Validator.ValidateCURP(tbCURP.Text) && Validator.ValidateNames(tbName.Text) && Validator.ValidateNames(tbFirstSurname.Text))
                {
                    /*using (ModelContainer db = new ModelContainer())
                    {
                        Customer customerToRegister = new Customer();
                        customerToRegister.Curp = "CUMP01050123";
                        customerToRegister.Name = "Juan";
                        customerToRegister.FirstSurname = "Mesa";
                        customerToRegister.SecondSurname = "Murrieta";

                        db.Customers.Add(customerToRegister);

                        try
                        {
                            // Guardamos los cambios en la base de datos
                            db.SaveChanges();
                            Console.WriteLine("Cliente registrado exitosamente.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ocurrió un error al intentar registrar el cliente: " + ex.Message);
                        }
                    }*/

                    using (ModelContainer db = new ModelContainer())
                    {
                        WorkCenter workCenter = new WorkCenter();
                        workCenter.Name = "Centro de trabajo 1";
                        workCenter.Address = "Calle 1";
                        workCenter.PhoneNumber = "1234567890";

                        db.WorkCenters.Add(workCenter);
                        try
                        {
                            db.SaveChanges();
                        }catch (Exception ex)
                        {
                            Console.WriteLine("Ocurrió un error al intentar registrar el centro de trabajo: " + ex.Message);
                        }

                    };

                    MessageBox.Show("Datos guardados correctamente", "Operación realizada con exito", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else
                {
                    MessageBox.Show("Los datos introducidos son incorrectos", "Información invalida", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }   


        private void tbName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbFirstSurname.Text + e.Text, @"[^a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$"))
            {
                e.Handled = true;
            }
        }

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
    }
}
