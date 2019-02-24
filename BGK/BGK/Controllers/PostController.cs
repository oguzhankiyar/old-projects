using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BGK.Models;
using BGK.Functions;

namespace BGK.Controllers
{
    [MaintenanceControl]
    public class PostController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int page)
        {
            var post = Db.bgk_yazi.Where(x => x.Onay == true && x.bgk_kategori.Onay == true).OrderByDescending(x => x.YazimTarihi).ToList();
            int take = 7;
            int skip = take * (page - 1);
            ViewBag.Message = "Gösterilebilecek yazı bulunamadı :(";
            ViewBag.count = post.Count();
            ViewBag.take = take;
            return View(post.Skip(skip).Take(take));
        }
        public ActionResult Category(string seo, int page)
        {
            var category = Db.bgk_kategori.SingleOrDefault(x => x.Seo == seo && x.Onay == true);
            if (category == null)
                return HttpNotFound();
            else
            {
                var post = category.bgk_yazi.Where(x => x.Onay == true).OrderByDescending(x => x.YazimTarihi).ToList();
                int take = 7;
                int skip = take * (page - 1);
                ViewBag.count = post.Count();
                ViewBag.take = take;
                ViewBag.Title = category.Adi;
                ViewBag.Message = "Bu kategoriye ait yazı bulunamadı :(";
                return View("Index", post.Skip(skip).Take(take));
            }
        }
        public ActionResult Search(string q, int page)
        {
            int take = 7;
            int skip = take * (page - 1);
            string sql = "";
            string[] split = q.ConvertSeo().Split('-');
            for (int i = 0; i < split.Count(); i++)
            {
                sql += "Seo like '%" + split[i] + "%' or ";
            }
            sql = sql.Substring(0, sql.Length - 3);
            var posts = Db.bgk_yazi.SqlQuery("Select * from bgk_yazi where Onay = 1 and (" + sql + ")").Where(x => x.bgk_kategori.Onay == true);
            ViewBag.Title = "Arama: " + q;
            ViewBag.Message = "Bu kelimeye uygun yazı bulunamadı :(";
            ViewBag.count = posts.Count();
            ViewBag.take = take;
            ViewBag.query = q;
            return View("Index", posts.Skip(skip).Take(take).ToList());
        }
        public ActionResult Popular(int page)
        {
            var post = Db.bgk_yazi.Where(x => x.Onay == true && x.bgk_kategori.Onay == true).OrderByDescending(x => x.Okunma).ToList();
            int take = 7;
            int skip = take * (page - 1);
            ViewBag.Title = "Neler Okunuyor?";
            ViewBag.Message = "Gösterilebilecek yazı bulunamadı :(";
            ViewBag.count = post.Count();
            ViewBag.take = take;
            return View("Index", post.Skip(skip).Take(take));
        }
        public ActionResult Details(int id, string seo, int page = 1)
        {
            var post = Db.bgk_yazi.SingleOrDefault(x => x.Id == id && x.Onay == true && x.bgk_kategori.Onay == true);
            if (post == null)
                return HttpNotFound();
            var comment = post.bgk_yorum.Where(x => x.Onay == true).OrderByDescending(x => x.YazilmaTarihi).ToList();
            int take = 7;
            int skip = take * (page - 1);
            ViewBag.Title = post.Baslik;
            ViewBag.count = comment.Count();
            ViewBag.take = take;
            ViewBag.comments = comment.Skip(skip).Take(take);
            post.Okunma++;
            Db.SaveChanges();
            return View(post);
        }
        public ActionResult Create()
        {
            if (!BGKFunction.GetMyRole().YaziYazma)
                return HttpNotFound();
            ViewBag.KategoriID = new SelectList(Db.bgk_kategori.Where(x => x.Onay == true), "Id", "Adi");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_yazi model)
        {
            if (ModelState.IsValid)
            {
                model.Seo = model.Baslik.ConvertSeo();
                model.YazimTarihi = DateTime.Now;
                model.UyeID = (int)Session["memberID"];
                model.Onay = BGKFunction.GetMyRole().YaziOnay;
                model.Manset = false;
                model.Okunma = 0;
                Db.bgk_yazi.Add(model);
                Db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.KategoriID = new SelectList(Db.bgk_kategori.Where(x => x.Onay == true), "Id", "Adi");
            return View(model);
        }
        [HttpPost]
        public ActionResult Vote()
        {
            int id = Convert.ToInt32(Request.Form["id"]);
            if (!BGKFunction.GetMyRole().OyKullanma)
                return Content("Oy Kullanma yetkiniz yok :(");
            else if (!BGKFunction.IsPostVoted(id))
            {
                bgk_oylama rating = new bgk_oylama();
                rating.Puan = Convert.ToInt32(Request.Form["point"]);
                if (Session["memberInfo"] != null)
                    rating.UyeID = (int)Session["memberID"];
                else
                    rating.OylayanIP = BGKFunction.GetIPAddress();
                rating.YaziID = id;
                Db.bgk_oylama.Add(rating);
                Db.SaveChanges();
                return Content("Oy kullandığınız için teşekkür ederiz.");
            }
            else
                return Content("Daha önce oy kullandınız.");
        }
        [HttpPost]
        public ActionResult CreateCommentSave(bgk_yorum comment)
        {
            string result;
            if (comment.Yorum == null || (Session["memberInfo"] == null && comment.Yazan == null))
            {
                result = "<font color=red>Boş bıraktığınız alan var..</font>";
            }
            else
            {
                comment.YazilmaTarihi = DateTime.Now;
                if (Session["memberID"].ToString() == "0")
                {
                    comment.Onay = false;
                    result = "<font color=green>Yorumunuz başarıyla kaydedildi.. Onaylandıktan sonra yayınlanacak..</font><script typr=\"text/javascript\">setTimeout(function (){ ToggleCommentBar(); }, 2000);</script>";
                }
                else
                {
                    comment.Onay = BGKFunction.GetMyRole().YorumOnay;
                    var post = Db.bgk_yazi.Find(comment.YaziID);
                    result = "<font color=green>Yorumunuz başarıyla yayınlandı..</font><script typr=\"text/javascript\">setTimeout(function (){ window.location.href = '" + Url.Action("Details", new { id = comment.YaziID, seo = post.Seo }) + "'; }, 2000);</script>";
                }
                Db.bgk_yorum.Add(comment);
                Db.SaveChanges();
            }
            return Content(result);
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
