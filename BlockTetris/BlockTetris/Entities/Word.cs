using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlockTetris.Entities
{
    [DataContract]
    public class Word
    {
        [DataMember]
        public List<char> Chars { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public bool IsFinished { get; set; }
    }
}
