using SGSC.Frames;
using SGSC.Utils;
using System;
using System.Collections.Generic;
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
using System.Xml.Linq;

namespace SGSC.Pages
{
    public partial class Credit_Application_Details_request_ : Page
    {
        private int? requestId = null;
        public Credit_Application_Details_request_(int requestId)
        {
            
            InitializeComponent();
            StepsSidebarFrame.Content = new CreditApplicationDataStepsSidebar();
            UserSessionFrame.Content = new UserSessionFrame();
            this.requestId = requestId;

            if (requestId > 0)
            {
                getRequestInfo(requestId);
            }
        }

        private void getRequestInfo(int RequestId)
        {
            using (sgscEntities db = new sgscEntities())
            {
                var requestInfo = (from request in db.CreditRequests
                                   join employee in db.Employees on request.EmployeeId equals employee.EmployeeId
                                   where request.CreditRequestId == RequestId
                                   select new
                                   {
                                       VendorName = employee.Name + " " + employee.FirstSurname + " " + employee.SecondSurname,
                                       RequestNumber = request.FileNumber,
                                       CreationDate = request.CreationDate,
                                       RequestedAmountNumber = request.Amount,
                                       Period = request.TimePeriod,
                                       InterestRate = request.CreditPromotion.InterestRate,
                                       Purpose = request.Purpose,
                                       PromotionTerm = request.CreditPromotion.Deadline,
                                   }).FirstOrDefault();

                if (requestInfo == null)
                {
                    MessageBox.Show("The specified request was not found.");
                    return;
                }

                // Asigna los valores a los controles en tu formulario
                lbSeller.Content = requestInfo.VendorName;
                lbRequestFolio.Content = requestInfo.RequestNumber;
                lbCreationDate.Content = requestInfo.CreationDate.ToString();
                lbAmount.Content = requestInfo.RequestedAmountNumber.ToString() + " (" + Utils.Utils.NumberALetter(requestInfo.RequestedAmountNumber) + ")";
                lbInterestRate.Content = requestInfo.InterestRate.ToString();
                lbDestination.Content = requestInfo.Purpose;
            }
        }

        private void StepsSidebarFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        public void CheckCheckboxDeadline(String deadline)
        {
            switch(deadline.ToLower())
            {
                case "semanas":
                    chWeek.IsChecked = true; 
                    break;
                case "quincenas":
                    chFortnights.IsChecked = true;
                    break;
                case "meses":
                    chMonths.IsChecked = true;
                    break;
                default:
                    chOther.IsChecked = true;
                    chOther.Content = deadline;
                    break;
            }
        }

        private void BtnClicContinue(object sender, RoutedEventArgs e)
        {
            var customerInfoPage = new Credit_Application_Details_personal_information_(requestId);

            if (NavigationService != null)
            {
                NavigationService.Navigate(customerInfoPage);
            }
        }
    }
}
