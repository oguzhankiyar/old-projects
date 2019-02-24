using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGK.Functions
{
    public class Controls : ActionFilterAttribute
    {
        private string _role { get; set; }
        public Controls(string role)
        {
            this._role = role;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool isRole = true;
            var role = BGKFunction.GetMyRole();
            switch (_role)
            {
                case "PostManagement":
                    if (!role.Yazi)
                        isRole = false;
                    break;
                case "CommentManagement":
                    if (!role.Yorum)
                        isRole = false;
                    break;
                case "PageManagement":
                    if (!role.Sayfa)
                        isRole = false;
                    break;
                case "ConfigManagement":
                    if (!role.Ayar)
                        isRole = false;
                    break;
                case "ActivityManagement":
                    if (!role.Etkinlik)
                        isRole = false;
                    break;
                case "BudgetManagement":
                    if (!role.Butce)
                        isRole = false;
                    break;
                case "FileManagement":
                    if (!role.Dosya)
                        isRole = false;
                    break;
                case "GroupManagement":
                    if (!role.Grup)
                        isRole = false;
                    break;
                case "MissionManagement":
                    if (!role.Gorev)
                        isRole = false;
                    break;
                case "NotificationManagement":
                    if (!role.Bildirim)
                        isRole =false;
                    break;
                case "FactoryManagement":
                    if (!role.Firma)
                        isRole = false;
                    break;
                case "MenuManagement":
                    if (!role.Link)
                        isRole = false;
                    break;
                case "MemberManagement":
                    if (!role.Uye)
                        isRole = false;
                    break;
                case "GradeManagement":
                    if (!role.Seviye)
                        isRole = false;
                    break;
                case "RoleManagement":
                    if (false) //Ayarlanacak
                        isRole = false;
                    break;
                case "DocumentManagement":
                    if (!role.Dokuman)
                        isRole = false;
                    break;
                case "NoteManagement":
                    if (!role.Not)
                        isRole = false;
                    break;
                case "GalleryManagement":
                    if (!role.Galeri)
                        isRole = false;
                    break;
                case "PollManagement":
                    if (!role.Anket)
                        isRole = false;
                    break;
                case "CreatePost":
                    if (!role.YaziYazma)
                        isRole = false;
                    break;
                case "GiveMission":
                    if (!role.GorevVerme)
                        isRole = false;
                    break;
            }
            if (!isRole)
            {
                UrlHelper url = new UrlHelper(filterContext.RequestContext);
                filterContext.Result = new EmptyResult();
                filterContext.Result = new RedirectResult(url.Action("NoRole", "Error", new { area = "" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}