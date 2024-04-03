using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SGSC.Pages
{
    public partial class PageWorkCenter : Page
    {
        string WorkCenterName = "";
        string Phone = "";
        string Street = "";
        string InnerNumber = "";
        string OutsideNumber = "";
        string Colony = "";
        string ZipCode = "";
        int CustomerId;
        SGSC.WorkCenter userWorkCenter = null;
        bool isEditable = false;

        private sgscEntities dbContext;

        Dictionary<TextBox, Label> textBoxLabelMap;

        public PageWorkCenter(bool isEditable, int CustomerId)
        {
            InitializeComponent();
            dbContext = new sgscEntities();
            this.CustomerId = CustomerId;
            this.isEditable = isEditable;

            txtWorkCenterName.PreviewTextInput += AllowWriteLetters;
            txtStreet.PreviewTextInput += AllowWriteLetters;
            txtColony.PreviewTextInput += AllowWriteLetters;

            txtPhone.PreviewTextInput += AllowPhoneNumber;
            txtInnerNumber.PreviewTextInput += AllowWriteNumbers;
            txtOutsideNumber.PreviewTextInput += AllowWriteNumbers;
            txtZipCode.PreviewTextInput += AllowZipCode;   
            

            if (isEditable)
            {
                try
                {
                    userWorkCenter = dbContext.WorkCenters.FirstOrDefault(c => c.CustormerId == 2);
                    ShowInformationWorkCenter(userWorkCenter);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al intentar recuperar la información de la base de datos: {ex.Message}");
                }
            }
            textBoxLabelMap = new Dictionary<TextBox, Label>
        {
            {txtWorkCenterName, lbIsEmptyCenterName},
            {txtPhone, lbIsEmptyPhone},
            {txtStreet, lbIsEmptyStreet},
            {txtColony, lbIsEmptyColony},
            {txtInnerNumber, lbIsEmptyInnerNumber},
            {txtOutsideNumber, lbIsEmptyOutsideNumber},
            {txtZipCode, lbIsEmptyZipCode}
        };
        }

        public void ShowInformationWorkCenter(SGSC.WorkCenter userWorkCenter)
        {
            if (userWorkCenter != null)
            {
                // Mostrar la información recuperada en los TextBox correspondientes
                txtWorkCenterName.Text = userWorkCenter.CenterName;
                txtPhone.Text = userWorkCenter.PhoneNumber;
                txtStreet.Text = userWorkCenter.Street;
                txtStreet.Text = userWorkCenter.Street;
                txtColony.Text = userWorkCenter.Colony;
                txtZipCode.Text = userWorkCenter.ZipCode.ToString();
                txtInnerNumber.Text = userWorkCenter.InnerNumber.ToString();
                txtOutsideNumber.Text = userWorkCenter.OutsideNumber.ToString();
            }
            else
            {
                MessageBox.Show("No se encontró la información del centro de trabajo del usuario");
            }
        }

        public bool ValidateFields()
        {
            bool IsValidate = true;

            if (string.IsNullOrWhiteSpace(txtWorkCenterName.Text) 
                || string.IsNullOrWhiteSpace(txtPhone.Text) 
                || string.IsNullOrWhiteSpace(txtStreet.Text)
                || string.IsNullOrWhiteSpace(txtColony.Text) 
                || string.IsNullOrWhiteSpace(txtInnerNumber.Text) 
                || string.IsNullOrWhiteSpace(txtOutsideNumber.Text) 
                || string.IsNullOrWhiteSpace(txtZipCode.Text))
            {
                IsValidate = false;
                

                foreach (var pair in textBoxLabelMap)
                {
                    CheckAndSetLabelVisibility(pair.Value, pair.Key);
                }
                
            } else
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


        private void BtnClicContinue(object sender, RoutedEventArgs e)
        {
            if (ValidateFields())
            {
                try
                {
                    if (isEditable)
                    {
                        userWorkCenter.CenterName = txtWorkCenterName.Text;
                        userWorkCenter.PhoneNumber = txtPhone.Text;
                        userWorkCenter.Street = txtStreet.Text;
                        userWorkCenter.Colony = txtColony.Text;
                        userWorkCenter.InnerNumber = int.Parse(txtInnerNumber.Text);
                        userWorkCenter.OutsideNumber = int.Parse(txtOutsideNumber.Text);
                        userWorkCenter.ZipCode = int.Parse(txtZipCode.Text);
                    }
                    else
                    {
                        WorkCenterName = txtWorkCenterName.Text;
                        Phone = txtPhone.Text;
                        Street = txtStreet.Text;
                        InnerNumber = txtInnerNumber.Text;
                        int IntInnerNumber = int.Parse(InnerNumber);
                        OutsideNumber = txtOutsideNumber.Text;
                        int IntOutsideNumber = int.Parse(OutsideNumber);
                        Colony = txtColony.Text;
                        ZipCode = txtZipCode.Text;
                        int IntZipCode = int.Parse(ZipCode);

                        SGSC.WorkCenter NewWorkcenter = new SGSC.WorkCenter
                        {
                            CenterName = WorkCenterName,
                            PhoneNumber = Phone,
                            Street = Street,
                            InnerNumber = IntInnerNumber,
                            OutsideNumber = IntOutsideNumber,
                            Colony = Colony,
                            ZipCode = IntZipCode,
                            CustormerId = 2
                        };

                        dbContext.WorkCenters.Add(NewWorkcenter);
                    }

                    // Intenta guardar cambios en la base de datos
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

        private void AllowWriteLetters(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[a-zA-Z]+$");
        }

        
        private void AllowPhoneNumber(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                string currentText = textBox.Text;
                string newText = currentText + e.Text;

                if (newText.Length > 10)
                {
                    e.Handled = true; 
                    return;
                }
            }
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$"); 
        }

        private void AllowZipCode(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                string currentText = textBox.Text;

                string newText = currentText + e.Text;

                if (newText.Length > 6)
                {
                    e.Handled = true;
                    return;
                }
            }
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$"); 
        }

        private void AllowWriteNumbers(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                string currentText = textBox.Text;

                string newText = currentText + e.Text;

                if (newText.Length > 5)
                {
                    e.Handled = true;
                    return;
                }
            }
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }


    }
}
