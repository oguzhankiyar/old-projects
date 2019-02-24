using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BGK.Models;
using BGK.Functions;
using System.Data;

namespace BGK.Areas.Admin.Controllers
{
    [Controls("MenuManagement")]
    public class MenuController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index()
        {
            return View(Db.bgk_menu.OrderBy(x => x.Sira).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            bgk_menu link = Db.bgk_menu.Find(num);
            if (link == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView(link);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_menu link)
        {
            if (ModelState.IsValid)
            {
                Db.bgk_menu.Add(link);
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(link);
        }
        public ActionResult Edit(int num = 0)
        {
            bgk_menu link = Db.bgk_menu.Find(num);
            if (link == null)
                return HttpNotFound();
            return View(link);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_menu link)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(link).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(link);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            bgk_menu link = Db.bgk_menu.Find(num);
            if (link != null)
            {
                link.Onay = !link.Onay;
                result = link.Onay == true ? "Link başarıyla onaylandı." : "Linkin onayı başarıyla kaldırıldı.";
                Db.SaveChanges();
            }
            return Content("<script>$.BGK.SuccessModal('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_menu link = Db.bgk_menu.Find(num);
            if (link == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = "Link Sil", Message = "Bu linki silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_menu link = Db.bgk_menu.Find(model.Id);
            Db.bgk_menu.Remove(link);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Link başarılı bir şekilde silindi.', function (){ window.location.href='" + Url.Action("index") + "'; }, 1500);</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
