using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iTunes.Models
{
    [DataContract(IsReference = true)]
    public class Settings
    {
        [DataMember]
        public Country DefaultCountry { get; set; }
        [DataMember]
        public bool InstallationCompleted { get; set; }

        public Settings()
        {
            DefaultCountry = Database.Countries.First();
            InstallationCompleted = false;
        }
    }
}
