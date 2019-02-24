using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mobisis.ServiceReference;
using System.Reflection;

namespace Mobisis
{
    class Data
    {
        private static string _Version = null;
        public static string Version {
            get
            {
                if (_Version == null)
                    _Version = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Version.ToString();
                return _Version;
            }
        }

        public static string StudentID { get; set; }
        public static string Password { get; set; }

        private static bool _IsRemember = true;
        public static bool IsRemember
        {
            get
            {
                return _IsRemember;
            }
            set {
                _IsRemember = value;
            }
        }


        private static Baglanti _Connection = null;
        public static Baglanti Connection
        {
            get
            {
                if (StudentID == null && Password == null)
                    return null;
                return _Connection;
            }
            set
            {
                _Connection = value;
            }
        }
        public static List<YemekList> FoodList { get; set; }
    }
}
