﻿using SGSC.Frames;
using SGSC.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SGSC.Pages
{
    /// <summary>
    /// Lógica de interacción para ReferenciasDelCliente.xaml
    /// </summary>
    public partial class CustomerReferencesPage : Page
    {
        private int customerId;
        private int? reference1Id;
        private int? reference2Id;
        private int creditRequestId = -1;
        public CustomerReferencesPage(int customerId, int creditRequestId)
        {
            InitializeComponent();

            this.customerId = customerId;

            StepsSidebarFrame.Content = new CustomerRegisterStepsSidebar("References");
            UserSessionFrame.Content = new UserSessionFrame();

            GetReferences();
            this.creditRequestId = creditRequestId;
        }

        private void GetReferences()
        {
            try
            {
                using (sgscEntities db = new sgscEntities())
                {
                    var customerContacts = db.Contacts.Where(ba => ba.CustomerId == customerId).ToArray();
                    if (customerContacts.Length > 0)
                    {
                        tbName.Text = customerContacts[0].Name;
                        tbFirstSurname.Text = customerContacts[0].FirstSurname;
                        tbSecondSurname.Text = customerContacts[0].SecondSurname;
                        tbPhoneNumber.Text = customerContacts[0].PhoneNumber;
                        reference1Id = customerContacts[0].ContactId;
                        //set the relationship
                        foreach (ComboBoxItem item in cbRelationship1.Items)
                        {
                            if (item.Content.ToString() == customerContacts[0].Relationship)
                            {
                                cbRelationship1.SelectedItem = item;
                                break;
                            }
                        }
                    }

                    if (customerContacts.Length > 1)
                    {
                        tbName1.Text = customerContacts[1].Name;
                        tbFirstSurname2.Text = customerContacts[1].FirstSurname;
                        tbSecondSurname2.Text = customerContacts[1].SecondSurname;
                        tbPhoneNumber2.Text = customerContacts[1].PhoneNumber;
                        reference2Id = customerContacts[1].ContactId;

                        //set the relationship
                        foreach (ComboBoxItem item in cbRelationship2.Items)
                        {
                            if (item.Content.ToString() == customerContacts[1].Relationship)
                            {
                                cbRelationship2.SelectedItem = item;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener las cuentas bancarias del cliente: " + ex.Message);
            }
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            List<string> names = new List<string> { tbName.Text, tbFirstSurname.Text, tbSecondSurname.Text, tbName1.Text, tbFirstSurname2.Text, tbSecondSurname2.Text };
            List<string> phoneNumbers = new List<string> { tbPhoneNumber.Text, tbPhoneNumber2.Text };

            if (!names.All(name => !string.IsNullOrEmpty(name)))
            {
                MessageBox.Show("Los campos no pueden estar vacios", "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!phoneNumbers.All(phoneNumber => TextValidator.ValidateTextNumeric(phoneNumber, 10, 10, false)))
            {
                MessageBox.Show("Los campos de teléfono no cumplen con el formato correcto", "Campos incorrectos", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (cbRelationship1.SelectedIndex == -1 || cbRelationship2.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar la relación de los contactos", "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //validate the fields
            if (Utils.TextValidator.ValidateMultipleNames(names))
            {
                using (sgscEntities db = new sgscEntities())
                {
                    try
                    {
                        Contact contact1 = new Contact();
                        contact1.Name = tbName.Text;
                        contact1.FirstSurname = tbFirstSurname.Text;
                        contact1.SecondSurname = tbSecondSurname.Text;
                        contact1.PhoneNumber = tbPhoneNumber.Text;
                        contact1.CustomerId = customerId;
                        ComboBoxItem item1 = (ComboBoxItem)cbRelationship1.SelectedItem;
                        contact1.Relationship = item1.Content.ToString();
                        if (reference1Id != null)
                        {
                            contact1.ContactId = reference1Id.Value;
                        }
                        db.Contacts.AddOrUpdate(contact1);

                        Contact contact2 = new Contact();
                        contact2.Name = tbName1.Text;
                        contact2.FirstSurname = tbFirstSurname2.Text;
                        contact2.SecondSurname = tbSecondSurname2.Text;
                        contact2.PhoneNumber = tbPhoneNumber2.Text;
                        contact2.CustomerId = customerId;
                        //get selected item 
                        ComboBoxItem item2 = (ComboBoxItem)cbRelationship2.SelectedItem;
                        //set the relationship of the contact to the content of the selected item
                        contact2.Relationship = item2.Content.ToString();
                        if (reference2Id != null)
                        {
                            contact2.ContactId = reference2Id.Value;
                        }
                        db.Contacts.AddOrUpdate(contact2);

                        db.SaveChanges();

                        MessageBox.Show("Referencias guardadas correctamente", "Referencias guardadas", MessageBoxButton.OK, MessageBoxImage.Information);

                        App.Current.MainFrame.Content = new CustomerBankAccounts(customerId, creditRequestId);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al registrar el cliente: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Los campos no cumplen con el formato correcto", "Campos incorrectos", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelRegister(object sender, RoutedEventArgs e)
        {
            var result = System.Windows.Forms.MessageBox.Show("Está seguro que desea cancelar el registro?\nSi decide cancelarlo puede retomarlo más tarde.", "Cancelar registro", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                App.Current.MainFrame.Content = new HomePageCreditAdvisor();
            }
        }

        private void tbName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbName.Text + e.Text, @"[^a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]+$"))
            {
                e.Handled = true;
            }
        }

        private void tbName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
