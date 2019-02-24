using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace BlockTetris.Data
{

    public class DataSaver<T>
    {
        static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        static StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        public async static Task<bool> SaveData(string fileName, T data)
        {
            try
            {
                string output = DataSerializer<T>.Serialize(data);
                StorageFile sampleFile = await localFolder.CreateFileAsync(fileName + ".txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(sampleFile, output);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async static Task<T> LoadData(string fileName)
        {
            try
            {
                StorageFile sampleFile = await localFolder.GetFileAsync(fileName + ".txt");
                string xml = await FileIO.ReadTextAsync(sampleFile);
                T data = DataSerializer<T>.Deserialize(xml);
                return data;
            }
            catch (Exception)
            {
                return default(T);
            }
        }
        public async static Task<bool> RemoveData(string fileName)
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
