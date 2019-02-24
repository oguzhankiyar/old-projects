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
    [Controls("PollManagement")]
    public class PollController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            var poll = Db.bgk_anket;
            ViewBag.count = poll.Count();
            ViewBag.take = take;
            return View(Db.bgk_anket.OrderBy(x => x.Sira).Skip(skip).Take(take).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            bgk_anket bgk_anket = Db.bgk_anket.Find(num);
            if (bgk_anket == null)
                return HttpNotFound();
            return View(bgk_anket);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_anket bgk_anket)
        {
            if (ModelState.IsValid)
            {
                Db.bgk_anket.Add(bgk_anket);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bgk_anket);
        }
        public ActionResult Edit(int num = 0)
        {
            bgk_anket bgk_anket = Db.bgk_anket.Find(num);
            if (bgk_anket == null)
                return HttpNotFound();
            return View(bgk_anket);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_anket bgk_anket)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(bgk_anket).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bgk_anket);
        }
        public ActionResult AddOption(int num = 0)
        {
            bgk_anket bgk_anket = Db.bgk_anket.Find(num);
            if (bgk_anket == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            ViewBag.pollName = bgk_anket.Soru;
            bgk_anket_secenek option = new bgk_anket_secenek() { AnketID = num };
            return PartialView(option);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOption(bgk_anket_secenek bgk_anket_secenek)
        {
            if (ModelState.IsValid)
            {
                Db.bgk_anket_secenek.Add(bgk_anket_secenek);
                Db.SaveChanges();
                return Content("<script>$.BGK.SuccessModal('Seçenek başarılı bir şekilde eklendi.', function (){ window.location.reload(); }, 1500);</script>");
            }
            return Content("<font color=\"red\">Eksik bıraktığınız alan var!</font>");
        }
        public ActionResult DeleteOption(int num = 0)
        {
            bgk_anket_secenek bgk_anket_secenek = Db.bgk_anket_secenek.Find(num);
            var poll = bgk_anket_secenek.bgk_anket;
            if (bgk_anket_secenek == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = "Seçenek Sil", Message = "Bu seçeneği silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOption(delete_action model)
        {
            bgk_anket_secenek option = Db.bgk_anket_secenek.Find(model.Id);
            foreach (var selection in option.bgk_anket_secim)
            {
                Db.bgk_anket_secim.Remove(selection);
            }
            Db.bgk_anket_secenek.Remove(option);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Seçenek başarılı bir şekilde silindi.', function (){ window.location.reload(); }, 1500);</script>");
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            bgk_anket poll = Db.bgk_anket.Find(num);
            if (poll != null)
            {
                poll.Onay = !poll.Onay;
                result = poll.Onay ? "Anket başarıyla onaylandı." : "Anketin onayı başarıyla kaldırıldı.";
                Db.SaveChanges();
            }
            return Content("<script>$.BGK.SuccessModal('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_anket bgk_anket = Db.bgk_anket.Find(num);
            if (bgk_anket == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = bgk_anket.Soru, Message = "Bu anketi silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_anket bgk_anket = Db.bgk_anket.Find(model.Id);
            foreach (var option in bgk_anket.bgk_anket_secenek.ToList())
	        {
                foreach (var selection in option.bgk_anket_secim.ToList())
	            {
		            Db.bgk_anket_secim.Remove(selection);
	            }
		        Db.bgk_anket_secenek.Remove(option);
	        }
            Db.bgk_anket.Remove(bgk_anket);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Anket başarılı bir şekilde silindi.', function (){ window.location.href='" + Url.Action("index") + "'; }, 1500);</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}