using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SGSC.Pages
{
    public partial class WorkCenter : Page
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

        
        public WorkCenter(bool isEditable, int CustomerId)
        {
            InitializeComponent();
            dbContext = new sgscEntities();
            this.CustomerId = CustomerId;
            this.isEditable = isEditable;
            userWorkCenter = dbContext.WorkCenters.FirstOrDefault(c => c.CustormerId == 1);
            if (isEditable)
            {
                ShowInformationWorkCenter(userWorkCenter);
            }
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
                MessageBox.Show("No se encontró información para el usuario con Id = 1.");
            }
        }

        private void BtnClicContinue(object sender, RoutedEventArgs e)
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
                    CustormerId = 1
                };

                dbContext.WorkCenters.Add(NewWorkcenter);
            }
            try
            {
                // Intenta guardar cambios en la base de datos
                dbContext.SaveChanges();
                MessageBox.Show("Operación realizada correctamente.");
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
}
