using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ObisisService.Models
{
    [DataContract]
    class Student
    {
        [DataMember]
        public int StudentID { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public List<Program> Programs { get; set; }
        [DataMember]
        public List<Graduation> GraduationState { get; set; }
        [DataMember]
        public List<Document> Documents { get; set; }

    }
}
