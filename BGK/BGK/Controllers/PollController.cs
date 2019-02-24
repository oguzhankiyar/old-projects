using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BGK.Models;
using BGK.Functions;

namespace BGK.Controllers
{
    [MaintenanceControl]
    public class PollController : Controller
    {
        private BGKEntities Db = new BGKEntities();
        public ActionResult Index(int page)
        {
            int memberID = Convert.ToInt32(Session["memberID"]);
            var poll = Db.bgk_anket.Where(x => x.Onay == true && (memberID == 0 ? x.SadeceUye == false : true)).OrderBy(x => x.Sira).ToList();
            if (poll.Count() == 0)
                return Content(null);
            else
            {
                int take = 4;
                int skip = take * (page - 1);
                ViewBag.Title = "Anketler";
                ViewBag.count = poll.Count();
                ViewBag.take = take;
                return View(poll.Skip(skip).Take(take));
            }
        }
        [HttpPost]
        public ActionResult Vote()
        {
            int id = Convert.ToInt32(Request.Form["id"]);
            if (!BGKFunction.IsVoted(id))
            {
                string[] optionID = Request.Form["optionID"].Replace("%2c", ",").Split(',');
                for (int i = 0; i < optionID.Length; i++)
                {
                    bgk_anket_secim selection = new bgk_anket_secim();
                    if (Session["memberInfo"] != null)
                        selection.UyeID = (int)Session["memberID"];
                    else
                        selection.OylayanIP = BGKFunction.GetIPAddress();
                    selection.SecenekID = Convert.ToInt32(optionID[i]);
                    selection.SecimTarihi = DateTime.Now;
                    Db.bgk_anket_secim.Add(selection);
                }
                Db.SaveChanges();
                return Content("Oy kullandığınız için teşekkür ederiz.<br />Oylar sayılıyor..");
            }
            else
                return Content("Daha önce oy kullandınız..");
        }
        public ActionResult Results(int id)
        {
            id = string.IsNullOrEmpty(Request.Form["id"]) ? id : Convert.ToInt32(Request.Form["id"]);
            var poll = Db.bgk_anket.SingleOrDefault(x => x.Id == id && x.Onay == true);
            if (poll == null)
                return HttpNotFound();
            else
                return PartialView(poll);
        }

    }
}
