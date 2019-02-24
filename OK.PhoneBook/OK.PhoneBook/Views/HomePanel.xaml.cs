using System.Windows.Controls;

namespace OK.PhoneBook.Views
{
    public partial class HomePanel : UserControl
    {
        public HomePanel()
        {
            InitializeComponent();
            pnlAppointments.Content = new AppointmentsPanel();
            pnlFavorites.Content = new FavoritesPanel();
        }
    }
}
