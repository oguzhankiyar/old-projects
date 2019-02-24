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
    [Controls("BudgetManagement")]
    public class BudgetController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            var budget = Db.bgk_butce.OrderByDescending(x => x.IslemTarihi).ToList();
            ViewBag.count = budget.Count();
            ViewBag.take = take;
            ViewBag.InCome = budget.Where(x => x.Islem == 1).Sum(x => x.Miktar);
            ViewBag.Expense = budget.Where(x => x.Islem == 2).Sum(x => x.Miktar);
            ViewBag.Title = "Gelir-Gider Hareketleri";
            return View(budget.Skip(skip).Take(take));
        }
        public ActionResult Details(int num = 0)
        {
            bgk_butce budget = Db.bgk_butce.Find(num);
            if (budget == null)
                return HttpNotFound();
            return PartialView(budget);
        }
        public ActionResult Create()
        {
            ViewBag.FirmaID = new SelectList(Db.bgk_firma, "Id", "Adi");
            ViewBag.EtkinlikID = new SelectList(Db.bgk_etkinlik, "Id", "Adi");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_butce budget)
        {
            if (ModelState.IsValid)
            {
                Db.bgk_butce.Add(budget);
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            ViewBag.FirmaID = new SelectList(Db.bgk_firma, "Id", "Adi");
            ViewBag.EtkinlikID = new SelectList(Db.bgk_etkinlik, "Id", "Adi");
            return View(budget);
        }
        public ActionResult Edit(int num = 0)
        {
            bgk_butce budget = Db.bgk_butce.Find(num);
            if (budget == null)
                return HttpNotFound();
            ViewBag.FirmaID = new SelectList(Db.bgk_firma, "Id", "Adi", budget.FirmaID);
            ViewBag.EtkinlikID = new SelectList(Db.bgk_etkinlik, "Id", "Adi", budget.EtkinlikID);
            return View(budget);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_butce budget)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(budget).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            ViewBag.FirmaID = new SelectList(Db.bgk_firma, "Id", "Adi", budget.FirmaID);
            ViewBag.EtkinlikID = new SelectList(Db.bgk_etkinlik, "Id", "Adi", budget.EtkinlikID);
            return View(budget);
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_butce budget = Db.bgk_butce.Find(num);
            if (budget == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = "İşlem Sil", Message = "Bu işlemi silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        public ActionResult Delete(delete_action model)
        {
            bgk_butce budget = Db.bgk_butce.Find(model.Id);
            if (budget.bgk_dosya != null)
                Db.bgk_dosya.Remove(budget.bgk_dosya);
            Db.bgk_butce.Remove(budget);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('İşlem başarılı bir şekilde silindi.', function () { window.location.href='" + Url.Action("index") + "' });</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
