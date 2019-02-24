using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BGK.Models;
using BGK.Functions;
using System.IO;

namespace BGK.Areas.Admin.Controllers
{
    [Controls("FileManagement")]
    public class UploadController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int num)
        {
            int take = 10;
            int skip = take * (num - 1);
            var upload = Db.bgk_dosya;
            ViewBag.count = upload.Count();
            ViewBag.take = take;
            return View(upload.OrderByDescending(x => x.YuklenmeTarihi).Skip(skip).Take(take).ToList());
        }
        public ActionResult Details(int num = 0)
        {
            bgk_dosya upload = Db.bgk_dosya.Find(num);
            if (upload == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView(upload);
        }
        public ActionResult Upload(string type, string columnID)
        {
            ViewBag.columnID = columnID;
            return PartialView();
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, string id, string des)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                string[] fileSplit = fileName.Split('.');
                string fileExtension = fileSplit[fileSplit.Length - 1];
                string fileType = file.ContentType.Split('/')[0];
                string filePath = BGKFunction.CreateCode(6) + "_" + fileName.Replace("." + fileExtension, "").ConvertSeo() + "." + fileExtension;
                var path = Server.MapPath("~/Uploads/" + fileType + "/");
                var path2 = Path.Combine(path, filePath);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                file.SaveAs(path2);
                bgk_dosya model = new bgk_dosya();
                model.DosyaAdi = filePath;
                model.Aciklama = des;
                model.Adres = Url.Content("~/Uploads/" + fileType + "/" + filePath);
                model.DosyaTipi = fileType;
                model.YukleyenID = (int)Session["memberID"];
                model.YuklenmeTarihi = DateTime.Now;
                Db.bgk_dosya.Add(model);
                Db.SaveChanges();
                return Content("$.BGK.SuccessModal('Dosyanız başarıyla yüklendi!', function (){ $(\"#" + id + "\").val(" + model.Id + "); }, 1500);");
            }
            else
                return Content("$(\".info\").html(\"<font color=\"red\">Dosya seçmediniz.</font>\");");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file, bgk_dosya model)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                string[] fileSplit = fileName.Split('.');
                string fileExtension = fileSplit[fileSplit.Length - 1];
                string fileType = file.ContentType.Split('/')[0];
                string filePath = BGKFunction.CreateCode(6) + "_" + fileName.Replace("." + fileExtension, "").ConvertSeo() + "." + fileExtension;
                var path = Server.MapPath("~/Uploads/" + fileType + "/");
                var path2 = Path.Combine(path, filePath);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                file.SaveAs(path2);
                model.DosyaAdi = filePath;
                model.Adres = Url.Content("~/Uploads/" + fileType + "/" + filePath);
                model.DosyaTipi = fileType;
                model.YukleyenID = (int)Session["memberID"];
                model.YuklenmeTarihi = DateTime.Now;
                Db.bgk_dosya.Add(model);
                Db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int num = 0)
        {
            bgk_dosya upload = Db.bgk_dosya.Find(num);
            if (upload == null)
                return Content("<script>$.BGK.ErrorModal('Bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.');</script>");
            return PartialView("DeleteActions", new delete_action() { Id = num, Title = "Dosya Sil", Message = "Bu dosyayı diskten tamamen silmek istediğinizden emin misiniz?" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(delete_action model)
        {
            bgk_dosya upload = Db.bgk_dosya.Find(model.Id);
            try
            {
                var path = Server.MapPath("~/Uploads/" + upload.DosyaTipi + "/");
                var fileName = Path.GetFileName(upload.DosyaAdi.Replace("/Uploads/" + upload.DosyaTipi + "/", ""));
                var path2 = Path.Combine(path, fileName);
                System.IO.File.Delete(path2);
            }
            catch (Exception) { }
            Db.bgk_dosya.Remove(upload);
            Db.SaveChanges();
            return Content("<script>$.BGK.SuccessModal('Dosya başarılı bir şekilde silindi.', function (){ window.location.href='" + Url.Action("index") + "'; }, 1500);</script>");
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
