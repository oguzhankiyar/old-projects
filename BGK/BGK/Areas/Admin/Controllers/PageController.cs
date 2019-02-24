using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using BGK.Models;
using BGK.Functions;

namespace BGK.Areas.Admin.Controllers
{
    [Controls("PageManagement")]
    public class PageController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index()
        {
            return View(Db.bgk_sayfa.Where(x => x.Onay == true).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            bgk_sayfa page = Db.bgk_sayfa.Find(num);
            if (page == null)
                return HttpNotFound();
            return View(page);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_sayfa page)
        {
            if (ModelState.IsValid)
            {
                Db.bgk_sayfa.Add(page);
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(page);
        }
        public ActionResult Edit(int num = 0)
        {
            bgk_sayfa page = Db.bgk_sayfa.Find(num);
            if (page == null)
                return HttpNotFound();
            return View(page);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_sayfa page)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(page).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(page);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            bgk_sayfa page = Db.bgk_sayfa.Find(num);
            if (page != null)
            {
                page.Onay = page.Onay == true ? false : true;
                result = page.Onay == true ? "Sayfa başarıyla onaylandı." : "Sayfanın onayı başarıyla kaldırıldı.";
                Db.SaveChanges();
            }
            return Content("<script>$.BGK.SuccessModal('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_sayfa page = Db.bgk_sayfa.Find(num);
            if (page == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = page.Adi, Message = "Bu sayfayı silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_sayfa page = Db.bgk_sayfa.Find(model.Id);
            Db.bgk_sayfa.Remove(page);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Sayfa başarılı bir şekilde silindi.', function (){ window.location.href='" + Url.Action("index") + "'; }, 1500);</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
