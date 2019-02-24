using Microsoft.Win32;
using Newtonsoft.Json;
using OK.PhoneBook.Models;
using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace OK.PhoneBook.Views
{
    public partial class ImportDataPanel : UserControl
    {
        public ImportDataPanel()
        {
            InitializeComponent();
        }

        private void btnImportData_Clicked(object sender, EventArgs args)
        {
            if (MainWindow.IsAdmin)
            {
                Import();
                return;
            }
            var verifyPanel = new VerifyPanel((s, e) =>
            {
                Import();
            }, (s, e) =>
            {
                MainWindow.SetContent(this);
            });
            MainWindow.SetContent(verifyPanel);
        }

        private void Import()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "JSON Files|*.json";
            dialog.CheckFileExists = true;
            dialog.ShowDialog();
            string fileName = dialog.FileName;
            if (string.IsNullOrEmpty(fileName))
                return;

            string json = File.ReadAllText(fileName);
            var oldData = Database.SavedData;
            var data = JsonConvert.DeserializeObject<SavedData>(json);
            if (cbRemoveOldData.IsChecked != true)
            {
                if (cbRecords.IsChecked == true && data.Records != null)
                {
                    foreach (var item in data.Records)
                    {
                        if (!oldData.Records.Any(x => x.Id.ToString() == item.Id.ToString()))
                            oldData.Records.Add(item);
                    }
                    data.Records = oldData.Records;
                }
                if (cbFavorites.IsChecked == true && data.Favorites != null)
                {
                    foreach (var item in data.Favorites)
                    {
                        if (!oldData.Favorites.Any(x => x.ToString() == item.ToString()))
                            oldData.Favorites.Add(item);
                    }
                    data.Favorites = oldData.Favorites;
                }
                if (cbAppointments.IsChecked == true && data.Appointments != null)
                {
                    foreach (var item in data.Appointments)
                    {
                        if (!oldData.Appointments.Any(x => x.Id.ToString() == item.Id.ToString()))
                            oldData.Appointments.Add(item);
                    }
                    data.Appointments = oldData.Appointments;
                }
            }
            Database.SavedData = data;
            Database.Save();

            var newPanel = new ImportDataPanel();
            newPanel.txtMessage.Text = "Veriler kaydedildi:\n" + fileName;
            MainWindow.SetContent(newPanel);
        }
    }
}
