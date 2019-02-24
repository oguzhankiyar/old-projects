using System;
using System.Collections.Generic;
using System.Text;

namespace iTunesMusic.Tools
{
    class DataSaver<T>
    {
        public static bool LoadData(string file, out T obj)
        {
            obj = default(T);
            return true;
        }

        public static bool SaveData(string file, T obj)
        {
            return true;
        }
    }
}
