using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using HtmlAgilityPack;

namespace ObisisService.Models
{
    [DataContract]
    public enum ProgramType
    { 
        [EnumMember]
        Major = 0,
        [EnumMember]
        Minor = 1
    };
    [DataContract]
    public class Program : ObjectModel
    {
        [DataMember]
        public ProgramType Type { get; set; }
        [DataMember]
        public string Faculty { get; set; }
        [DataMember]
        public string Department { get; set; }
        [DataMember]
        public int Class { get; set; }
        [DataMember]
        public double GANO { get; set; }
        [DataMember]
        public List<Period> Periods { get; set; }

        public static List<Program> GetPrograms(string html)
        {
            List<Program> programs = new List<Program>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode node = doc.DocumentNode;
            Program program = new Program();
            program.Type = ProgramType.Major;
            program.Faculty = Function.TidyText(node.SelectSingleNode("//span[@id='Banner1_Kullanici1_txtFakulteAdi']").InnerText);
            program.Department = Function.TidyText(node.SelectSingleNode("//span[@id='Banner1_Kullanici1_txtBolumAdi']").InnerText);
            program.Class = Convert.ToInt32(node.SelectSingleNode("//span[@id='Banner1_Kullanici1_txtSinifSeneGano']").InnerText.Replace("OKUYOR(", "").Split('.')[0]);
            program.GANO = Convert.ToDouble(node.SelectSingleNode("//span[@id='Banner1_Kullanici1_txtSinifSeneGano']").InnerText.Split(':')[1].Trim().Replace(",", "."));
            
            program.Periods = Period.GetPeriods(Function.GetHTML(Data.LessonStateURL, Data.StudentID, Data.Password));

            programs.Add(program);
            return programs;
        }
    }
}
