using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mobisis.ObisisServiceReference;

namespace Mobisis.Models
{
    public class LessonPlanItem
    {
        public Lesson Lesson { get; set; }
        public DayOfWeek Day { get; set; }
        public DateTime BeginDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
