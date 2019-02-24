using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ObisisService.Models
{
    [DataContract]
    public enum AbsentType
    {
        [EnumMember]
        Free = 0,
        [EnumMember]
        Passed = 1,
        [EnumMember]
        Failed = -1
    };
    [DataContract]
    public enum GradeType
    {
        [EnumMember]
        GT,
        [EnumMember]
        FF,
        [EnumMember]
        FD,
        [EnumMember]
        DD,
        [EnumMember]
        DC,
        [EnumMember]
        CC,
        [EnumMember]
        CB,
        [EnumMember]
        BB,
        [EnumMember]
        BA,
        [EnumMember]
        AA
    };
    [DataContract]
    public class Lesson : ObjectModel
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public double Credit { get; set; }
        [DataMember]
        public Exam FirstMidterm { get; set; }
        [DataMember]
        public Exam SecondMidterm { get; set; }
        [DataMember]
        public Exam ThirdMidterm { get; set; }
        [DataMember]
        public Exam Final { get; set; }
        [DataMember]
        public Exam Integration { get; set; }
        [DataMember]
        public bool? IntegrationRight { get; set; }
        [DataMember]
        public int? Average { get; set; }
        [DataMember]
        public bool AverageEffect { get; set; }
        [DataMember]
        public double SuccessFactor { get; set; }
        [DataMember]
        public int Class { get; set; }
        [DataMember]
        public GradeType? Grade { get; set; }
        [DataMember]
        public bool? State { get; set; }
        [DataMember]
        public AbsentType? AbsentState { get; set; }

        public static GradeType? GetGrade(string grade)
        {
            switch (grade)
            {
                case "FF":
                    return GradeType.FF;
                case "FD":
                    return GradeType.FD;
                case "DD":
                    return GradeType.DD;
                case "DC":
                    return GradeType.DC;
                case "CC":
                    return GradeType.CC;
                case "CB":
                    return GradeType.CB;
                case "BB":
                    return GradeType.BB;
                case "BA":
                    return GradeType.BA;
                case "AA":
                    return GradeType.AA;
                default:
                    return (GradeType?)null;
            }
        }
    }
}
