using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OKComplex.Models;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;

namespace OKComplex.Functions
{
    public class OK
    {
        public static string CreateConfirmationCode(string type)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            Random random = new Random();
            string code = type + "-";
            for (int i = 0; i < 10; i++)
            {
                int index = random.Next(0, chars.Length - 1);
                if (!code.Contains(chars.GetValue(index).ToString()))
                    code += chars.GetValue(index);
                else
                    i--;
            }
            return code;
        }
        public static void SendEmail(string email, string title, string content)
        {
            try
            {
                string emailfrom = "noreplay@gmail.com";
                content = "<html><head><meta content=\"text/html; charset=utf-8\" /></head><body>" + content + "</body></html>";
                OKDbEntities Db = new OKDbEntities();
                SmtpClient client = new SmtpClient(Config("smtp-server"), Convert.ToInt32(Config("smtp-port")));
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(Config("smtp-username"), Config("smtp-password"));
                MailMessage message = new MailMessage(emailfrom, email, title, content);
                message.IsBodyHtml = true;
                client.Send(message);
            }
            catch { }
        }
        public static int FindCommentPage(comment comment)
        {
            var comments = comment.topic.comments.Where(x => x.IsApproval == true).OrderBy(x => x.CreationDate);
            int take = Convert.ToInt32(OK.Config("comment-paging-count"));
            int count = 0;
            int page = 1;
            foreach (var item in comments)
            {
                count++;
                if (item == comment)
                {
                    page = count % take == 0 ? count / take : count / take + 1;
                    break;
                }
            }
            return page;
        }
        public static string Config(string key)
        {
            OKDbEntities Db = new OKDbEntities();
            var config = Db.config.SingleOrDefault(x => x.Key == key);
            if (config != null)
                return config.Value;
            else
                return null;
        }
        public static string ClubConfig(string key)
        {
            InfosecEntities Db = new InfosecEntities();
            var config = Db.club_config.SingleOrDefault(x => x.Key == key);
            if (config != null)
                return config.Value;
            else
                return null;
        }
        public static string ConvertSeo(string text)
        {
            text = text.ToLower();
            text = text.Replace(" ", "-");
            text = text.Replace("#", "-");
            text = text.Replace("$", "-dolar-");
            text = text.Replace("€", "-euro-");
            text = text.Replace("%", "-yuzde-");
            text = text.Replace("&", "-ve-");
            text = text.Replace("/", "-");
            text = text.Replace("!", "-");
            text = text.Replace("é", "-");
            text = text.Replace("'", "-");
            text = text.Replace("^", "-");
            text = text.Replace("+", "-");
            text = text.Replace("(", "-");
            text = text.Replace(")", "-");
            text = text.Replace("=", "-");
            text = text.Replace("?", "-");
            text = text.Replace("*", "-");
            text = text.Replace("_", "-");
            text = text.Replace(".", "");
            text = text.Replace("{", "-");
            text = text.Replace("}", "-");
            text = text.Replace("[", "-");
            text = text.Replace("]", "-");
            text = text.Replace("@", "-");
            text = text.Replace("~", "-");
            text = text.Replace("´", "-");
            text = text.Replace(",", "-");
            text = text.Replace("`", "-");
            text = text.Replace(":", "-");
            text = text.Replace(";", "-");
            text = text.Replace("ğ", "g");
            text = text.Replace("ü", "u");
            text = text.Replace("ş", "s");
            text = text.Replace("ı", "i");
            text = text.Replace("ö", "o");
            text = text.Replace("ç", "c");
            text = text.Replace("---", "-");
            text = text.Replace("--", "-");
            text = text.Trim('-');
            return text;
        }
        public static string UserName(user user)
        {
            return (user.DisplayType == 1 ? user.UserName : user.Name);
        }
        public static string UserState(int id, DateTime lastlogin)
        {
            OKDbEntities Db = new OKDbEntities();
            TimeSpan ts = DateTime.Now - lastlogin;
            return (ts.Minutes <= 5 ? "online" : "offline");
        }
        public static int UserRating(int rating)
        {
            OKDbEntities Db = new OKDbEntities();
            int totalpoint = 0;
            var items = Db.user.Where(x => x.Rating >= 500 && x.IsBanned == false);
            int average;
            if (items.Count() == 0)
            {
                var itemm = Db.user.Where(x => x.IsBanned == false);
                foreach (var item in itemm)
                {
                    totalpoint += Convert.ToInt32(item.Rating);
                }
                average = totalpoint / itemm.Count();
            }
            else
            {
                foreach (var item in items)
                {
                    totalpoint += Convert.ToInt32(item.Rating);
                }
                average = totalpoint / items.Count();
            }
            int point = rating * 3 / average;
            return point > 5 ? 5 : point;
        }
        public static int TopicPoint(string point, int type)
        {
            string[] rating = point.Split('/');
            int like = Convert.ToInt32(rating[0]);
            int dislike = Convert.ToInt32(rating[1]);
            int total = like + dislike;
            int result = 0;
            if (type == 1)
            {
                result = like;
            }
            else if (type == 2)
            {
                result = dislike;
            }
            else if (type == 3)
            {
                result = like + dislike == 0 ? 50 : 100 * like / (like + dislike);
            }
            else if (type == 4)
            {
                result = like + dislike == 0 ? 50 : 100 - (100 * like / (like + dislike));
            }
            else if (type == 5)
            {
                result = (like + dislike);
            }
            return result;
        }
        public static string Summary(string text, int character)
        {
            text = text.Replace("\r\n", " ");
            text = text.Replace("\n", " ");
            text = Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
            if (text.Length > character)
            {
                string[] split = text.Substring(0, character).Split(' ');
                text = text.Substring(0, character).TrimEnd(' ') + ".."; //.Replace(split[split.Count() - 1], "")
            }
            return text;
        }
        public static topic LastTopic(forum forum)
        {
            return (forum.topics.Where(x => x.IsApproval == true).OrderByDescending(x => x.ModifyDate).FirstOrDefault());
        }
        public static comment LastComment(topic topic)
        {
            return (topic.comments.Where(x => x.IsApproval == true).OrderByDescending(x => x.ModifyDate).FirstOrDefault());
        }
        public static void UpdateRating(int id, int score)
        {
            OKDbEntities Db = new OKDbEntities();
            var user = Db.user.Find(id);
            user.Rating += score;
            Db.SaveChanges();
        }
        public static int RatingScore(int id, string type, int userid)
        {
            OKDbEntities Db = new OKDbEntities();
            var rating = Db.rating.SingleOrDefault(x => x.ItemId == id && x.UserId == userid && x.Type == type);
            return rating == null ? 0 : Convert.ToInt32(rating.Score);
        }
        public static IEnumerable<topic> SortTopics(IEnumerable<topic> topics)
        {
            OKDbEntities Db = new OKDbEntities();
            int day = HttpContext.Current.Request.QueryString["day"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Request.QueryString["day"]);
            string sort = HttpContext.Current.Request.QueryString["sort"];
            string order = HttpContext.Current.Request.QueryString["order"] == "asc" ? "asc" : "desc";
            DateTime dt = DateTime.Now.AddDays(-1 * day);
            if (order == "asc")
            {
                if (sort == "title")
                {
                    topics = topics.OrderBy(x => x.Title);
                }
                else if (sort == "writer")
                {
                    topics = topics.OrderBy(x => UserName(x.user));
                }
                else if (sort == "date")
                {
                    topics = topics.OrderBy(x => x.ModifyDate);
                }
                else if (sort == "comments")
                {
                    topics = topics.OrderBy(x => x.comments.Where(y => y.IsApproval == true).Count());
                }
                else if (sort == "views")
                {
                    topics = topics.OrderBy(x => x.ViewsCount);
                }
                else if (sort == "lastcomment")
                {
                    topics = topics.OrderBy(x => LastComment(x) == null ? 0 : ((TimeSpan)(DateTime.Now - LastComment(x).CreationDate)).TotalDays);
                }
                else
                {
                    topics = topics.OrderBy(x => 0.3 * x.ViewsCount + 0.3 * Convert.ToInt32(Convert.ToDateTime(x.ModifyDate).Day) + 0.2 * x.comments.Where(y => y.IsApproval == true).Count() + 0.1 * (Convert.ToInt32(x.Rating.Split('/')[0]) - Convert.ToInt32(x.Rating.Split('/')[1])));
                }
            }
            else
            {
                if (sort == "title")
                {
                    topics = topics.OrderBy(x => x.Title);
                }
                else if (sort == "writer")
                {
                    topics = topics.OrderByDescending(x => UserName(x.user));
                }
                else if (sort == "date")
                {
                    topics = topics.OrderByDescending(x => x.ModifyDate);
                }
                else if (sort == "comments")
                {
                    topics = topics.OrderByDescending(x => x.comments.Where(y => y.IsApproval == true).Count());
                }
                else if (sort == "views")
                {
                    topics = topics.OrderByDescending(x => x.ViewsCount);
                }
                else if (sort == "lastcomment")
                {
                    topics = topics.OrderByDescending(x => LastComment(x) == null ? 0 : ((TimeSpan)(DateTime.Now - LastComment(x).CreationDate)).TotalDays);
                }
                else
                {
                    topics = topics.OrderByDescending(x => 0.2 * x.ViewsCount + 0.6 * Convert.ToInt32(Convert.ToDateTime(x.ModifyDate).Day) + 0.1 * x.comments.Where(y => y.IsApproval == true).Count() + 0.05 * (Convert.ToInt32(x.Rating.Split('/')[0]) - Convert.ToInt32(x.Rating.Split('/')[1])) + 0.05 * x.user.Rating);
                }
            }
            return topics.Where(x => x.IsApproval == true && x.forum.IsApproval == true && x.forum.category.IsApproval == true && day != 0 ? x.CreationDate > dt : true).ToList();
        }
        public static IEnumerable<topic> RelatedTopics(topic topic)
        {
            OKDbEntities Db = new OKDbEntities();
            string sql = "";
            string[] split = topic.Seo.Split('-');
            for (int i = 0; i < split.Count(); i++)
            {
                sql += "Seo like '%" + ConvertSeo(split[i]) + "%' || ";
            }
            sql = sql.Substring(0, sql.Length - 3);
            Random random = new Random();
            var topics = Db.topic.SqlQuery("Select * from topic where IsApproval = '1' and Id != '" + topic.Id + "' and (" + sql + ") order by ModifyDate desc").Take(5);
            for (int i = 0; i < 5 - topics.Count(); i++)
            {
                int skip = random.Next(0, Db.topic.Where(x => x.IsApproval == true).Count());
                topics = topics.Concat(Db.topic.Where(x => x.IsApproval == true && x.Id != topic.Id).OrderByDescending(x => x.ModifyDate).Skip(skip).Take(1));
            }
            return topics.ToList();
        }
        public static topic PreviousTopic(int id)
        {
            OKDbEntities Db = new OKDbEntities();
            return (topic)(Db.topic.Where(x => x.IsApproval == true && x.Id < id).OrderByDescending(x => x.Id).FirstOrDefault());
        }
        public static topic NextTopic(int id)
        {
            OKDbEntities Db = new OKDbEntities();
            return (topic)(Db.topic.Where(x => x.IsApproval == true && x.Id > id).FirstOrDefault());
        }
        public static void AddPost(int userid, string itemid, string type, bool isapproval)
        {
            OKDbEntities Db = new OKDbEntities();
            post post = new post();
            post.UserId = userid;
            post.ItemId = itemid;
            post.Type = type;
            post.CreationDate = DateTime.Now;
            post.IsApproval = isapproval;
            Db.post.Add(post);
            Db.SaveChanges();
        }
        public static void ChangePostApproval(int userid, string itemid, string type, bool isapproval)
        {
            OKDbEntities Db = new OKDbEntities();
            var post = Db.post.SingleOrDefault(x => x.UserId == userid && x.ItemId == itemid && x.Type == type);
            if (post != null)
                post.IsApproval = isapproval;
            Db.SaveChanges();
        }
        public static void DeletePost(int userid, string itemid, string type)
        {
            OKDbEntities Db = new OKDbEntities();
            var post = Db.post.SingleOrDefault(x => x.UserId == userid && x.ItemId == itemid && x.Type == type);
            if (post != null)
                Db.post.Remove(post);
            Db.SaveChanges();
        }
    }
}