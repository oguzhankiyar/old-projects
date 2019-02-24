using System;
using System.Collections.Generic;
using System.Text;
using OK.Mobisis.Entities.ModelEntities;

namespace OK.Mobisis.Data
{
    public class TempData
    {
        public Student Student { get; set; }
        public Period Period { get; set; }
        public Lesson Lesson { get; set; }
        public List<FriendMark> MarkList { get; set; }
        public bool RememberMe { get; set; }

        public void Reset()
        {
            this.Student = null;
            this.Period = null;
            this.Lesson = null;
            this.MarkList = null;
        }

    }
}
