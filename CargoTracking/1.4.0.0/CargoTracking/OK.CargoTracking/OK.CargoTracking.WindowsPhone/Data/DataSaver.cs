using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;

namespace OK.CargoTracking.WindowsPhone.Data
{
    public static class DataSaver<T>
    {
        private const string TargetFolderName = "CargoTrackingData";
        private static IsolatedStorageFile _isoFile;
        private static IsolatedStorageFile IsoFile
        {
            get
            {
                if (_isoFile == null)
                    _isoFile = IsolatedStorageFile.GetUserStoreForApplication();
                return _isoFile;
            }
        }

        public static void Save(string fileName, T data)
        {
            string TargetFileName = String.Format("{0}/{1}.json", TargetFolderName, fileName);
            if (!IsoFile.DirectoryExists(TargetFolderName))
                IsoFile.CreateDirectory(TargetFolderName);
            try
            {
                using (var targetFile = IsoFile.CreateFile(TargetFileName))
                {
                    string json = JsonConvert.SerializeObject(data);
                    byte[] array = Encoding.UTF8.GetBytes(json);
                    targetFile.Write(array, 0, array.Length);
                }
            }
            catch (Exception e)
            {
                string message = e.Message;
                IsoFile.DeleteFile(TargetFileName);
            }
        }

        public static T Load(string fileName)
        {
            try
            {
                T retVal = default(T);
                string TargetFileName = String.Format("{0}/{1}.json", TargetFolderName, fileName);
                if (IsoFile.FileExists(TargetFileName))
                    using (var sourceStream = IsoFile.OpenFile(TargetFileName, FileMode.Open))
                    {
                        string json = new StreamReader(sourceStream).ReadToEnd();
                        retVal = JsonConvert.DeserializeObject<T>(json);
                    }
                return retVal;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static void Remove(string fileName)
        {
            try
            {
                string TargetFileName = String.Format("{0}/{1}.json", TargetFolderName, fileName);
                if (IsoFile.FileExists(TargetFileName))
                    IsoFile.DeleteFile(TargetFileName);
            }
            catch (Exception)
            {
            }
        }
        
    }
}
