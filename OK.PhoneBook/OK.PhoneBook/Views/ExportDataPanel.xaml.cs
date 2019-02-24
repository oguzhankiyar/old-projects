using Microsoft.Win32;
using Newtonsoft.Json;
using OK.PhoneBook.Models;
using System;
using System.IO;
using System.Windows.Controls;

namespace OK.PhoneBook.Views
{
    public partial class ExportDataPanel : UserControl
    {
        public ExportDataPanel()
        {
            InitializeComponent();
        }

        private void btnExportData_Clicked(object sender, EventArgs args)
        {
            if (MainWindow.IsAdmin)
            {
                Export();
                return;
            }
            var verifyPanel = new VerifyPanel((s, e) =>
            {
                Export();
            }, (s, e) =>
            {
                MainWindow.SetContent(this);
            });
            MainWindow.SetContent(verifyPanel);
        }

        private void Export()
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "JSON Files|*.json";
            dialog.ShowDialog();
            string fileName = dialog.FileName;
            if (string.IsNullOrEmpty(fileName))
                return;

            var data = new SavedData();
            if (cbRecords.IsChecked == true)
                data.Records = Database.SavedData.Records;
            if (cbAppointments.IsChecked == true)
                data.Appointments = Database.SavedData.Appointments;
            if (cbFavorites.IsChecked == true)
                data.Favorites = Database.SavedData.Favorites;
            if (cbSettings.IsChecked == true)
                data.Settings = Database.SavedData.Settings;

            File.WriteAllText(fileName, JsonConvert.SerializeObject(data));
            
            var newPanel = new ExportDataPanel();
            newPanel.txtMessage.Text = "Veriler başarıyla kaydedildi:\n" + fileName;
            MainWindow.SetContent(newPanel);
        }
    }
}
