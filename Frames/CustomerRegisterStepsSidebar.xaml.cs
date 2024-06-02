﻿using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SGSC.Frames
{
    /// <summary>
    /// Interaction logic for CustomerRegisterSidebar.xaml
    /// </summary>
    public partial class CustomerRegisterStepsSidebar : Page
    {
        public CustomerRegisterStepsSidebar(string currentStep)
        {
            InitializeComponent();
            this.SetActive(currentStep);
        }

        public void SetActive(string active)
        {
            PersonalInfoActive.Visibility = Visibility.Hidden;
            AddressActive.Visibility = Visibility.Hidden;
            BankAccountsActive.Visibility = Visibility.Hidden;
            WorkCenterActive.Visibility = Visibility.Hidden;
            ContactInfoActive.Visibility = Visibility.Hidden;
            ReferencesActive.Visibility = Visibility.Hidden;

            switch (active)
            {
                case "PersonalInfo":
                    AddressActive.Visibility = Visibility.Visible;
                    break;
                case "Address":
                    BankAccountsActive.Visibility = Visibility.Visible;
                    break;
                case "WorkCenter":
                    WorkCenterActive.Visibility = Visibility.Visible;
                    break;
                case "ContactInfo":
                    ContactInfoActive.Visibility = Visibility.Visible;
                    break;
                case "References":
                    ReferencesActive.Visibility = Visibility.Visible;
                    break;
                case "BankAccounts":
                    PersonalInfoActive.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
