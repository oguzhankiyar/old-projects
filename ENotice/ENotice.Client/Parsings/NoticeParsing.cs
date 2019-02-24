using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENotice.Client.Entities;
using Newtonsoft.Json.Linq;

namespace ENotice.Client.Parsings
{
    public class NoticeParsing
    {
        public static List<Notice> ParseNotices(string json)
        {
            var notices = new List<Notice>();
            var array = JArray.Parse(json);
            foreach (var item in array)
            {
                var notice = new Notice();
                notice.Id = Convert.ToInt32(item["Id"].ToString());
                notice.Title = item["Title"].ToString();
                notice.Content = item["Content"].ToString();
                notice.ImageUrl = item["ImageUrl"].ToString();
                notice.Type = Convert.ToInt32(item["Type"].ToString());
                notice.CreatedAt = Convert.ToDateTime(item["CreatedAt"].ToString());
                if (item["ModifiedAt"] != null && !string.IsNullOrEmpty(item["ModifiedAt"].ToString()))
                    notice.ModifiedAt = Convert.ToDateTime(item["ModifiedAt"].ToString());
                notice.IsApproved = item["IsApproved"].ToString() == "true";
                notices.Add(notice);
            }
            return notices;
        }
    }
}
