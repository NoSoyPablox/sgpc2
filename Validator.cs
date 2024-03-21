using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SGSC
{
    internal class Validator
    {
        public static bool ValidateCURP(string curp)
        {
            Regex regex = new Regex(@"^[A-Z]{4}[0-9]{6}[HM][A-Z]{2}[A-Z]{3}[0-9]{2}$");
            return regex.IsMatch(curp);
        }

        public static bool ValidateNames(string text)
        {
            return !Regex.IsMatch(text, @"[^a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$");
        }
    }

}
