using OK.PhoneBook.Controls;
using OK.PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace OK.PhoneBook.Views
{
    public partial class CreateRecordPanel : UserControl
    {
        People people, oldPeople;
        List<PhoneNumber> phoneNumbers = new List<PhoneNumber>();
        bool isEdit = false;

        public CreateRecordPanel(People p = null)
        {
            InitializeComponent();

            if (p == null)
                people = new People();
            else
            {
                txtTitle.Text = "Kayıt Düzenle";
                oldPeople = people = Database.SavedData.Records.Find(x => x.Id == p.Id);
                isEdit = true;
            }
            pnlPeople.DataContext = people;
            
            cbPhoneType.SelectionChanged += cbPhoneType_SelectionChanged;

            if (people.PhoneNumbers != null)
                phoneNumbers = people.PhoneNumbers;

            lsPhoneNumbers.ItemsSource = null;
            lsPhoneNumbers.ItemsSource = phoneNumbers;
        }

        private void btnSend_Clicked(object sender, EventArgs args)
        {
            if (MainWindow.IsAdmin)
            {
                Create();
                return;
            }
            var verifyPanel = new VerifyPanel((s, e) =>
            {
                Create();
            }, (s, e) =>
            {
                if (isEdit)
                    people = oldPeople;
                MainWindow.SetContent(this);
            });
            MainWindow.SetContent(verifyPanel);
        }

        private void Create()
        {
            people.CreatedBy = Database.SavedData.Settings.Username;
            people.PhoneNumbers = phoneNumbers;

            if (!isEdit)
                Database.SavedData.Records.Add(people);
            Database.Save();
            MainWindow.SetContent(new RecordsPanel());
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            if (isEdit)
                people = oldPeople;
            MainWindow.SetContent(new RecordsPanel());
        }
        
        private void cbPhoneType_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (cbPhoneType.SelectedItem == null)
            {
                cbPhoneType.Visibility = Visibility.Visible;
                return;
            }
            var value = (cbPhoneType.SelectedItem as ComboBoxItem).Content.ToString();
            if (value == "Diğer")
                cbPhoneType.Visibility = Visibility.Collapsed;
            else if (value == "Dahili" && txtPhoneNumber.Text.Length != 0)
                txtPhoneNumber.Text = txtPhoneNumber.Text.Replace(" ", "").Replace("-", "");
        }

        private void btnAddPhoneSend_Clicked(object sender, EventArgs e)
        {
            var number = new PhoneNumber();
            var value = cbPhoneType.SelectedItem == null ? "" : (cbPhoneType.SelectedItem as ComboBoxItem).Content.ToString();

            if (value == "Diğer")
                number.Type = txtOther.Text;
            else
                number.Type = value;

            number.Number = txtPhoneNumber.Text;
            phoneNumbers.Add(number);

            lsPhoneNumbers.ItemsSource = null;
            lsPhoneNumbers.ItemsSource = phoneNumbers;

            txtOther.Text = "";
            cbPhoneType.SelectedItem = null;
            txtPhoneNumber.Text = "";
        }

        private void btnAddPhoneCancel_Clicked(object sender, EventArgs e)
        {
            txtOther.Text = "";
            cbPhoneType.SelectedItem = null;
            txtPhoneNumber.Text = "";
        }

        private void btnRemovePhoneNumber_Clicked(object sender, EventArgs e)
        {
            var item = (sender as ImageButton).Tag as PhoneNumber;
            phoneNumbers.RemoveAll(x => x.Type == item.Type && x.Number == item.Number);

            lsPhoneNumbers.ItemsSource = null;
            lsPhoneNumbers.ItemsSource = phoneNumbers;
        }

        private void txtPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            var value = cbPhoneType.SelectedItem == null ? "" : (cbPhoneType.SelectedItem as ComboBoxItem).Content.ToString();
            if (value != "Dahili")
            {
                var textBox = sender as TextBox;
                string text = textBox.Text;

                textBox.Text = getFormattedPhoneNumber(text);
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private string getFormattedPhoneNumber(string number)
        {
            if (number == null)
                return string.Empty;

            number = number.Replace(" ", "").Replace("-", "");
            if (number.Length > 0 && number[0] == '0')
                number = number.Substring(1, number.Length - 1);

            switch (number.Length)
            {
                case 7:
                    return Regex.Replace(number, @"(\d{3})(\d{2})(\d{2})", "$1-$2-$3");
                case 10:
                    return Regex.Replace(number, @"(\d{3})(\d{3})(\d{2})(\d{2})", "$1-$2-$3-$4");
                default:
                    return number;
            }
        }
    }
}
