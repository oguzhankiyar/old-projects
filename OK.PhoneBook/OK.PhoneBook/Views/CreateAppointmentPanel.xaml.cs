using OK.PhoneBook.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace OK.PhoneBook.Views
{
    public partial class CreateAppointmentPanel : UserControl
    {
        public CreateAppointmentPanel(DateTime? selectedDate = null)
        {
            InitializeComponent();
            
            datePicker.SelectedDate = selectedDate;
        }

        private void btnSave_Clicked(object sender, EventArgs args)
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
                MainWindow.SetContent(this);
            });
            MainWindow.SetContent(verifyPanel);
        }

        private void Create()
        {
            var appointment = new Appointment() { Date = datePicker.SelectedDate.Value };
            appointment.CreatedBy = Database.SavedData.Settings.Username;

            string beginHour = cbBeginHour.SelectedItem == null ? null : (cbBeginHour.SelectedItem as ComboBoxItem).Content.ToString();
            string endHour = cbEndHour.SelectedItem == null ? null : (cbEndHour.SelectedItem as ComboBoxItem).Content.ToString();
            string description = txtDescription.Text;
            if (string.IsNullOrEmpty(beginHour) || string.IsNullOrEmpty(endHour) || string.IsNullOrEmpty(description))
                MessageBox.Show("Boş bıraktığınız alan(lar) var!");
            else
            {
                appointment.Description = description;
                appointment.BeginHour = new TimeSpan(Convert.ToInt32(beginHour.Split(':')[0]), Convert.ToInt32(beginHour.Split(':')[1]), 0);
                appointment.EndHour = new TimeSpan(Convert.ToInt32(endHour.Split(':')[0]), Convert.ToInt32(endHour.Split(':')[1]), 0);
                Database.SavedData.Appointments.Add(appointment);
                Database.Save();

                MainWindow.SetContent(new AppointmentsPanel(datePicker.SelectedDate.Value));
            }
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            MainWindow.SetContent(new AppointmentsPanel());
        }
    }
}
