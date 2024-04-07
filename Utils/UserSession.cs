using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SGSC.Utils
{
    public class UserSession
    {
        public enum UserRole : short
        {
            Admin = 0,
            CreditAdvisor,
            CreditAnalyst,
        }

        private static UserSession _instance;
        public static UserSession Instance
        {
            get
            {
                return _instance;
            }
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string FirstSurname { get; set; }
        public string SecondSurname { get; set; }
        public short Role { get; set; }
        public string FullName
        {
            get
            {
                return $"{Name} {FirstSurname} {SecondSurname}";
            }
        }

        private UserSession(int id, string email, string name, string firstSurname, string secondSurname, short role)
        {
            Id = id;
            Email = email;
            Name = name;
            FirstSurname = firstSurname;
            SecondSurname = secondSurname;
            Role = role;
        }

        public static void LogIn(int id, string email, string name, string firstSurname, string secondSurname, short role)
        {
            _instance = new UserSession(id, email, name, firstSurname, secondSurname, role);

            var mainFrame = App.Current.MainFrame;
            switch(role)
            {
                case (short)UserRole.CreditAdvisor:
                {
                    var landingPage = new Pages.HomePageCreditAdvisor();
                    mainFrame.Content = landingPage;
                    break;
                }

                default:
                    MessageBox.Show("Rol no implementado.");
                    break;
            }
        }

        public static void LogOut()
        {
            var mainFrame = App.Current.MainFrame;
            var logInPage = new Pages.LogIn();
            mainFrame.Content = logInPage;
            _instance = null;
        }
    }
}
