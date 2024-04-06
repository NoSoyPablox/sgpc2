using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace SGSC.Pages
{
    /// <summary>
    /// Lógica de interacción para AddressInformation.xaml
    /// </summary>
    public partial class AddressInformation : Window
    {
        private int customer;

        public AddressInformation(/*int customerId*/)
        {
            customer = 1;
            InitializeComponent();
            UpdateAddressInformation();
        }

        private void AddAddressInformation(object sender, RoutedEventArgs e)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(Tb_Street.Text) || string.IsNullOrWhiteSpace(Tb_ExternalNumber.Text) ||
                    string.IsNullOrWhiteSpace(Tb_InternalNumber.Text) || string.IsNullOrWhiteSpace(Tb_ZipCode.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos de dirección.");
                    return;
                }


                if (!IsValidZipCode(Tb_ZipCode.Text))
                {
                    MessageBox.Show("Por favor, introduzca un código postal válido.");
                    return;
                }

                var newCustomerAddressInfo = new CustomerAddress
                {
                    Street = Tb_Street.Text,
                    ExternalNumber = Tb_ExternalNumber.Text,
                    InternalNumber = Tb_InternalNumber.Text,
                    ZipCode = Tb_ZipCode.Text
                };

                if (customer != 0)
                {
                    newCustomerAddressInfo.CustomerAddressId = customer;
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

        private void UpdateAddressInformation()
        {
            try
            {

                using (var context = new sgscEntities())
                {
                    var customerData = context.CustomerAddresses
                        .Where(customerDb => customerDb.CustormerId == customer)
                        .FirstOrDefault();


                    if (customerData != null)
                    {
                        Tb_Street.Text = customerData.Street;
                        Tb_ExternalNumber.Text = customerData.ExternalNumber;
                        Tb_InternalNumber.Text = customerData.InternalNumber;
                        Tb_ZipCode.Text = customerData.ZipCode;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar obtener los datos de contacto: " + ex.Message);
            }
        }

        private bool IsValidZipCode(string zipCode)
        {
            // Expresión regular para validar un código postal de 5 dígitos
            return Regex.IsMatch(zipCode, @"^\d{5}$");
        }

        private void CancelRegister(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}