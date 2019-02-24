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
    public class CategoryController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index()
        {
            return View(Db.bgk_kategori.OrderBy(x => x.Sira).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            bgk_kategori category = Db.bgk_kategori.Find(num);
            if (category == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView(category);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_kategori category)
        {
            if (ModelState.IsValid)
            {
                category.Seo = category.Adi.ConvertSeo();
                Db.bgk_kategori.Add(category);
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(category);
        }
        public ActionResult Edit(int num = 0)
        {
            bgk_kategori category = Db.bgk_kategori.Find(num);
            if (category == null)
                return HttpNotFound();
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_kategori category)
        {
            if (ModelState.IsValid)
            {
                category.Seo = category.Adi.ConvertSeo();
                Db.Entry(category).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(category);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            bgk_kategori category = Db.bgk_kategori.Find(num);
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
            bgk_kategori category = Db.bgk_kategori.Find(num);
            if (category == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = category.Adi, Message = "Bu kategoriyi ve bu kategoriye ait " + category.bgk_yazi.Count() + " yazıyı ve yorumlarını silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        public ActionResult Delete(delete_action model)
        {
            bgk_kategori category = Db.bgk_kategori.Find(model.Id);
            BGKFunction.DeleteCategory(category);
            Db.bgk_kategori.Remove(category);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Kategori başarılı bir şekilde silindi.', function () { window.location.href='" + Url.Action("index") + "' });</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}