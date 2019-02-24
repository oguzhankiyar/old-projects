using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LibraryProject.Classes
{
    class StudentBook : BaseObject
    {
        public Student Student { get; set; }
        public Book Book { get; set; }
        public DateTime TakeDate { get; set; }
        public Nullable<DateTime> GiveDate { get; set; }

        static int CurrentID = 0;

        public StudentBook()
        {
            this.ID = ++CurrentID;
        }
    }
}