using SGSC.Frames;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SGSC.Pages
{
    public partial class Credit_Application_Details_personal_information_ : Page
    {
        private int? requestId;

        public Credit_Application_Details_personal_information_(int? requestId)
        {
            InitializeComponent();
            StepsSidebarFrame.Content = new CreditApplicationDataStepsSidebar();
            UserSessionFrame.Content = new UserSessionFrame();
            this.requestId = requestId;

            if (requestId > 0)
            {
                getPersonalInformation();
            }
        }

        public void getPersonalInformation()
        {
            using (sgscEntities db = new sgscEntities())
            {
                var creditRequest = (from cr in db.CreditRequests
                    where cr.CreditRequestId == requestId
                                     join cus in db.Customers on cr.CustomerId equals cus.CustomerId
                                            join cc in db.CustomerContactInfoes on cus.CustomerId equals cc.CustomerId
                                            select new
                                            {
                                                Name = cus.Name,
                                                FirstSurname = cus.FirstSurname,
                                                SecondSurname = cus.SecondSurname,
                                                DateOfBirthay = cus.DateOfBirthay,
                                                Gender = cus.Gender,
                                                Curp = cus.Curp,
                                                Email = cc.Email,
                                                MaritalStatus = cus.maritalStatus,
                                                PhoneOne = cc.PhoneNumberOne,
                                                PhoneTwo = cc.PhoneNumberTwo
                                            }).FirstOrDefault();

                if (creditRequest == null)
                {
                    MessageBox.Show("The specified request was not found.");
                    return;
                }

                lbName.Content = creditRequest.Name;
                lbFirstSurname.Content = creditRequest.FirstSurname;
                lbSecondSurname.Content= creditRequest.SecondSurname;
                lbDateOfBirth.Content = creditRequest.DateOfBirthay;
                CheckCheckboxGender(creditRequest.Gender);
                lbCurp.Content = creditRequest.Curp;
                lbEmail.Content = creditRequest.Email;
                //Tipo de domicilio
                lbPhoneOne.Content = creditRequest.PhoneOne;
                lbPhoneTwo.Content = creditRequest.PhoneTwo;
                CheckCheckboxMaritalStatus(creditRequest.MaritalStatus);
            }
        }

        public void CheckCheckboxGender(String gender)
        {
            switch (gender.ToLower())
            {
                case "femenino":
                    cbxWoman.IsChecked = true;
                    break;
                case "masculino":
                    cbxMan.IsChecked = true;
                    break;
            }
        }

        public void CheckCheckboxMaritalStatus(String maritalStatus)
        {
            switch (maritalStatus.ToLower())
            {
                case "soltero(a)":
                    cbxSingle.IsChecked = true;
                    break;
                case "casado(a)":
                    cbxMarried.IsChecked = true;
                    break;
                case "divorciado(a)":
                    cbxDivorced.IsChecked = true;
                    break;
                case "viudo(a)":
                    cbxMortgaged.IsChecked = true;
                    break;
                case "unión libre":
                    cbxFreeUnion.IsChecked = true;
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var customerInfoPage = new Credit_Application_Details_WorkCenter_(requestId);

            if (NavigationService != null)
            {
                NavigationService.Navigate(customerInfoPage);
            }
        }
    }
}
