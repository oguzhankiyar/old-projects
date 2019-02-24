using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OK.PhoneBook.Models
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan BeginHour { get; set; }
        public TimeSpan EndHour { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }

        public Appointment()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
