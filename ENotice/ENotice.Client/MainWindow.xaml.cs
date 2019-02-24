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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ENotice.Client.Requests;
using WebSocket4Net;

namespace ENotice.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebSocket ws;

        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
            this.Loaded += MainWindow_Loaded;
            GetBolumName();
        }

        private void GetBolumName()
        {
            MonitorRequest.OnDataLoaded = (monitor) =>
            {
                this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    MonitorNameText.Text = monitor.Name;
                }));
            };
            MonitorRequest.GetMonitor(Settings.MonitorId);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshNotices();
            string host = "ws://" + Settings.HostUrl + ":" + Settings.HostPort + "/api/socketapi?monitorId=" + Settings.MonitorId;

            this.ws = new WebSocket(host);
            this.ws.Closed += new EventHandler(ws_Closed);
            this.ws.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(ws_Error);
            this.ws.MessageReceived += new EventHandler<MessageReceivedEventArgs>(ws_MessageReceived);
            this.ws.Opened += new EventHandler(ws_Opened);
            this.ws.Open();

            ws.MessageReceived += new EventHandler<MessageReceivedEventArgs>(ws_MessageReceived);
        }

        private void ws_Opened(object sender, EventArgs e)
        {
        }

        private void ws_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                //MessageText.Text = e.Message;
                RefreshNotices();
            }));
        }

        private void ws_Closed(object sender, EventArgs e)
        {
        }

        private void ws_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
        }

        private void RefreshNotices()
        {
            //StatusText.Text = "güncelleniyor..";
            NoticeRequest.OnDataLoaded = (list) =>
            {
                this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    //StatusText.Text = "güncellendi: " + DateTime.Now.ToString();
                    DuyuruList.ItemsSource = list.ToList();
                }));
            };
            NoticeRequest.GetNotices(Settings.MonitorId);
        }
    }
}
