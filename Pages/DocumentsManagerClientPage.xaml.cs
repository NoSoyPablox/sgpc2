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
using SGSC.Frames;

namespace SGSC.Pages
{
    /// <summary>
    /// Interaction logic for DocumentsManagerClientPage.xaml
    /// </summary>
    public partial class DocumentsManagerClientPage : Page
    {
        private sgscEntities _context;
        private ObservableCollection<Document> documentsDataAux;
        private int customerId = 0;
        private int creditRequestId = -1;

        public DocumentsManagerClientPage(int customerId, int creditRequestId)
        {
            InitializeComponent();
            _context = new sgscEntities();
            this.customerId = customerId;
            this.creditRequestId = creditRequestId;
            LoadDocuments(customerId);

            StepsSidebarFrame.Content = new CustomerRegisterStepsSidebar("Documents");
            UserSessionFrame.Content = new UserSessionFrame();
            this.creditRequestId = creditRequestId;
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
            //search if already exists
            var actualDocument = _context.Documents.FirstOrDefault(d => d.DocumentType == (short)documentType && d.CreditRequestId == customerId);
            bool needsToReplace = false;
            //if already exists replace it
            if (actualDocument != null)
            {
                MessageBoxResult result = MessageBox.Show("Ya existe un documento de este tipo. ¿Desea reemplazarlo?", "Documento existente", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                       needsToReplace = true;
                }
            }
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
                        CreditRequestId = customerId, //
                        DocumentType = (short)documentType
                    };

                    if(needsToReplace)
                    {
                        _context.Documents.Remove(actualDocument);
                    }
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
            var document = _context.Documents.FirstOrDefault(d => d.DocumentType == (short)documentType && d.CreditRequestId == customerId);
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

        private void UploadINE_Click(object sender, RoutedEventArgs e) => UploadDocument(Document.DocumentTypes.NationalId);
        private void DownloadINE_Click(object sender, RoutedEventArgs e) => DownloadDocument(Document.DocumentTypes.NationalId);

        private void UploadDomicile_Click(object sender, RoutedEventArgs e) => UploadDocument(Document.DocumentTypes.Domicile);
        private void DownloadDomicile_Click(object sender, RoutedEventArgs e) => DownloadDocument(Document.DocumentTypes.Domicile);

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            //go to workcenter page
            App.Current.MainFrame.Content = new PageWorkCenter(customerId, creditRequestId);
        }

        private void CancelRegister(object sender, RoutedEventArgs e)
        {
            var result = System.Windows.Forms.MessageBox.Show("Está seguro que desea cancelar el registro?\nSi decide cancelarlo puede retomarlo más tarde.", "Cancelar registro", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                App.Current.MainFrame.Content = new HomePageCreditAdvisor();
            }
        }
    }
}

