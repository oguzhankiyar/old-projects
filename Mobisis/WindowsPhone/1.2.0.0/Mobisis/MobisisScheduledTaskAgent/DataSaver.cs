using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;

namespace MobisisScheduledTaskAgent
{
    public class DataSaver<T>
    {
        private const string TargetFolderName = "MobisisData";
        private DataContractSerializer _mySerializer;
        private IsolatedStorageFile _isoFile;
        IsolatedStorageFile IsoFile
        {
            get
            {
                if (_isoFile == null)
                    _isoFile = IsolatedStorageFile.GetUserStoreForApplication();
                return _isoFile;
            }
        }

        public DataSaver()
        {
            _mySerializer = new DataContractSerializer(typeof(T));
        }

        public void SaveMyData(T sourceData, String targetFileName)
        {
            string TargetFileName = String.Format("{0}/{1}.dat", TargetFolderName, targetFileName);
            if (!IsoFile.DirectoryExists(TargetFolderName))
                IsoFile.CreateDirectory(TargetFolderName);
            try
            {
                using (var targetFile = IsoFile.CreateFile(TargetFileName))
                {
                    _mySerializer.WriteObject(targetFile, sourceData);
                }
            }
            catch (Exception)
            {
                IsoFile.DeleteFile(TargetFileName);
            }
        }

        public T LoadMyData(string sourceName)
        {
            try
            {
                T retVal = default(T);
                string TargetFileName = String.Format("{0}/{1}.dat", TargetFolderName, sourceName);
                if (IsoFile.FileExists(TargetFileName))
                    using (var sourceStream = IsoFile.OpenFile(TargetFileName, FileMode.Open))
                    {
                        retVal = (T)_mySerializer.ReadObject(sourceStream);
                    }
                return retVal;
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}