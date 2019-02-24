using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OKPort.Models;

namespace OKPort.Functions
{
    public class Function
    {
        private static OKDbEntities db = new OKDbEntities();

        public static int? GetPageId(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;
            return db.Pages.SingleOrDefault(x => x.ShortURL == url).ID;
        }

        public static int? GetSectionId(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;
            return db.Sections.SingleOrDefault(x => x.ShortURL == url).ID;
        }

        public static int? GetCategoryId(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;
            return db.Categories.SingleOrDefault(x => x.ShortURL == url).ID;
        }

        public static int? GetPostId(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;
            return db.Posts.SingleOrDefault(x => x.ShortURL == url).ID;
        }
    }
}