using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SGSC.Utils
{
    public static class Authenticator
    {
        public enum AuthResult
        {
            Success = 0,
            InvalidCredentials,
            DatabaseError,
            SessionAlreadyActive
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

        public static AuthResult AuthUser(string email, string password, bool setSession = false)
        {
            if(setSession && UserSession.Instance != null)
            {
                return AuthResult.SessionAlreadyActive;
            }
            try
            {
                SGSCEntities context = new SGSCEntities();
                var user = context.Employees.Where(c => c.Email == email).FirstOrDefault();
                if (user == null)
                {
                    return AuthResult.InvalidCredentials;
                }
                if (user.Password != HashPassword(password))
                {
                    return AuthResult.InvalidCredentials;
                }
                if(setSession)
                {
                    UserSession.LogIn(user.EmployeeId, user.Email, user.Name, user.FirstSurname, user.SecondSurname, user.Role);
                }
                return AuthResult.Success;
            }
            catch (Exception)
            {
                return AuthResult.DatabaseError;
            }
        }
    }
}
