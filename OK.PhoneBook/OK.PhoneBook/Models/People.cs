using System;
using System.Collections.Generic;

namespace OK.PhoneBook.Models
{
    public class People
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Job { get; set; }
        public string Institution { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }
        public string CreatedBy { get; set; }

        public People()
        {
            this.Id = Guid.NewGuid();
            this.PhoneNumbers = new List<PhoneNumber>();
        }
    }
}
