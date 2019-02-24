using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Classes
{
    class BaseObject
    {
        public int ID { get; set; }
        public BaseObject Next { get; set; }

    }
}
