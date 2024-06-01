using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGSC.Model
{
    public partial class Document
    {
        public enum DocumentTypes : short
        {
            NationalId,
            Domicile,
            CreditRequestForm,
            CreditRequestFormSigned,
            CollectionLayoutSender,
            CollectionLayoutReceiver,
            CreditContractCoverSheet,
            CreditContractCoverSheetSigned,
            DirectDebitAuthorization,
            DirectDebitAuthorizationSigned,
        }

        public static string RequestDocumentTypeToString(DocumentTypes documentType)
        {
            switch (documentType)
            {
                case DocumentTypes.NationalId:
                    return "Cédula de identidad";
                case DocumentTypes.Domicile:
                    return "Comprobante de domicilio";
                case DocumentTypes.CreditRequestForm:
                    return "Solicitud de crédito";
                case DocumentTypes.CreditRequestFormSigned:
                    return "Solicitud de crédito firmada";
                case DocumentTypes.CollectionLayoutSender:
                    return "Layout de cobranza (enviado)";
                case DocumentTypes.CollectionLayoutReceiver:
                    return "Layout de cobranza (recibido)";
                case DocumentTypes.CreditContractCoverSheet:
                    return "Caratula de contrato de crédito";
                case DocumentTypes.CreditContractCoverSheetSigned:
                    return "Caratula de contrato de crédito firmada";
                case DocumentTypes.DirectDebitAuthorization:
                    return "Autorizacion para domiciliazion de pagos";
                case DocumentTypes.DirectDebitAuthorizationSigned:
                    return "Autorizacion para domiciliazion de pagos firmada";
                default:
                    return "Tipo de documento desconocido";
            }
        }
    }
}
