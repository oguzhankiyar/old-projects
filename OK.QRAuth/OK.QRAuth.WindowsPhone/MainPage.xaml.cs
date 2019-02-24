using System;
using System.Net;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Text;
using System.IO;
using JeffWilcox.Controls;

namespace OK.QRAuth.WindowsPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        string code = "";
        QRCodeScanner scanner;
        public MainPage()
        {
            InitializeComponent();
            prepare();
            btnScan.Click += BtnScan_Click;
        }

        private void BtnScan_Click(object sender, RoutedEventArgs e)
        {
            prepare();
        }

        private void prepare()
        {
            scanner = new QRCodeScanner();
            scanner.Width = 400;
            scanner.Height = 400;
            scanner.ScanComplete += QRCodeScanner_ScanComplete;
            scanner.Error += QRCodeScanner_Error;

            pnlScanner.Children.Clear();
            pnlScanner.Children.Add(scanner);
        }

        private void QRCodeScanner_ScanComplete(object sender, JeffWilcox.Controls.ScanCompleteEventArgs e)
        {
            code = "{ \"Token\": \"" + e.Result + "\", \"AccessToken\": \"" + Guid.NewGuid().ToString().Replace("-", "") + "\" }";

            var request = HttpWebRequest.Create("http://auth.ogzkyr.net/api/auth");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.BeginGetRequestStream(GetRequestStream, request);
        }

        private void GetRequestStream(IAsyncResult ar)
        {
            var request = ar.AsyncState as HttpWebRequest;
            var stream = request.EndGetRequestStream(ar);
            var bytes = Encoding.UTF8.GetBytes(code);
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
            request.BeginGetResponse(GetResponse, request);
        }

        private void GetResponse(IAsyncResult ar)
        {
            var request = ar.AsyncState as HttpWebRequest;
            var response = request.EndGetResponse(ar);
            var reader = new StreamReader(response.GetResponseStream());

            Dispatcher.BeginInvoke(() => {
                MessageBox.Show(reader.ReadToEnd());
            });
        }

        private void QRCodeScanner_Error(object sender, JeffWilcox.Controls.ScanFailureEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }
    }
}