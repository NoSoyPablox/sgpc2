using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Data.Entity.Validation;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Windows.Media;



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

        Dictionary<TextBox, Label> textBoxLabelMap;

        public ContactInformation(bool isEditable, int CustomerId)
        {
            InitializeComponent();
            dbContext = new sgscEntities();
            userContactInformation = dbContext.CustomerContactInfoes.FirstOrDefault(c => c.CustomerId == 1);
            ShowInformationContact(userContactInformation);

            this.CustomerId = CustomerId;
            this.isEditable = isEditable;
            
            if (isEditable)
            {
                try
                {
                    userContactInformation = dbContext.CustomerContactInfoes.FirstOrDefault(c => c.CustomerContactInfoId == 2);
                    ShowInformationContact(userContactInformation);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al intentar recuperar la información de la base de datos: {ex.Message}");
                }
            }
            textBoxLabelMap = new Dictionary<TextBox, Label>
        {
            {txtPhoneOne, lbPhoneOneIsEmpty},
            {txtPhoneTwo, lbPhoneTwoIsEmpty},
            {txtEmail, lbEmailIsEmpty},
        };
        }

        private void BtnClicContinue(object sender, RoutedEventArgs e)
        {
            if (ValidateFields())
            {
                try
                {
                    if (isEditable)
                    {
                        userContactInformation.PhoneNumberOne = txtPhoneOne.Text;
                        userContactInformation.PhoneNumberTwo = txtPhoneTwo.Text;
                        userContactInformation.Email = txtEmail.Text;
                    }
                    else
                    {
                        // Cuando es nuevo
                    }

                    dbContext.SaveChanges();

                    MessageBox.Show("Operación realizada correctamente.");

                    // Ocultar los labels de campos vacíos
                    foreach (var pair in textBoxLabelMap)
                    {
                        pair.Value.Visibility = Visibility.Hidden;
                    }
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

        public bool ValidateFields()
        {
            bool IsValidate = true;

            if (string.IsNullOrWhiteSpace(txtPhoneOne.Text)
                || string.IsNullOrWhiteSpace(txtPhoneTwo.Text)
                || string.IsNullOrWhiteSpace(txtEmail.Text)
            {
                IsValidate = false;


                foreach (var pair in textBoxLabelMap)
                {
                    CheckAndSetLabelVisibility(pair.Value, pair.Key);
                }

            }
            else
            {
                IsValidate = true;
            }
            return IsValidate;
        }

        private void CheckAndSetLabelVisibility(Label label, TextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                label.Visibility = Visibility.Visible;
                textBox.BorderBrush = Brushes.Red;
            }
            else
            {
                textBox.ClearValue(Border.BorderBrushProperty);
                label.Visibility = Visibility.Hidden;
            }
        }
    }
}
