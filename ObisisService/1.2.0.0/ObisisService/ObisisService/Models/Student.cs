using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ObisisService.Models
{
    [DataContract]
    public class Student : ObjectModel
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<Program> Programs { get; set; }
        [DataMember]
        public List<Graduation> GraduationState { get; set; }
        [DataMember]
        public List<Document> Documents { get; set; }

        public Student()
        {
            this.ID = 0;
            this.Password = null;
            this.Name = null;
            this.Programs = new List<Program>();
            this.GraduationState = new List<Graduation>();
            this.Documents = new List<Document>();
        }

    }
}
