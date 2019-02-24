using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iTunes.Models
{
    [DataContract(IsReference = true)]
    public class Artist
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Image { get; set; }
        [DataMember]
        public Genre Genre { get; set; }
        [DataMember]
        public List<Album> Albums { get; set; }
        [DataMember]
        public List<Song> Songs { get; set; }
        public Artist()
        {
            this.Albums = new List<Album>();
            this.Songs = new List<Song>();
        }
    }
}
