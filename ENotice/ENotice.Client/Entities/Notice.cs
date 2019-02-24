using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENotice.Client.Entities
{
    public class Notice
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public int WriterId { get; set; }
        public int Type { get; set; }
        public int Priority { get; set; }
        public string FacultyIds { get; set; }
        public string DepartmentIds { get; set; }
        public string MonitorIds { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> ModifiedAt { get; set; }
        public bool IsApproved { get; set; }
    }
}
