using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.Storage.Streams;

namespace OK.CargoTracking.Windows.Data
{
    public static class DataSaver<T>
    {
        static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        static StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        public async static Task<bool> Save(string fileName, T data)
        {
            try
            {
                string output = JsonConvert.SerializeObject(data);
                StorageFile sampleFile = await localFolder.CreateFileAsync(fileName + ".txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(sampleFile, output);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async static Task<T> Load(string fileName)
        {
            try
            {
                StorageFile sampleFile = await localFolder.GetFileAsync(fileName + ".txt");
                string json = await FileIO.ReadTextAsync(sampleFile);
                T data = JsonConvert.DeserializeObject<T>(json);
                return data;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public async static Task<bool> Remove(string fileName)
        {
            try
            {
                StorageFile sampleFile = await localFolder.GetFileAsync(fileName + ".txt");
                await sampleFile.DeleteAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
