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
    [Controls("DocumentManagement")]
    public class DocumentCategoryController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index()
        {
            return View(Db.bgk_dokuman_kategori.OrderBy(x => x.Sira).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            bgk_dokuman_kategori bgk_dokuman_kategori = Db.bgk_dokuman_kategori.Find(num);
            if (bgk_dokuman_kategori == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView(bgk_dokuman_kategori);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_dokuman_kategori bgk_dokuman_kategori)
        {
            if (ModelState.IsValid)
            {
                bgk_dokuman_kategori.Seo = bgk_dokuman_kategori.Adi.ConvertSeo();
                Db.bgk_dokuman_kategori.Add(bgk_dokuman_kategori);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bgk_dokuman_kategori);
        }
        public ActionResult Edit(int id = 0)
        {
            bgk_dokuman_kategori bgk_dokuman_kategori = Db.bgk_dokuman_kategori.Find(id);
            if (bgk_dokuman_kategori == null)
                return HttpNotFound();
            return View(bgk_dokuman_kategori);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_dokuman_kategori bgk_dokuman_kategori)
        {
            if (ModelState.IsValid)
            {
                bgk_dokuman_kategori.Seo = bgk_dokuman_kategori.Adi.ConvertSeo();
                Db.Entry(bgk_dokuman_kategori).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bgk_dokuman_kategori);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            bgk_dokuman_kategori category = Db.bgk_dokuman_kategori.Find(num);
            if (category != null)
            {
                category.Onay = !category.Onay;
                result = category.Onay == true ? "Kategori başarıyla onaylandı." : "Kategorinin onayı başarıyla kaldırıldı.";
                Db.SaveChanges();
            }
            return Content("<script>$.BGK.SuccessModal('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_dokuman_kategori bgk_dokuman_kategori = Db.bgk_dokuman_kategori.Find(num);
            if (bgk_dokuman_kategori == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = bgk_dokuman_kategori.Adi, Message = "Bu kategoriyi ve bu kategoriye ait tüm dökümanları silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_dokuman_kategori bgk_dokuman_kategori = Db.bgk_dokuman_kategori.Find(model.Id);
            foreach (var document in bgk_dokuman_kategori.bgk_dokuman)
            {
                BGKFunction.RemoveUploadFile(document.bgk_dosya);
                Db.bgk_dokuman.Remove(document);                
            }
            Db.bgk_dokuman_kategori.Remove(bgk_dokuman_kategori);
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