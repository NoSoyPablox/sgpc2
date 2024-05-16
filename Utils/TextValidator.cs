﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SGSC.Utils
{
    public static class TextValidator
    {
        public static bool ValidateText(string text, int maxLength, int minLength = 1, bool spacesAllowed = true)
        {
            if (text == null || text.Length < minLength || text.Length > maxLength)
            {
                return false;
            }
            if (!spacesAllowed)
            {
                if (text.Contains(' '))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ValidateTextAlphaNumeric(string text, int maxLength, int minLength = 1, bool spacesAllowed = true)
        {
            if (!ValidateText(text, maxLength, minLength, spacesAllowed))
            {
                return false;
            }
            if (!Regex.IsMatch(text, @"^[a-zA-Z0-9 ]+$"))
            {
                return false;
            }
            return true;
        }

        public static bool ValidateTextAlpha(string text, int maxLength, int minLength = 1, bool spacesAllowed = true)
        {
            if (!ValidateText(text, maxLength, minLength, spacesAllowed))
            {
                return false;
            }
            if (!Regex.IsMatch(text, @"^[a-zA-Z ]+$"))
            {
                return false;
            }
            return true;
        }

        public static bool ValidateTextNumeric(string text, int maxLength, int minLength = 1, bool spacesAllowed = true)
        {
            if (!ValidateText(text, maxLength, minLength, spacesAllowed))
            {
                return false;
            }
            if (!Regex.IsMatch(text, @"^[0-9]+$"))
            {
                return false;
            }
            return true;
        }

        public static bool ValidateEmail(string email)
        {
            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z0-9]+$"))
            {
                return false;
            }
            return true;
        }

        public static bool CheckPasswordStrength(string password)
        {
            if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*()_+.])"))
            {
                return false;
            }
            return true;
        }

        public static bool ValidateCURP(string curp)
        {
            if (string.IsNullOrEmpty(curp) || curp.Length != 18)
            {
                return false;
            }
            else
            {
                Regex regex = new Regex(@"^[A-Z]{4}[0-9]{6}[HM][A-Z]{2}[A-Z]{3}[0-9]{2}$");
                return regex.IsMatch(curp);
            }
        }

        public static bool ValidateNames(string text)
        {
            return !Regex.IsMatch(text, @"[^a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$");
        }

        public static bool ValidateMultipleNames(List<string> text)
        {
            return text.TrueForAll(ValidateNames);
        }

        public static bool ValidateCardNumber(string cardNumber)
        {
            return Regex.IsMatch(cardNumber, @"^[0-9]{16}$");
        }
    }
}
