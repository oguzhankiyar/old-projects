using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.Storage.Streams;

namespace OK.Mobisis.BackgroundTasks
{
    public sealed class DataSaver
    {
        static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        static StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        
        internal Action<string> ReadCompleted = null;
        internal Action<bool> SaveCompleted = null;

        public async void ReadAsync(string fileName)
        {
            var sampleFile = await localFolder.GetFileAsync(fileName + ".txt");
            string json = await FileIO.ReadTextAsync(sampleFile);
            if (ReadCompleted != null)
                ReadCompleted(json);
        }

        public async void SaveAsync(string fileName, string output)
        {
            try
            {
                var sampleFile = await localFolder.CreateFileAsync(fileName + ".txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(sampleFile, output);
                if (SaveCompleted != null)
                    SaveCompleted(true);
            }
            catch
            {
                if (SaveCompleted != null)
                    SaveCompleted(false);
            }
        }
    }
}
