﻿using System;
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
            creditRequestDocumentForm.SetParameterValue("pMonto", creditRequestDocument.Amount);
            creditRequestDocumentForm.SetParameterValue("pPlazoSolicitado", creditRequestDocument);
            creditRequestDocumentForm.SetParameterValue("pInteres", creditRequestDocument.PromotionName);
            creditRequestDocumentForm.SetParameterValue("pProposito", creditRequestDocument.PromotionName);
            creditRequestDocumentForm.SetParameterValue("pClabe", creditRequestDocument.PromotionName);

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (.pdf)|.pdf",
                Title = "Guardar Documento de Solicitud de Crédito",
                FileName = "CreditRequestForm.pdf"
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

        }

        private void GenerateDocumentCreditOpeningForm(Document.DocumentTypes documentType)
        {

        }


        private void DownloadDocumentKit_Click(object sender, RoutedEventArgs e)
        {
            
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