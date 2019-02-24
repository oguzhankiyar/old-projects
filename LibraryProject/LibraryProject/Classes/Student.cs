using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LibraryProject.Classes
{
    class Student : BaseObject
    {
        public string Name { get; set; }
        public int bookCount { get; set; }

        public Student()
        {
            bookCount = 0;
        }
    }
}