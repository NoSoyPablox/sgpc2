using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using SGSC; // Ensure correct reference to SGSC.Model
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
using Microsoft.Win32;
using SGSC.Utils;

namespace SGSC.Pages
{
    /// <summary>
    /// Interaction logic for DocumentsManagerPage.xaml
    /// </summary>
    public partial class DocumentsManagerPage : Page
    {
        private sgscEntities _context;
        private ObservableCollection<Document> documentsDataAux;
        private int? creditRequestId;

        public DocumentsManagerPage(int? creditRequestId)
        {
            InitializeComponent();
            _context = new sgscEntities();
            this.creditRequestId = creditRequestId;
            LoadDocuments(creditRequestId);
        }

        private void LoadDocuments(int? creditRequestId)
        {
            try
            {
                using (sgscEntities db = new sgscEntities())
                {
                    var documents = db.Documents.ToList();
                    documentsDataAux = new ObservableCollection<Document>(documents);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al intentar obtener los documentos. Por favor, inténtelo de nuevo más tarde.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UploadDocument(Document.DocumentTypes documentType)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                byte[] fileContent = File.ReadAllBytes(openFileDialog.FileName);

                try
                {
                    Document document = new Document
                    {
                        FileName = fileName,
                        FileContent = fileContent,
                        CreditRequestId = creditRequestId.Value,
                        DocumentType = (short)documentType
                    };

                    _context.Documents.Add(document);
                    _context.SaveChanges();

                    documentsDataAux.Add(document);

                    MessageBox.Show("Documento subido con éxito.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error al intentar subir el documento. Por favor, inténtelo de nuevo más tarde.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DownloadDocument(Document.DocumentTypes documentType)
        {
            var document = _context.Documents.FirstOrDefault(d => d.DocumentType == (short)documentType && d.CreditRequestId == creditRequestId);
            if (document != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = document.FileName
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllBytes(saveFileDialog.FileName, document.FileContent);
                    MessageBox.Show("Documento descargado con éxito.");
                }
            }
            else
            {
                MessageBox.Show("No se encontró el documento solicitado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GenerateDocument(Document.DocumentTypes documentType)
        {
            // Logic to generate document using Crystal Reports
            // Example:
            // CrystalReportGenerator.Generate(documentType, creditRequestId);
        }

        private void DownloadDocumentKit_Click(object sender, RoutedEventArgs e)
        {
            // Logic to download the document kit (a collection of specified documents)
            // Example:
            // DocumentKitDownloader.Download(creditRequestId);
        }

        private void UploadINE_Click(object sender, RoutedEventArgs e) => UploadDocument(Document.DocumentTypes.NationalId);
        private void DownloadINE_Click(object sender, RoutedEventArgs e) => DownloadDocument(Document.DocumentTypes.NationalId);

        private void UploadDomicile_Click(object sender, RoutedEventArgs e) => UploadDocument(Document.DocumentTypes.Domicile);
        private void DownloadDomicile_Click(object sender, RoutedEventArgs e) => DownloadDocument(Document.DocumentTypes.Domicile);

        private void GenerateCreditRequestForm_Click(object sender, RoutedEventArgs e) => GenerateDocument(Document.DocumentTypes.CreditRequestForm);
        private void UploadSignedCreditRequestForm_Click(object sender, RoutedEventArgs e) => UploadDocument(Document.DocumentTypes.CreditRequestFormSigned);
        private void DownloadSignedCreditRequestForm_Click(object sender, RoutedEventArgs e) => DownloadDocument(Document.DocumentTypes.CreditRequestFormSigned);

        private void GenerateCreditContractCoverSheet_Click(object sender, RoutedEventArgs e) => GenerateDocument(Document.DocumentTypes.CreditContractCoverSheet);
        private void UploadSignedCreditContractCoverSheet_Click(object sender, RoutedEventArgs e) => UploadDocument(Document.DocumentTypes.CreditContractCoverSheetSigned);
        private void DownloadSignedCreditContractCoverSheet_Click(object sender, RoutedEventArgs e) => DownloadDocument(Document.DocumentTypes.CreditContractCoverSheetSigned);

        private void GenerateDirectDebitAuthorization_Click(object sender, RoutedEventArgs e) => GenerateDocument(Document.DocumentTypes.DirectDebitAuthorization);
        private void UploadSignedDirectDebitAuthorization_Click(object sender, RoutedEventArgs e) => UploadDocument(Document.DocumentTypes.DirectDebitAuthorizationSigned);
        private void DownloadSignedDirectDebitAuthorization_Click(object sender, RoutedEventArgs e) => DownloadDocument(Document.DocumentTypes.DirectDebitAuthorizationSigned);

        private void LogoutButton_Click(object sender, RoutedEventArgs e) => UserSession.LogOut();

        private void HomePageCreditAdvisorMenu(object sender, RoutedEventArgs e) => NavigationService.Navigate(new HomePageCreditAdvisor());
    }
}
