using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BGK.Models;
using System.Data;
using BGK.Functions;

namespace BGK.Areas.Admin.Controllers
{
    [Controls("PostManagement")]
    public class CommentController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            int postID = Convert.ToInt32(Request.QueryString["postID"]);
            int memberID = Convert.ToInt32(Request.QueryString["memberID"]);
            var comment = !string.IsNullOrEmpty(Request.QueryString["postID"]) ? Db.bgk_yorum.Where(x => x.YaziID == postID) : string.IsNullOrEmpty(Request.QueryString["memberID"]) ? Db.bgk_yorum : Db.bgk_yorum.Where(x => x.UyeID == memberID);
            ViewBag.count = comment.Count();
            ViewBag.take = take;
            return View(comment.OrderByDescending(x => x.YazilmaTarihi).Skip(skip).Take(take).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            bgk_yorum comment = Db.bgk_yorum.Find(num);
            if (comment == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView(comment);
        }
        public ActionResult Create()
        {
            ViewBag.YaziID = new SelectList(Db.bgk_yazi, "Id", "Baslik");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bgk_yorum comment)
        {
            if (ModelState.IsValid)
            {
                comment.UyeID = (int)Session["memberID"];
                comment.YazilmaTarihi = DateTime.Now;
                Db.bgk_yorum.Add(comment);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.YaziID = new SelectList(Db.bgk_yazi, "Id", "Baslik");
            return View(comment);
        }
        public ActionResult Edit(int num = 0)
        {
            bgk_yorum comment = Db.bgk_yorum.Find(num);
            if (comment == null)
                return HttpNotFound();
            return View(comment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bgk_yorum comment)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(comment).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(comment);
        }
        public ActionResult ChangeApproval(int num = 0)
        {
            string result = "";
            bgk_yorum comment = Db.bgk_yorum.Find(num);
            if (comment != null)
            {
                result = comment.Onay ? "Yorumun onayı başarıyla kaldırıldı." : "Yorum başarıyla onaylandı.";
                comment.Onay = !comment.Onay;
                Db.SaveChanges();
            }
            return Content("<script>$.BGK.SuccessModal('" + result + "');</script>");
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_yorum comment = Db.bgk_yorum.Find(num);
            if (comment == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = "Yorum Sil", Message = "Bu yorumu silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_yorum comment = Db.bgk_yorum.Find(model.Id);
            Db.bgk_yorum.Remove(comment);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Yorum başarıyla silindi.', function () { window.location.href='" + Url.Action("index") + "' });</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
