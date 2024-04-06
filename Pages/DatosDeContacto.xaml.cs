using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
    /// Lógica de interacción para DatosDeContacto.xaml
    /// </summary>
    public partial class DatosDeContacto : Page
    {
        private int customer;
        public DatosDeContacto()
        {
            customer = 1;
            InitializeComponent();
            //UpdateContactInformation();
        }

        private void AddContactInformation(object sender, RoutedEventArgs e)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(Tb_Email.Text) || string.IsNullOrWhiteSpace(Tb_PhoneNumberOne.Text) ||
                    string.IsNullOrWhiteSpace(Tb_PhoneNumberTwo.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos solicitados.");
                    return;
                }


                var newCustomerContactInfo = new CustomerContactInfo
                {
                  PhoneNumberOne = Tb_PhoneNumberOne.Text,
                  PhoneNumberTwo = Tb_PhoneNumberTwo.Text,
                  Email = Tb_Email.Text,
                };

                if (customer != 0)
                {
                    newCustomerContactInfo.CustomerContactInfoId = customer;
                }


                using (sgscEntities context = new sgscEntities())
                {
                    context.CustomerContactInfoes.AddOrUpdate();
                    context.SaveChanges();
                }

                MessageBox.Show("Los datos de contacto se han guardado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar guardar los datos de contacto: " + ex.Message);
            }
        }

        /*private void UpdateContactInformation()
        {
            try
            {

                using (var context = new sgscEntities())
                {
                    var customerData = context.CustomerContactInfoes
                        .Where(customerDb => customerDb. == customer)
                        .FirstOrDefault();


                    if (customerData != null)
                    {
                        Tb_PhoneNumberOne = customerData.PhoneNumberOne;
                        Tb_PhoneNumberTwo = customerData.PhoneNumberTwo;    
                        Tb_Email = customerData.Email;  
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar obtener los datos de contacto: " + ex.Message);
            }
        }*/
    }
}
