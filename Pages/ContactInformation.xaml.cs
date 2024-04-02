using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Data.Entity.Validation;



namespace SGSC.Pages
{
    public partial class ContactInformation : Page
    {
        

        private sgscEntities dbContext;

        String PhoneOne = "";
        String PhoneTwo = "";
        String Email = "";
        int CustomerId;
        SGSC.CustomerContactInfo userContactInformation = null;
        bool isEditable = false;

        public ContactInformation(bool isEditable, int CustomerId)
        {
            InitializeComponent();
            dbContext = new sgscEntities();
            userContactInformation = dbContext.CustomerContactInfoes.FirstOrDefault(c => c.CustomerId == 1);
            ShowInformationContact(userContactInformation);

            this.CustomerId = CustomerId;
            this.isEditable = isEditable;
            userContactInformation = dbContext.CustomerContactInfoes.FirstOrDefault(c => c.CustomerContactInfoId == 1);
            if (isEditable)
            {
                ShowInformationContact(userContactInformation);
            }
        }

        private void BtnClicContinue(object sender, RoutedEventArgs e)
        {
            if (isEditable)
            {
                userContactInformation.PhoneNumberOne = txtPhoneOne.Text;
                userContactInformation.PhoneNumberTwo = txtPhoneTwo.Text;
                userContactInformation.Email = txtEmail.Text;
            }
            else
            {
                // Poner código de cuando no es editable
            }
                

            try
            {
                dbContext.SaveChanges();
                MessageBox.Show("Información actualizada correctamente.");
            }
            catch (DbEntityValidationException ex)
            {
                // Itera sobre cada error de validación
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    // Itera sobre cada error de validación para esta entidad
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        // Accede a los detalles del error de validación
                        Console.WriteLine($"Propiedad: {validationError.PropertyName}");
                        Console.WriteLine($"Error: {validationError.ErrorMessage}");
                    }
                }
            }
        }

        public void ShowInformationContact(SGSC.CustomerContactInfo userContactInformation)
        {
            if (userContactInformation != null)
            {
                // Mostrar la información recuperada en los TextBox correspondientes
                txtPhoneOne.Text = userContactInformation.PhoneNumberOne;
                txtPhoneTwo.Text = userContactInformation.PhoneNumberTwo;
                txtEmail.Text = userContactInformation.Email;

                Console.WriteLine(txtEmail);
                Console.WriteLine(txtPhoneOne.Text);
                Console.WriteLine(txtPhoneTwo.Text);

                Console.WriteLine("Información BD" + userContactInformation.Email + " " + userContactInformation.PhoneNumberTwo + " " + userContactInformation.PhoneNumberOne);

            }
            else
            {
                MessageBox.Show("No se encontró información para el usuario con Id = 1.");
            }
        }
    }
}
