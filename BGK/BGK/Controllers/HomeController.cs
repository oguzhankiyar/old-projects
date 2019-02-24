using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BGK.Models;
using BGK.Functions;

namespace BGK.Controllers
{
    public class HomeController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index()
        {
            if (BGKFunction.GetConfig("site-index") != "0" && Session["Index"].ToString() == "0")
            {
                Session["Index"] = "1";
                return PartialView();                
            }
            return RedirectToAction("Index", "Post");
        }
        public ActionResult Maintenance()
        {
            if (Session["memberRole"].ToString() == "100" || BGKFunction.GetConfig("site-on/off") == "1")
                return RedirectToAction("Index");
            return View();
        }
        public ActionResult Menu()
        {
            if (Session["memberInfo"] == null)
                return PartialView(Db.bgk_menu.Where(x => x.SadeceUye == false && x.Onay == true).OrderBy(x => x.Sira).ToList());
            return PartialView(Db.bgk_menu.Where(x => x.Onay == true).OrderBy(x => x.Sira).ToList());
        }
        public ActionResult Membership()
        {
            return PartialView();
        }
        [MaintenanceControl]
        public ActionResult Communication()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Communication(bgk_communication model)
        {
            if (ModelState.IsValid)
            {
                if (BGKFunction.IsSendEmail(BGKFunction.GetConfig("smtp-username"), model.Konu, model.Mesaj + "<br /><br />Bu mesaj <b>" + model.AdSoyad + "(" + model.Email + ")</b> tarafından gönderilmiştir."))
                    ModelState.AddModelError("", "Mesajınız başarıyla gönderilmiştir.");
                else
                    ModelState.AddModelError("", "Mesajınız gönderilirken bir sorun oluştu. Lütfen tekrar deneyiniz.");
            }
            return View(model);
        }
        [MaintenanceControl]
        public ActionResult Modal()
        {
            if (Session["memberID"].ToString() != "0")
            {
                HttpCookie cookie = new HttpCookie("BGK_member-message");
                cookie.Value = BGKFunction.GetConfig("member-message").ConvertSeo();
                cookie.Expires = DateTime.Now.AddDays(365);
                Response.Cookies.Add(cookie);
            }
            else
            {
                HttpCookie cookie = new HttpCookie("BGK_visitor-message");
                cookie.Value = Server.UrlDecode(BGKFunction.GetConfig("visitor-message")).ConvertSeo();
                cookie.Expires = DateTime.Now.AddDays(365);
                Response.Cookies.Add(cookie);
            }
            return PartialView();
        }
        public ActionResult Sitemap()
        {
            List<SitemapItem> veriler = new List<SitemapItem>();
            veriler.Add(new SitemapItem() { loc = Url.Action("index", "post", null, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
            veriler.Add(new SitemapItem() { loc = Url.Action("communication", "home", null, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
            veriler.Add(new SitemapItem() { loc = Url.Action("index", "gallery", null, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
            veriler.Add(new SitemapItem() { loc = Url.Action("index", "document", null, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
            veriler.Add(new SitemapItem() { loc = Url.Action("index", "activity", null, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
            veriler.Add(new SitemapItem() { loc = Url.Action("index", "factory", null, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
            foreach (var page in Db.bgk_sayfa.Where(x => x.Onay == true))
            {
                veriler.Add(new SitemapItem() { loc = Url.Action("details", "page", new { seo = page.Seo }, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
            }
            foreach (var gallery in Db.bgk_galeri.Where(x => x.Onay == true))
            {
                veriler.Add(new SitemapItem() { loc = Url.Action("category", "gallery", new { seo = gallery.Seo, id = gallery.Id }, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
                foreach (var image in gallery.bgk_galeri_resim.Where(x => x.Onay == true))
                {
                    veriler.Add(new SitemapItem() { loc = Url.Action("images", "gallery", new { id = image.Id, seo = gallery.Seo }, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
                }
            }
            foreach (var document_category in Db.bgk_dokuman_kategori.Where(x => x.Onay == true))
            {
                veriler.Add(new SitemapItem() { loc = Url.Action("category", "document", new { seo = document_category.Seo, id = document_category.Id }, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
                foreach (var document in document_category.bgk_dokuman.Where(x => x.Onay == true))
                {
                    veriler.Add(new SitemapItem() { loc = Url.Action("details", "document", new { seo = document.Seo, id = document_category.Id }, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
                }
            }
            foreach (var post in Db.bgk_yazi.Where(x => x.Onay == true))
            {
                veriler.Add(new SitemapItem() { loc = Url.Action("details", "post", new { id = post.Id, seo = post.Seo }, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
            }
            foreach (var activity in Db.bgk_etkinlik)
            {
                veriler.Add(new SitemapItem() { loc = Url.Action("details", "activity", new { id = activity.Id, seo = activity.Adi.ConvertSeo() }, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
                foreach (var speaker in activity.bgk_etkinlik_konusmaci)
                {
                    veriler.Add(new SitemapItem() { loc = Url.Action("speaker", "activity", new { id = speaker.Id, seo = speaker.AdSoyad.ConvertSeo() }, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
                }
                foreach (var officer in activity.bgk_etkinlik_gorevli)
                {
                    veriler.Add(new SitemapItem() { loc = Url.Action("officer", "activity", new { id = officer.Id, seo = officer.bgk_uye.AdSoyad.ConvertSeo() }, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
                }
            }
            foreach (var factory in Db.bgk_firma)
            {
                veriler.Add(new SitemapItem() { loc = Url.Action("details", "factory", new { id = factory.Id, seo = factory.Adi.ConvertSeo() }, "http"), lastmod = DateTime.Now, changefreq = "daily", priority = "1" });
            }
            Sitemap sitemap = new Sitemap(veriler);
            return Content(sitemap.Result(), "text/xml");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
