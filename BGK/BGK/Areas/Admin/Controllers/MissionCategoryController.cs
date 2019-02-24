using BGK.Functions;
using BGK.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGK.Areas.Admin.Controllers
{
    [Controls("MissionManagement")]
    public class MissionCategoryController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index()
        {
            return View(Db.bgk_gorev_kategori.OrderBy(x => x.Sira).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            bgk_gorev_kategori category = Db.bgk_gorev_kategori.Find(num);
            if (category == null)
                return HttpNotFound();
            return View(category);
        }
        public ActionResult Create()
        {
            ViewBag.OlusturanID = new SelectList(Db.bgk_uye, "Id", "AdSoyad", Convert.ToInt32(Request.QueryString["memberID"]));
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_gorev_kategori category)
        {
            if (ModelState.IsValid)
            {
                Db.bgk_gorev_kategori.Add(category);
                Db.SaveChanges();
                ViewBag.OlusturanID = new SelectList(Db.bgk_uye, "Id", "AdSoyad", Convert.ToInt32(Request.QueryString["memberID"]));
                return RedirectToAction("index");
            }
            return View(category);
        }
        public ActionResult Edit(int num = 0)
        {
            bgk_gorev_kategori category = Db.bgk_gorev_kategori.Find(num);
            if (category == null)
                return HttpNotFound();
            ViewBag.OlusturanID = new SelectList(Db.bgk_uye, "Id", "AdSoyad", category.OlusturanID);
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_gorev_kategori category)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(category).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            ViewBag.OlusturanID = new SelectList(Db.bgk_uye, "Id", "AdSoyad", category.OlusturanID);
            return View(category);
        }
        public ActionResult SubscribtionDetails(int num)
        {
            var subscribtion = Db.bgk_gorev_kategori_uye.Find(num);
            if (subscribtion == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView(subscribtion);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            bgk_gorev_kategori category = Db.bgk_gorev_kategori.Find(num);
            if (category != null)
            {
                category.Onay = category.Onay == true ? false : true;
                result = category.Onay == true ? "Kategori başarıyla onaylandı." : "Kategorinin onayı başarıyla kaldırıldı.";
                Db.SaveChanges();
            }
            return Content("<script>$.BGK.SuccessModal('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_gorev_kategori category = Db.bgk_gorev_kategori.Find(num);
            if (category == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = category.Adi, Message = "Bu kategoriyi ve bu kategoriye ait tüm görevleri silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_gorev_kategori category = Db.bgk_gorev_kategori.Find(model.Id);
            BGKFunction.DeleteMissionCategory(category);
            Db.bgk_dosya.Remove(category.bgk_dosya);
            Db.bgk_gorev_kategori.Remove(category);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Kategori başarılı bir şekilde silindi.', function (){ window.location.href='" + Url.Action("index") + "'; }, 1500);</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
