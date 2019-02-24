using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BGK.Models;
using BGK.Functions;

namespace BGK.Areas.Admin.Controllers
{
    [Controls("PostManagement")]
    public class PostController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            int categoryID = Convert.ToInt32(Request.QueryString["categoryID"]);
            int memberID = Convert.ToInt32(Request.QueryString["memberID"]);
            var post = !string.IsNullOrEmpty(Request.QueryString["categoryID"]) ? Db.bgk_yazi.Where(x => x.KategoriID == categoryID) : string.IsNullOrEmpty(Request.QueryString["memberID"]) ? Db.bgk_yazi : Db.bgk_yazi.Where(x => x.UyeID == memberID);
            ViewBag.count = post.Count();
            ViewBag.take = 10;
            return View(post.OrderByDescending(x => x.YazimTarihi).Skip(skip).Take(take).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            bgk_yazi post = Db.bgk_yazi.Find(num);
            if (post == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView(post);
        }
        public ActionResult Create()
        {
            ViewBag.KategoriID = new SelectList(Db.bgk_kategori, "Id", "Adi");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_yazi post)
        {
            if (ModelState.IsValid)
            {
                int memberID = (int)Session["memberID"];
                post.UyeID = memberID;
                post.Seo = post.Baslik.ConvertSeo();
                post.YazimTarihi = DateTime.Now;
                post.Okunma = 0;
                Db.bgk_yazi.Add(post);
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            ViewBag.KategoriID = new SelectList(Db.bgk_kategori, "Id", "Adi");
            return View(post);
        }
        public ActionResult Edit(int num = 0)
        {
            bgk_yazi post = Db.bgk_yazi.Find(num);
            if (post == null)
                return HttpNotFound();
            ViewBag.KategoriID = new SelectList(Db.bgk_kategori, "Id", "Adi", post.KategoriID);
            return View(post);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_yazi post)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(post).State = EntityState.Modified;
                post.Seo = post.Baslik.ConvertSeo();
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            ViewBag.KategoriID = new SelectList(Db.bgk_kategori, "Id", "Adi", post.KategoriID);
            return View(post);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            bgk_yazi post = Db.bgk_yazi.Find(num);
            if (post != null)
            {
                result = post.Onay ? "Yazının onayı başarıyla kaldırıldı." : "Yazı başarıyla onaylandı.";
                post.Onay = !post.Onay;
                Db.SaveChanges();
            }
            return Content("<script>$.BGK.SuccessModal('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_yazi post = Db.bgk_yazi.Find(num);
            if (post == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = post.Baslik, Message = "Bu yazıyı ve bu yazıya ait yorumları silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_yazi post = Db.bgk_yazi.Find(model.Id);
            BGKFunction.DeletePost(post);
            Db.bgk_yazi.Remove(post);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Yazı başarılı bir şekilde silindi.', function (){ window.location.href='" + Url.Action("index") + "'; }, 1500);</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}