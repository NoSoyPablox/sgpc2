using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

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
                if (!IsValidEmail(Tb_Email.Text) || !IsValidPhoneNumber(Tb_PhoneNumberOne.Text) || !IsValidPhoneNumber(Tb_PhoneNumberTwo.Text))
                {
                    MessageBox.Show("Por favor, ingrese datos válidos en los campos solicitados.");
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
                    context.CustomerContactInfoes.AddOrUpdate(newCustomerContactInfo);
                    context.SaveChanges();
                }

                MessageBox.Show("Los datos de contacto se han guardado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar guardar los datos de contacto: " + ex.Message);
            }
        }
        private void UpdateContactInformation()
        {
            try
            {
                using (var context = new sgscEntities())
                {
                    var customerData = context.CustomerContactInfoes
                        .Where(customerDb => customerDb.CustomerContactInfoId == customer)
                        .FirstOrDefault();

                    if (customerData != null)
                    {
                        Tb_PhoneNumberOne.Text = customerData.PhoneNumberOne;
                        Tb_PhoneNumberTwo.Text = customerData.PhoneNumberTwo;
                        Tb_Email.Text = customerData.Email;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar obtener los datos de contacto: " + ex.Message);
            }
        }


        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^\d{10}$");
        }
    }
}
