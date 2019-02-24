using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iTunes.Models
{
    [DataContract(IsReference = true)]
    public class Country
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        public override bool Equals(object obj)
        {
            var other = obj as Country;
            if (other == null) return false;
            else return (this.Name == other.Name);
        }
    }
}
