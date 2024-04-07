using SGSC.Messages;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

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

            // hide all error labels
            foreach (var pair in textBoxLabelMap)
            {
                pair.Value.Visibility = Visibility.Hidden;
            }
        }

        public void ShowInformationWorkCenter(SGSC.WorkCenter userWorkCenter)
        {
            if (userWorkCenter != null)
            {
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

                    dbContext.SaveChanges();



                    ShowNotification("Se ha registrado con éxito la información", "Success");

                    foreach (var pair in textBoxLabelMap)
                    {
                        pair.Value.Visibility = Visibility.Hidden;
                    }
                }
                catch (EntityException ex)
                {
                    ShowNotification("No se puede conectar con la base de datos. " +
                        "Por favor, inténtelo más tarde.", "Error");
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

        public void ShowNotification(string Message, String NotificationType)
        {
                var notificationWindow = new ToastNotification(Message, NotificationType);
                notificationWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                notificationWindow.Left = SystemParameters.WorkArea.Left; // Ajustar según sea necesario
                notificationWindow.Top = SystemParameters.WorkArea.Bottom - notificationWindow.Height; // Ajustar según sea necesario

                notificationWindow.Show();
                Task.Delay(3000).ContinueWith(_ =>
                {
                    notificationWindow.Dispatcher.Invoke(() =>
                    {
                        notificationWindow.Close();
                    });
                });
        }

        private void BtnClicPageContactInformation(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            PageContactInformation contactInformation = new PageContactInformation(false, 2);
            if (NavigationService != null)
            {
                NavigationService.Navigate(contactInformation);
            }
            clickedButton.Background = Brushes.Green;
        }

        private void BtnClicPageWorkCenter(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            PageWorkCenter workCenter = new PageWorkCenter(false, 2);
            if (NavigationService != null)
            {
                NavigationService.Navigate(workCenter);
            }

            clickedButton.Background = Brushes.Green;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }

   
}
