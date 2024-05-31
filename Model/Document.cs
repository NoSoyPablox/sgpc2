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
                default:
                    return "Tipo de documento desconocido";
            }
        }
    }
}
