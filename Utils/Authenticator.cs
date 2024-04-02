using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SGSC.Utils
{
    internal class Authenticator
    {
        public enum AuthResult
        {
            Success = 0,
            InvalidCredentials,
            DatabaseError
        }

        public static string HashPassword(string password)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                stringBuilder.Append(hashBytes[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        public static AuthResult AuthUser(string email, string password)
        {
            sgscEntities context = new sgscEntities();
            var user = context.Employees.Where(c => c.Email == email).FirstOrDefault();
            if (user == null)
            {
                return AuthResult.InvalidCredentials;
            }
            if (user.Password != HashPassword(password))
            {
                return AuthResult.InvalidCredentials;
            }
            return AuthResult.Success;
        }
    }
}
