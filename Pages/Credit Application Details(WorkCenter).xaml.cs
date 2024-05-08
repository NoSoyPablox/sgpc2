using SGSC.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography.X509Certificates;
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
    public partial class Credit_Application_Details_WorkCenter_ : Page
    {
        private int? requestId;

        public Credit_Application_Details_WorkCenter_(int? requestId)
        {
            InitializeComponent();
            StepsSidebarFrame.Content = new CreditApplicationDataStepsSidebar();
            UserSessionFrame.Content = new UserSessionFrame();
            this.requestId = requestId;
            getWorkCenterInformation(requestId);
        }

        public void getWorkCenterInformation(int? requestId)
        {
            using (sgscEntities db = new sgscEntities())
            {
                var workCenterQuery = (from cr in db.CreditRequests
                             join wc in db.WorkCenters on cr.CustomerId equals wc.Customer_CustomerId
                             where cr.CreditRequestId == requestId
                             select new
                             {
                                 WorkCenterName = wc.CenterName,
                                 PhoneNumber = wc.PhoneNumber,
                                 Street = wc.Street,
                                 Colony = wc.Colony,
                                 InnerNumber = wc.InnerNumber,
                                 OutsideNumber = wc.OutsideNumber,
                                 ZipCode = wc.ZipCode,
                                 EmployeeNumber = wc.EmployeeNumber,
                                 DateOfContract = wc.dateOfContract,
                                 Occupation = wc.occupation,
                                 Position = wc.position
                             }).FirstOrDefault();
                if (workCenterQuery == null)
                {
                    MessageBox.Show("The specified request was not found.");
                    return;
                }

                lbCompanyName.Content = workCenterQuery.WorkCenterName;
                lbEmployeeNumber.Content = workCenterQuery.EmployeeNumber;
                lbPhoneNumber.Content = workCenterQuery.PhoneNumber;
                lbOccupation.Content = workCenterQuery.Occupation;
                lbPosition.Content = workCenterQuery.Position;
                lbEmployeeNumber.Content = workCenterQuery.EmployeeNumber;
                CalculateLengthOfService((DateTime)workCenterQuery.DateOfContract);
            }
        }

        public void CalculateLengthOfService(DateTime dateOfHire)
        {
            DateTime currentDate = DateTime.Today;

            int years = currentDate.Year - dateOfHire.Year;
            int Months = currentDate.Month - dateOfHire.Month;

            if (Months < 0)
            {
                
                years--;
                Months += 12;
            }

            lbYearsWork.Content = years;
            lbMonthsWork.Content = Months;
        }

        private void btnClicContinue(object sender, RoutedEventArgs e)
        {
            var customerInfoPage = new Credit_Application_Details_personal_references_(requestId);

            if (NavigationService != null)
            {
                NavigationService.Navigate(customerInfoPage);
            }
        }
    }
}
