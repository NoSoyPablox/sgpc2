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

namespace SGSC.Pages
{
    /// <summary>
    /// Lógica de interacción para ReferenciasDelCliente.xaml
    /// </summary>
    public partial class ReferenciasDelCliente : Page
    {
        int idCustomer = 1;
        public ReferenciasDelCliente()
        {
            InitializeComponent();
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            List<string> names = new List<string> { tbName.Text, tbFirstSurname.Text, tbSecondSurname.Text, tbName1.Text, tbFirstSurname2.Text, tbSecondSurname2.Text };


            if (!names.All(name => !string.IsNullOrEmpty(name)))
            {
                MessageBox.Show("Los campos no pueden estar vacios", "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                //validate the fields
                if (Utils.TextValidator.ValidateMultipleNames(names))
                {
                    using (sgscEntities db = new sgscEntities())
                    {
                        Contact contact1 = new Contact();
                        contact1.Name = tbName.Text;
                        contact1.FirstSurname = tbFirstSurname.Text;
                        contact1.SecondSurname =tbSecondSurname.Text;
                        db.Contacts.Add(contact1);

                        Contact contact2 = new Contact();
                        contact2.Name = tbName1.Text;
                        contact2.FirstSurname = tbFirstSurname2.Text;
                        contact2.SecondSurname = tbSecondSurname2.Text;
                        db.Contacts.Add(contact2);


                        try
                        {
                            db.SaveChanges();
                            Console.WriteLine("Cliente registrado exitosamente.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error al registrar el cliente: " + ex.Message);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Los campos no cumplen con el formato correcto", "Campos incorrectos", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void tbName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbName.Text + e.Text, @"[^a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$"))
            {
                e.Handled = true;
            }
        }

        private void tbName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
