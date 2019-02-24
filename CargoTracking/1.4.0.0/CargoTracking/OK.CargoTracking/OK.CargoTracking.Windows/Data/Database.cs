using OK.CargoTracking.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace OK.CargoTracking.Windows.Data
{
    public static class Database
    {
        public static TempData TempData { get; set; }
        public static SavedData SavedData { get; set; }

        static Database()
        {
            TempData = new TempData();
            SavedData = DataSaver<SavedData>.Load("SavedData");
            if (SavedData == null)
                SavedData = new SavedData();
        }
        
        public static void Update()
        {
            DataSaver<SavedData>.Save("SavedData", SavedData);
        }

        public static void AddTracking(Tracking tracking)
        {
            SavedData.History.RemoveAll(x => x.Code.ToUpper(new CultureInfo("en-US")) == tracking.Code.ToUpper(new CultureInfo("en-US")) && x.Factory.Id == tracking.Factory.Id);
            SavedData.History.Insert(0, tracking);
            Update();
        }

        public static void RemoveTracking(Tracking tracking)
        {
            SavedData.History.RemoveAll(x => x.Code.ToUpper(new CultureInfo("en-US")) == tracking.Code.ToUpper(new CultureInfo("en-US")) && x.Factory.Id == tracking.Factory.Id);
            Update();
        }

        public static void AddMessage(Message message)
        {
            SavedData.Messages.RemoveAll(x => x.Title == message.Title && x.Content == message.Content && x.Type == message.Type);
            SavedData.Messages.Add(message);
            Update();
        }

        public static void RemoveMessage(Message message)
        {
            SavedData.Messages.RemoveAll(x => x.Title == message.Title && x.Content == message.Content && x.Type == message.Type);
            Update();
        }

        internal static void UpdateFactories(List<Factory> factories)
        {
            // Clear all images in Assets/factories
            SavedData.Factories = new List<Factory>();
            foreach (var factory in factories)
            {
                AddFactory(factory);
            }
        }

        internal static void AddFactory(Factory factory)
        {
            if (factory != null)
            {
                downloadImage(factory.IconSource, "factory" + factory.Id.ToString());
                factory.IconSource = null;
                SavedData.Factories.RemoveAll(x => x.Id == factory.Id);
                SavedData.Factories.Add(factory);
                Update();
            }
        }

        private async static void downloadImage(string url, string name)
        {
            var client = new WebClient();
            var stream = await client.OpenReadTaskAsync(new Uri(url, UriKind.Absolute));
            var bytes = ReadFully(stream);
            DataSaver<byte[]>.Remove(name);
            DataSaver<byte[]>.Save(name, bytes);
            Update();
        }
        
        public static Task<Stream> OpenReadTaskAsync(this WebClient client, Uri uri)
        {
            var tcs = new TaskCompletionSource<Stream>();

            OpenReadCompletedEventHandler openReadEventHandler = null;
            openReadEventHandler = (sender, args) =>
            {
                try
                {
                    tcs.SetResult(args.Result);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            };

            client.OpenReadCompleted += openReadEventHandler;
            client.OpenReadAsync(uri);

            return tcs.Task;
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        internal static void RemoveFactory(Factory factory)
        {
            DataSaver<byte[]>.Remove("factory" + factory.Id);
            SavedData.Factories.RemoveAll(x => x.Id == factory.Id);
            Update();
        }

        internal static void ClearHistory()
        {
            SavedData.History = new List<Tracking>();
            Update();
        }

        internal static ImageSource GetFactoryImage(Factory factory)
        {
            if (factory != null)
            {
                string name = "factory" + factory.Id;
                var bytes = DataSaver<byte[]>.Load(name);
                if (bytes == null)
                    return null;
                var imgSource = new BitmapImage();
                imgSource.SetSource(new MemoryStream(bytes));
                return imgSource;
            }
            return null;
        }
    }
}