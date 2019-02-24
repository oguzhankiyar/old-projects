using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SocialObisisService
{
    public class Service1 : IService1
    {
        public Obisis.Ogrenci GetStudent(string ogrenciNo, string sifre)
        {
            return (new Obisis.Ogrenci(ogrenciNo, sifre));
        }
        public string GetStudentName(string ogrenciNo, string sifre)
        {
            return (new Obisis.Ogrenci(ogrenciNo, sifre).AdSoyad);
        }
        public Obisis.Duyuru GetNotices()
        {
            return (new Obisis.Duyuru());
        }
        public List<Person> GetPlayers()
        {
            List<Person> players = new List<Person>();
            players.Add(new Person { FirstName = "Peyton", LastName = "Manning", Age = 35 });
            players.Add(new Person { FirstName = "Drew", LastName = "Brees", Age = 31 });
            players.Add(new Person { FirstName = "Brett", LastName = "Favre", Age = 58 });

            return players;
        }
    }
}