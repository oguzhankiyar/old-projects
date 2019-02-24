using OK.PhoneBook.Controls;
using OK.PhoneBook.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace OK.PhoneBook.Views
{
    public partial class AppointmentsPanel : UserControl
    {
        public AppointmentsPanel(DateTime? selectedDate = null)
        {
            InitializeComponent();

            if (selectedDate == null)
                selectedDate = DateTime.Now;

            datePicker.SelectedDate = selectedDate;

            lsAppointments.SelectionChanged += (s, e) =>
            {
                if (lsAppointments.SelectedItem != null)
                lsAppointments.SelectedItem = null;
            };
        }
        
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            lsAppointments.ItemsSource = null;
            if (datePicker.SelectedDate != null)
            {
                var appointments = Database.SavedData.Appointments.Where(x => x.Date.Date == datePicker.SelectedDate.Value.Date).OrderBy(x => x.BeginHour);
                lsAppointments.ItemsSource = null;
                lsAppointments.ItemsSource = appointments;

                txtWarning.Visibility = appointments.Any() ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        
        private void btnCreateAppointment_Clicked(object sender, EventArgs e)
        {
            MainWindow.SetContent(new CreateAppointmentPanel(datePicker.SelectedDate));
        }

        private void btnRemove_Clicked(object sender, EventArgs args)
        {
            var item = (sender as ImageButton).Tag as Appointment;
            var verifyPanel = new VerifyPanel((s, e) =>
            {
                Database.SavedData.Appointments.RemoveAll(x => x.Id == item.Id);
                Database.Save();

                var appointments = Database.SavedData.Appointments.Where(x => x.Date.Date == datePicker.SelectedDate.Value.Date).OrderBy(x => x.BeginHour);

                lsAppointments.ItemsSource = null;
                lsAppointments.ItemsSource = appointments;

                txtWarning.Visibility = appointments.Any() ? Visibility.Collapsed : Visibility.Visible;

                MainWindow.SetContent(this);
            }, (s, e) =>
            {
                MainWindow.SetContent(this);
            });
            MainWindow.SetContent(verifyPanel);
        }
    }
}
