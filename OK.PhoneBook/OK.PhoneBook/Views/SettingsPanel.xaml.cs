using System.Windows;
using System.Windows.Controls;

namespace OK.PhoneBook.Views
{
    public partial class SettingsPanel : UserControl
    {
        public SettingsPanel()
        {
            InitializeComponent();
            pnlContent.Content = new AccountPanel();
        }

        private void btnGeneral_Click(object sender, RoutedEventArgs e)
        {
            btnGeneral.Opacity = 1;
            btnImport.Opacity = .5;
            btnExport.Opacity = .5;
            pnlContent.Content = new AccountPanel();
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            btnGeneral.Opacity = .5;
            btnImport.Opacity = 1;
            btnExport.Opacity = .5;
            pnlContent.Content = new ImportDataPanel();
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            btnGeneral.Opacity = .5;
            btnImport.Opacity = .5;
            btnExport.Opacity = 1;
            pnlContent.Content = new ExportDataPanel();
        }
    }
}
