using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ObisisService.Models
{
    [DataContract]
    [KnownType(typeof(Activity))]
    [KnownType(typeof(Document))]
    [KnownType(typeof(Exam))]
    [KnownType(typeof(Food))]
    [KnownType(typeof(FoodList))]
    [KnownType(typeof(Graduation))]
    [KnownType(typeof(Lesson))]
    [KnownType(typeof(Period))]
    [KnownType(typeof(Program))]
    [KnownType(typeof(Student))]
    [KnownType(typeof(AbsentType))]
    [KnownType(typeof(ProgramType))]
    [KnownType(typeof(GradeType))]
    [KnownType(typeof(ListModel))]
    public class ObjectModel
    {
    }
}
