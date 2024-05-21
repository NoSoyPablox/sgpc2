using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGSC.Utils
{
    public class Utils
    {

        public static string NumberALetter(double? number)
        {
            if (number == 0)
                return "cero";

            string[] unidades = { "", "un", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve" };
            string[] especiales = { "diez", "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve" };
            string[] decenas = { "", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };
            string[] centenas = { "", "ciento", "doscientos", "trescientos", "cuatrocientos", "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos" };

            if (number < 10)
                return unidades[(int)number];
            if (number < 20)
                return especiales[(int)number - 10];
            if (number < 100)
                return decenas[(int)number / 10] + ((number % 10 != 0) ? " y " + NumberALetter(number % 10) : "");
            if (number < 1000)
                return centenas[(int)number / 100] + ((number % 100 != 0) ? " " + NumberALetter(number % 100) : "");
            if (number < 1000000)
                return NumberALetter(number / 1000) + " mil" + ((number % 1000 != 0) ? " " + NumberALetter(number % 1000) : "");
            if (number < 1000000000)
                return NumberALetter(number / 1000000) + " millones" + ((number % 1000000 != 0) ? " " + NumberALetter(number % 1000000) : "");
            if (number < 1000000000000)
                return NumberALetter(number / 1000000000) + " mil millones" + ((number % 1000000000 != 0) ? " " + NumberALetter(number % 1000000000) : "");

            return "";
        }

    }
}
