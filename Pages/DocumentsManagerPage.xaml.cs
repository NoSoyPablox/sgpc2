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
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Runtime.InteropServices.ComTypes;

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
            btnDownloadKit.Visibility = Visibility.Hidden;
            btnDownloadKit.IsEnabled = false;
            if(UserSession.Instance.Role == 2)
            {
                btnDownloadKit.IsEnabled = true;
                btnDownloadKit.Visibility = Visibility.Visible;
            }
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
            //search if already exists
            var actualDocument = _context.Documents.FirstOrDefault(d => d.DocumentType == (short)documentType && d.CreditRequestId == creditRequestId);
            bool needToReplace = false;
            if (actualDocument != null)
            {
                MessageBoxResult result = MessageBox.Show("Ya existe un documento de este tipo. ¿Desea reemplazarlo?", "Documento existente", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    needToReplace = true;
                }else
                {
                    return;
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
                        CreditRequestId = creditRequestId.Value,
                        DocumentType = (short)documentType
                    };

                    if (needToReplace)
                    {
                        _context.Documents.Remove(actualDocument);
                        _context.SaveChanges();
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

        private void GenerateDocumentCreditRequest(Document.DocumentTypes documentType)
        {
            CreditRequestDocument creditRequestDocument = _context.CreditRequestDocuments.FirstOrDefault(crd => crd.CreditRequestId == creditRequestId);
            if (creditRequestDocument == null)
            {
                MessageBox.Show("No se encontró la información necesaria para generar el documento.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CreditRequestDocumentForm creditRequestDocumentForm = new CreditRequestDocumentForm();

            creditRequestDocumentForm.SetParameterValue("pVendedor", creditRequestDocument.EmployeeId);
            creditRequestDocumentForm.SetParameterValue("pFolio", creditRequestDocument.FileNumber);
            creditRequestDocumentForm.SetParameterValue("pPromocion", creditRequestDocument.PromotionName);
            creditRequestDocumentForm.SetParameterValue("pCreationDate", creditRequestDocument.CreationDateRequest);
            string formattedAmount = (creditRequestDocument.Amount ?? 0).ToString("C");
            creditRequestDocumentForm.SetParameterValue("pMonto", formattedAmount);
            creditRequestDocumentForm.SetParameterValue("pPlazoSolicitado", CreditRequestDocument.RequestPaymentIntervalToString((CreditRequestDocument.RequestPaymentInterval)creditRequestDocument.PaymentsInterval));
            creditRequestDocumentForm.SetParameterValue("pInteres", creditRequestDocument.InterestRate);
            creditRequestDocumentForm.SetParameterValue("pProposito", creditRequestDocument.Purpose);
            creditRequestDocumentForm.SetParameterValue("pClabe", creditRequestDocument.InterbankCode);
            creditRequestDocumentForm.SetParameterValue("pBankName", creditRequestDocument.BankName);
            creditRequestDocumentForm.SetParameterValue("pApellidoPaterno", creditRequestDocument.FirstSurname);
            creditRequestDocumentForm.SetParameterValue("pApellidoMaterno", creditRequestDocument.SecondSurname);
            creditRequestDocumentForm.SetParameterValue("pName", creditRequestDocument.CustomerName);
            creditRequestDocumentForm.SetParameterValue("pBirthDate", creditRequestDocument.BirthDate);
            creditRequestDocumentForm.SetParameterValue("pGenero", creditRequestDocument.Genre);
            creditRequestDocumentForm.SetParameterValue("pRFC", creditRequestDocument.Rfc);
            creditRequestDocumentForm.SetParameterValue("pCURP", creditRequestDocument.Curp);
            creditRequestDocumentForm.SetParameterValue("pColonia", creditRequestDocument.Colony);
            creditRequestDocumentForm.SetParameterValue("pCodigoPostal", creditRequestDocument.ZipCode);
            creditRequestDocumentForm.SetParameterValue("pEstado", creditRequestDocument.State);
            creditRequestDocumentForm.SetParameterValue("pCalle", creditRequestDocument.Street);
            creditRequestDocumentForm.SetParameterValue("pNumeroExterior", creditRequestDocument.ExternalNumber);
            creditRequestDocumentForm.SetParameterValue("pNumeroInterior", creditRequestDocument.InternalNumber);
            creditRequestDocumentForm.SetParameterValue("pTelefono1", creditRequestDocument.PhoneNumber1);
            creditRequestDocumentForm.SetParameterValue("pTelefono2", creditRequestDocument.PhoneNumber2);
            creditRequestDocumentForm.SetParameterValue("pCorreoElectronico", creditRequestDocument.Email);
            creditRequestDocumentForm.SetParameterValue("pEstadoCivil", CreditRequestDocument.RequestCivilStatusToString((CreditRequestDocument.CivilStatuses)creditRequestDocument.CivilStatus));
            creditRequestDocumentForm.SetParameterValue("pCentroTrabajoTelefono", creditRequestDocument.WorkCenterPhoneNumber);
            creditRequestDocumentForm.SetParameterValue("pCentroTrabajoColonia", creditRequestDocument.WorkCenterColony);
            creditRequestDocumentForm.SetParameterValue("pCentroTrabajoCalle", creditRequestDocument.WorkCenterStreet);
            creditRequestDocumentForm.SetParameterValue("pCentroTrabajoNombre", creditRequestDocument.CenterName);
            creditRequestDocumentForm.SetParameterValue("pCentroTrabajoCodigoPostal", creditRequestDocument.WorkCenterZipCode);
            creditRequestDocumentForm.SetParameterValue("pCentroTrabajoNumeroExterior", creditRequestDocument.WorkCenterOutsideNumber);

            if (creditRequestDocument.WorkCenterInnerNumber != null)
            {
                  creditRequestDocumentForm.SetParameterValue("pCentroTrabajoNumeroInterior", creditRequestDocument.WorkCenterInnerNumber);
            }
            else
            {
                creditRequestDocumentForm.SetParameterValue("pCentroTrabajoNumeroInterior", "");
            }
            

            
            var contacts = _context.Contacts.Where(c => c.CustomerId == creditRequestDocument.CustomerId).Take(2).ToList();

            if (contacts.Count > 0)
            {
                creditRequestDocumentForm.SetParameterValue("pNombreReferencia1", contacts[0].Name);
                creditRequestDocumentForm.SetParameterValue("pApellidoPaternoReferencia1", contacts[0].FirstSurname);
                creditRequestDocumentForm.SetParameterValue("pApellidoMaternoReferencia1", contacts[0].SecondSurname);
                creditRequestDocumentForm.SetParameterValue("pRelacionReferencia1", contacts[0].Relationship);
                creditRequestDocumentForm.SetParameterValue("pTelefonoReferencia1", contacts[0].PhoneNumber);
            }

            if (contacts.Count > 1)
            {
                creditRequestDocumentForm.SetParameterValue("pNombreReferencia2", contacts[1].Name);
                creditRequestDocumentForm.SetParameterValue("pApellidoPaternoReferencia2", contacts[1].FirstSurname);
                creditRequestDocumentForm.SetParameterValue("pApellidoMaternoReferencia2", contacts[1].SecondSurname);
                creditRequestDocumentForm.SetParameterValue("pRelacionReferencia2", contacts[1].Relationship);
                creditRequestDocumentForm.SetParameterValue("pTelefonoReferencia2", contacts[1].PhoneNumber);
            }



            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (.pdf)|.pdf",
                Title = "Guardar Documento de Solicitud de Crédito",
                FileName = $"Solicitud de Crédito_{creditRequestDocument.FileNumber}_Pendiente de Firma.pdf"
            };

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = saveFileDialog.FileName;
                try
                {
                    creditRequestDocumentForm.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);
                    MessageBox.Show("Documento generado con éxito.");
                }
                catch (ParameterFieldCurrentValueException ex)
                {
                    MessageBox.Show("Error: Faltan valores de parámetros. " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al generar el documento: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("La operación fue cancelada.", "Cancelado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void GenerateDocumentDomicilePaymentsAuth(Document.DocumentTypes documentType)
        {

            DomicilePaymentsAuth domicilePaymentsAuth = _context.DomicilePaymentsAuths.FirstOrDefault(crd => crd.CreditRequestId == creditRequestId);
            if (domicilePaymentsAuth == null)
            {
                MessageBox.Show("No se encontró la información necesaria para generar el documento.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DomicilePaymentsAuthForm domicilePaymentsAuthForm = new DomicilePaymentsAuthForm();

            domicilePaymentsAuthForm.SetParameterValue("pFolioAuth", domicilePaymentsAuth.FileNumber);
            domicilePaymentsAuthForm.SetParameterValue("pFechaHoy", DateTime.Today);
            domicilePaymentsAuthForm.SetParameterValue("pEmpresa", "FinancieraRunaterraCredit");
            domicilePaymentsAuthForm.SetParameterValue("pPeriocidadPago", CreditRequestDocument.RequestPaymentIntervalToString((CreditRequestDocument.RequestPaymentInterval)domicilePaymentsAuth.PaymentsInterval));
            string formattedAmount = (domicilePaymentsAuth.PaymentAmount ?? 0).ToString("C");
            domicilePaymentsAuthForm.SetParameterValue("pMontoCargo", formattedAmount);
            domicilePaymentsAuthForm.SetParameterValue("pPlazo", domicilePaymentsAuth.TimePeriod);
            domicilePaymentsAuthForm.SetParameterValue("pNombreBancoAuth", domicilePaymentsAuth.BankName);
            domicilePaymentsAuthForm.SetParameterValue("pTarjetaDebito", domicilePaymentsAuth.CardNumber);
            domicilePaymentsAuthForm.SetParameterValue("pCLABE", domicilePaymentsAuth.InterbankCode);
            domicilePaymentsAuthForm.SetParameterValue("pTelefonoAuth", domicilePaymentsAuth.PhoneNumber1);

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (.pdf)|.pdf",
                Title = "Autorización para Domiciliación de Pagos",
                FileName = $"AutorizaciónParaDomiciliacióndePagos_{domicilePaymentsAuth.FileNumber}_Pendiente de Firma.pdf"
            };

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = saveFileDialog.FileName;
                try
                {
                    domicilePaymentsAuthForm.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);
                    MessageBox.Show("Documento generado con éxito.");
                }
                catch (ParameterFieldCurrentValueException ex)
                {
                    MessageBox.Show("Error: Faltan valores de parámetros. " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al generar el documento: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("La operación fue cancelada.", "Cancelado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void GenerateDocumentCreditOpeningForm(Document.DocumentTypes documentType)
        {

        }


        private void DownloadDocumentKit_Click(object sender, RoutedEventArgs e)
        {
            DownloadSignedCreditRequestForm_Click(sender, e);
            DownloadSignedCreditContractCoverSheet_Click(sender, e);
            DownloadSignedDirectDebitAuthorization_Click(sender, e);
        }


        private void GenerateCreditRequestForm_Click(object sender, RoutedEventArgs e) => GenerateDocumentCreditRequest(Document.DocumentTypes.CreditRequestForm);
        private void UploadSignedCreditRequestForm_Click(object sender, RoutedEventArgs e) => UploadDocument(Document.DocumentTypes.CreditRequestFormSigned);
        private void DownloadSignedCreditRequestForm_Click(object sender, RoutedEventArgs e) => DownloadDocument(Document.DocumentTypes.CreditRequestFormSigned);

        private void GenerateCreditContractCoverSheet_Click(object sender, RoutedEventArgs e) => GenerateDocumentCreditOpeningForm(Document.DocumentTypes.CreditContractCoverSheet);
        private void UploadSignedCreditContractCoverSheet_Click(object sender, RoutedEventArgs e) => UploadDocument(Document.DocumentTypes.CreditContractCoverSheetSigned);
        private void DownloadSignedCreditContractCoverSheet_Click(object sender, RoutedEventArgs e) => DownloadDocument(Document.DocumentTypes.CreditContractCoverSheetSigned);

        private void GenerateDirectDebitAuthorization_Click(object sender, RoutedEventArgs e) => GenerateDocumentDomicilePaymentsAuth(Document.DocumentTypes.DirectDebitAuthorization);
        private void UploadSignedDirectDebitAuthorization_Click(object sender, RoutedEventArgs e) => UploadDocument(Document.DocumentTypes.DirectDebitAuthorizationSigned);
        private void DownloadSignedDirectDebitAuthorization_Click(object sender, RoutedEventArgs e) => DownloadDocument(Document.DocumentTypes.DirectDebitAuthorizationSigned);

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            //return to homepage
            //Aqui se tendria que validar que en sistema ya esten subidos los documentos
            MessageBox.Show("Documentos subidos con éxito.");
            //get the credit request
            var creditRequest = _context.CreditRequests.FirstOrDefault(cr => cr.CreditRequestId == creditRequestId);
            creditRequest.Status = (short)CreditRequest.RequestStatus.Pending;
            _context.SaveChanges();
            App.Current.MainFrame.Content = new HomePageCreditAdvisor();
        }
    }
}