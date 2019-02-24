using System;
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
using System.Windows.Shapes;

namespace ENotice.Client
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();
            this.WindowStyle = WindowStyle.None;
            HostUrl.Text = "localhost";
            HostPort.Text = "19433";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Settings.HostUrl = HostUrl.Text;
            Settings.HostPort = HostPort.Text;
            Settings.MonitorId = Convert.ToInt32(MonitorId.Text);
            new MainWindow().Show();
            this.Close();
        }
    }
}
