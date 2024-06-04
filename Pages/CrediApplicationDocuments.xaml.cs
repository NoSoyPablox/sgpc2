using Microsoft.Win32;
using SGSC.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace SGSC.Pages
{
    /// <summary>
    /// Lógica de interacción para CrediApplicationDocuments.xaml
    /// </summary>
    public partial class CrediApplicationDocuments : Page
    {
        private sgscEntities _context;
        private int? creditRequestId;
        public CrediApplicationDocuments(int? creditRequestId)
        {
            InitializeComponent();
            _context = new sgscEntities();
            this.creditRequestId = creditRequestId;
        }

        private void btnDownloadCreditRequestDocument_Click(object sender, RoutedEventArgs e)
        {
            var document = _context.Documents.FirstOrDefault(d => d.DocumentType == (short)Document.DocumentTypes.CreditRequestFormSigned && d.CreditRequestId == creditRequestId);
            if (document != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = document.FileName,
                    Filter = "PDF Files (*.pdf)|*.pdf" // Set the filter to save as PDF
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        fileStream.Write(document.FileContent, 0, document.FileContent.Length);
                    }

                    MessageBox.Show("Documento descargado con éxito.");
                }
            }
            else
            {
                MessageBox.Show("No se encontró el documento solicitado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnDownloadCreditOpeningForm_Click(object sender, RoutedEventArgs e)
        {
            var document = _context.Documents.FirstOrDefault(d => d.DocumentType == (short)Document.DocumentTypes.CreditContractCoverSheetSigned && d.CreditRequestId == creditRequestId);
            if (document != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = document.FileName,
                    Filter = "PDF Files (*.pdf)|*.pdf" // Set the filter to save as PDF
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        fileStream.Write(document.FileContent, 0, document.FileContent.Length);
                    }

                    MessageBox.Show("Documento descargado con éxito.");
                }
            }
            else
            {
                MessageBox.Show("No se encontró el documento solicitado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDownloadCreditDomicileForm_Click(object sender, RoutedEventArgs e)
        {
            var document = _context.Documents.FirstOrDefault(d => d.DocumentType == (short)Document.DocumentTypes.DirectDebitAuthorizationSigned && d.CreditRequestId == creditRequestId);
            if (document != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = document.FileName,
                    Filter = "PDF Files (*.pdf)|*.pdf" // Set the filter to save as PDF
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        fileStream.Write(document.FileContent, 0, document.FileContent.Length);
                    }

                    MessageBox.Show("Documento descargado con éxito.");
                }
            }
            else
            {
                MessageBox.Show("No se encontró el documento solicitado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDownloadKit_Click(object sender, RoutedEventArgs e)
        {
            //call all documents download method
            btnDownloadCreditDomicileForm_Click(sender, e);
            btnDownloadCreditOpeningForm_Click(sender, e);
            btnDownloadCreditRequestDocument_Click(sender, e);
        }


        //metodos para moverse entre ventanas
        private void BtnClicPersonalInformation(object sender, RoutedEventArgs e)
        {
            var personalInformationPage = new CreditApplicationDetailsPersonalInformation(creditRequestId);
            if (NavigationService != null)
            {
                NavigationService.Navigate(personalInformationPage);
            }
            else
            {
                ToastNotification notification = new ToastNotification("No se puede realizar la navegación en este momento. Por favor, inténtelo más tarde.", "Error");

            }
        }

        private void BtnClicWorkCenter(object sender, RoutedEventArgs e)
        {
            var workCenterPage = new CreditApplicationDetailsWorkCenter(creditRequestId);
            if (NavigationService != null)
            {
                NavigationService.Navigate(workCenterPage);
            }
            else
            {
                ToastNotification notification = new ToastNotification("No se puede realizar la navegación en este momento. Por favor, inténtelo más tarde.", "Error");

            }
        }

        private void BtnClicPersonalReferences(object sender, RoutedEventArgs e)
        {
            var personalReferences = new CreditApplicationDetailsPersonalReferences((int)creditRequestId);
            if (NavigationService != null)
            {
                NavigationService.Navigate(personalReferences);
            }
            else
            {
                ToastNotification notification = new ToastNotification("No se puede realizar la navegación en este momento. Por favor, inténtelo más tarde.", "Error");

            }
        }

        private void BtnClicBankAccounts(object sender, RoutedEventArgs e)
        {
            var bankAccounts = new CreditApplicationDetailsBankAccounts(creditRequestId);
            if (NavigationService != null)
            {
                NavigationService.Navigate(bankAccounts);
            }
            else
            {
                ToastNotification notification = new ToastNotification("No se puede realizar la navegación en este momento. Por favor, inténtelo más tarde.", "Error");

            }
        }

        private void BtnClicAproveRequest(object sender, RoutedEventArgs e)
        {
            var bankAccounts = new CreditApplicationDetailsApproveCreditApplication(creditRequestId);
            if (NavigationService != null)
            {
                NavigationService.Navigate(bankAccounts);
            }
            else
            {
                ToastNotification notification = new ToastNotification("No se puede realizar la navegación en este momento. Por favor, inténtelo más tarde.", "Error");

            }

        }

        private void btnRequest_Click(object sender, RoutedEventArgs e)
        {
            var requestPage = new CreditApplicationDetailsRequest((int)creditRequestId);
            if (NavigationService != null)
            {
                NavigationService.Navigate(requestPage);
            }
            else
            {
                ToastNotification notification = new ToastNotification("No se puede realizar la navegación en este momento. Por favor, inténtelo más tarde.", "Error");
            }
        }

        private void BtnClicContinue(object sender, RoutedEventArgs e)
        {
            var creditAprovePage = new CreditApplicationDetailsApproveCreditApplication(creditRequestId);

            if (NavigationService != null)
            {
                NavigationService.Navigate(creditAprovePage);
            }
            else
            {
                ToastNotification notification = new ToastNotification("No se puede realizar la navegación en este momento. Por favor, inténtelo más tarde.", "Error");
            }
        }
    }
}
