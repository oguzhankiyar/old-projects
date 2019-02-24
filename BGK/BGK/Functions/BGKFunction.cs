using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BGK.Models;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace BGK.Functions
{
    public class BGKFunction
    {
        public static bool IsSendEmail(string email, string title, string content)
        {
            try
            {
                string emailfrom = "noreplay@gmail.com";
                content = "<html><head><meta content=\"text/html; charset=utf-8\" /></head><body>" + content + "</body></html>";
                SmtpClient client = new SmtpClient(GetConfig("smtp-server"), Convert.ToInt32(GetConfig("smtp-port")));
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(GetConfig("smtp-username"), GetConfig("smtp-password"));
                MailMessage message = new MailMessage(emailfrom, email, title, content);
                message.IsBodyHtml = true;
                client.Send(message);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string CreateCode(int count)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            Random random = new Random();
            string code = "";
            for (int i = 0; i < count; i++)
            {
                int index = random.Next(0, chars.Length - 1);
                if (!code.Contains(chars.GetValue(index).ToString()))
                    code += chars.GetValue(index);
                else
                    i--;
            }
            return code;
        }
        public static void AddLog(int memberID, string section, string action)
        {
            /*
             * BGKEntities Db = new BGKEntities();
             * bgk_log log = new bgk_log();
             * log.Bolum = section;
             * log.Islem = action;
             * log.UyeID = memberID;
             * log.IslemTarihi = DateTime.Now;
             * Db.bgk_log.Add(log);
             * Db.SaveChanges();
             */
        }
        public static void AddNotification(int memberID, string content)
        {
            /*
             * BGKEntities Db = new BGKEntities();
             * bgk_bildirim notification = new bgk_bildirim();
             * notification.UyeID = memberID;
             * notification.Icerik = content;
             * notification.Tarih = DateTime.Now;
             * Db.bgk_bildirim.Add(notification);
             * Db.SaveChanges();
             */
        }
        public static bgk_yetki GetMyRole()
        {
            BGKEntities Db = new BGKEntities();
            if (HttpContext.Current.Session["memberID"].ToString() == "0")
                return (Db.bgk_yetki.SingleOrDefault(x => x.Kod == 0));
            else
            {
                int memberID = (int)HttpContext.Current.Session["memberID"];
                var member = Db.bgk_uye.Find(memberID);
                return (member.GetMemberRole());
            }
        }
        public static string GetConfig(string key)
        {
            BGKEntities Db = new BGKEntities();
            var config = Db.bgk_ayar.SingleOrDefault(x => x.Adi == key);
            if (config != null)
                return config.Deger;
            else
                return null;
        }
        public static bgk_uye GetMember(int id)
        {
            BGKEntities Db = new BGKEntities();
            return Db.bgk_uye.Find(id);
        }
        public static string GetIPAddress()
        {
            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }
        public static bool IsVoted(int id)
        {
            BGKEntities Db = new BGKEntities();
            int memberID = Convert.ToInt32(HttpContext.Current.Session["memberID"]);
            string IP = GetIPAddress().ToString();
            var control = Db.bgk_anket_secim.Where(x => x.bgk_anket_secenek.AnketID == id && (x.UyeID == memberID || x.OylayanIP == IP));
            if (control.Count() == 0)
                return false;
            else
                return true;
        }
        public static bool IsPostVoted(int id)
        {
            BGKEntities Db = new BGKEntities();
            int memberID = Convert.ToInt32(HttpContext.Current.Session["memberID"]);
            string IP = GetIPAddress().ToString();
            var control = Db.bgk_oylama.Where(x => x.YaziID == id && (x.UyeID == memberID || x.OylayanIP == IP));
            if (control.Count() == 0)
                return false;
            else
                return true;
        }
        public static int GetPostPoint(int id)
        {
            BGKEntities Db = new BGKEntities();
            var ratings = Db.bgk_oylama.Where(x => x.YaziID == id).ToList();
            int totalPoint = 0;
            foreach (var rating in ratings)
                totalPoint += rating.Puan;
            decimal ceiling = ratings.Count() == 0 ? 0 : totalPoint / ratings.Count();
            return Convert.ToInt32(Math.Ceiling(ceiling));
        }
        public static string GetGrade(int score)
        {
            BGKEntities Db = new BGKEntities();
            var grade = Db.bgk_seviye.Where(x => x.AltSinir <= score).OrderByDescending(x => x.AltSinir).FirstOrDefault();
            return grade == null ? null : grade.Adi;
        }
        public static void RemoveUploadFile(bgk_dosya file)
        {
            BGKEntities Db = new BGKEntities();
            if (file != null)
            {
                try
                {
                    var path = HttpContext.Current.Server.MapPath("~/Uploads/" + file.DosyaTipi + "/");
                    var fileName = Path.GetFileName(file.DosyaAdi.Replace("/Uploads/" + file.DosyaTipi + "/", ""));
                    var path2 = Path.Combine(path, fileName);
                    System.IO.File.Delete(path2);
                }
                catch (Exception) { }
                Db.bgk_dosya.Remove(file);
            }
            Db.SaveChanges();
        }
        public static bgk_uye GetMyAccount()
        {
            BGKEntities Db = new BGKEntities();
            return Db.bgk_uye.Find(HttpContext.Current.Session["memberID"]);
        }
        public static string GetFile(bgk_dosya file)
        {
            if (file == null)
                return "/Uploads/error.jpg";
            else
                return file.Adres;
        }
        public static void DeleteActivity(bgk_etkinlik activity)
        {
            BGKEntities Db = new BGKEntities();
            foreach (var speaker in activity.bgk_etkinlik_konusmaci)
            {
                Db.bgk_etkinlik_konusmaci.Remove(speaker);
            }
            foreach (var officer in activity.bgk_etkinlik_gorevli)
            {
                Db.bgk_etkinlik_gorevli.Remove(officer);
            }
            Db.bgk_etkinlik.Remove(activity);
            Db.SaveChanges();
        }
        public static void DeleteCategory(bgk_kategori category)
        {
            BGKEntities Db = new BGKEntities();
            foreach (var post in category.bgk_yazi)
	        {
                DeletePost(post);
	        }
            Db.bgk_kategori.Remove(category);
            Db.SaveChanges();
        }
        public static void DeletePost(bgk_yazi post)
        {
            BGKEntities Db = new BGKEntities();
            foreach (var comment in post.bgk_yorum)
            {
                Db.bgk_yorum.Remove(comment);
            }
            foreach (var rating in post.bgk_oylama)
            {
                Db.bgk_oylama.Remove(rating);
            }
            Db.bgk_yazi.Remove(post);
            Db.SaveChanges();
        }
        public static void DeleteMissionCategory(bgk_gorev_kategori category)
        {
            BGKEntities Db = new BGKEntities();
            foreach (var member in category.bgk_gorev_kategori_uye)
	        {
                Db.bgk_gorev_kategori_uye.Remove(member);
	        }
            foreach (var mission in category.bgk_gorev)
            {
                DeleteMission(mission);
            }
            Db.bgk_gorev_kategori.Remove(category);
            Db.SaveChanges();
        }
        public static void DeleteMission(bgk_gorev mission)
        {
            BGKEntities Db = new BGKEntities();
            foreach (var member in mission.bgk_gorev_uye)
            {
                Db.bgk_gorev_uye.Remove(member);
            }
            Db.bgk_gorev.Remove(mission);
            Db.SaveChanges();
        }
        public static void DeleteGroup(bgk_grup group)
        {
            BGKEntities Db = new BGKEntities();
            foreach (var member in group.bgk_grup_uye)
            {
                Db.bgk_grup_uye.Remove(member);
            }
            Db.bgk_grup.Remove(group);
            Db.SaveChanges();
        }
        public static void DeleteMember(bgk_uye member)
        {
            BGKEntities Db = new BGKEntities();
            foreach (var post in member.bgk_yazi)
            {
                DeletePost(post);
            }
            foreach (var comment in member.bgk_yorum)
            {
                Db.bgk_yorum.Remove(comment);
            }
            foreach (var group in member.bgk_grup_uye)
            {
                if (group.Tip == 1)
                {
                    DeleteGroup(group.bgk_grup);
                }
                Db.bgk_grup_uye.Remove(group);
            }
            foreach (var category in member.bgk_gorev_kategori)
            {
                DeleteMissionCategory(category);
            }
            foreach (var categorymember in member.bgk_gorev_kategori_uye)
            {
                Db.bgk_gorev_kategori_uye.Remove(categorymember);
            }
            foreach (var mission in member.bgk_gorev)
            {
                DeleteMission(mission);                
            }
            foreach (var missionmember in member.bgk_gorev_uye)
            {
                Db.bgk_gorev_uye.Remove(missionmember);
            }
            foreach (var officer in member.bgk_etkinlik_gorevli)
	        {
		         Db.bgk_etkinlik_gorevli.Remove(officer);
	        }
            foreach (var selection in member.bgk_anket_secim)
            {
                Db.bgk_anket_secim.Remove(selection);
            }
            Db.SaveChanges();
        }
    }
}