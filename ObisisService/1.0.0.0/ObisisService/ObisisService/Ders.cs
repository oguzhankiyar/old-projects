using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ObisisService
{
    [DataContract]
    public class Ders
    {
        [DataMember]
        public string Kod { get; set; }
        [DataMember]
        public string Adi { get; set; }
        [DataMember]
        public int? Vize1 { get; set; }
        [DataMember]
        public int? Vize2 { get; set; }
        [DataMember]
        public int? Vize3 { get; set; }
        [DataMember]
        public int? Final { get; set; }
        [DataMember]
        public int? Butunleme { get; set; }
        [DataMember]
        public int? Ortalama { get; set; }
        [DataMember]
        public string HarfNotu { get; set; }
        [DataMember]
        public double BasariKatsayisi { get; set; }
        [DataMember]
        public string Durum { get; set; }
        [DataMember]
        public int Sinif { get; set; }
        [DataMember]
        public double Kredi { get; set; }
        [DataMember]
        public bool OrtalamayaEtki { get; set; }
    }
}