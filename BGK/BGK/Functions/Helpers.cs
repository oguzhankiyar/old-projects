using BGK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace BGK.Functions
{
    public static class Helpers
    {
        public static string ConvertSeo(this string text)
        {
            text = text.Replace("\r\n", " ");
            text = text.Replace("\n", " ");
            text = text.ToLower();
            text = text.Replace("<", "-");
            text = text.Replace(">", "-");
            text = text.Replace(" ", "-");
            text = text.Replace("#", "-");
            text = text.Replace("$", "-dolar-");
            text = text.Replace("€", "-euro-");
            text = text.Replace("%", "-yuzde-");
            text = text.Replace("&", "-ve-");
            text = text.Replace("/", "-");
            text = text.Replace("!", "-");
            text = text.Replace("é", "-");
            text = text.Replace("'", "-");
            text = text.Replace("\"", "-");
            text = text.Replace("^", "-");
            text = text.Replace("+", "-");
            text = text.Replace("(", "-");
            text = text.Replace(")", "-");
            text = text.Replace("=", "-");
            text = text.Replace("?", "-");
            text = text.Replace("*", "-");
            text = text.Replace("_", "-");
            text = text.Replace(".", "");
            text = text.Replace("{", "-");
            text = text.Replace("}", "-");
            text = text.Replace("[", "-");
            text = text.Replace("]", "-");
            text = text.Replace("@", "-");
            text = text.Replace("~", "-");
            text = text.Replace("´", "-");
            text = text.Replace(",", "-");
            text = text.Replace("`", "-");
            text = text.Replace(":", "-");
            text = text.Replace(";", "-");
            text = text.Replace("ğ", "g");
            text = text.Replace("ü", "u");
            text = text.Replace("ş", "s");
            text = text.Replace("ı", "i");
            text = text.Replace("ö", "o");
            text = text.Replace("ç", "c");
            text = text.Replace("---", "-");
            text = text.Replace("--", "-");
            text = text.Trim('-');
            return text;
        }
        public static string GetSummary(this string text, int character)
        {
            if (text != null)
            {
                text = text.Replace("\r\n", " ");
                text = text.Replace("\n", " ");
                text = Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
                if (text.Length > character)
                {
                    if (text.Contains(' '))
                    {
                        string[] split = text.Substring(0, character).Split(' ');
                        text = text.Substring(0, character);
                        text = text.Substring(0, text.Length - split[split.Count() - 1].Length).TrimEnd(' ') + "...";
                    }
                    else
                        text = text.Substring(0, character) + "...";
                }
            }
            return text;
        }
        public static MvcHtmlString DescriptionFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string spanText = metaData.DisplayName ?? metaData.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(spanText))
                return MvcHtmlString.Empty;
            var span = new TagBuilder("i");
            span.SetInnerText(metaData.Description);
            return MvcHtmlString.Create(span.ToString());
        }
        public static MvcHtmlString Button(this HtmlHelper html, string text, object htmlAttributes)
        {
            IDictionary<string, object> Attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            TagBuilder input = new TagBuilder("input");
            input.Attributes.Add("value", text);
            if (Attributes.Keys.Contains("type") == false)
            {
                input.Attributes.Add("type", "button");
            }
            input.MergeAttributes(Attributes);
            return MvcHtmlString.Create(input.ToString());
        }
        public static MvcHtmlString MyEditor(this HtmlHelper html, string id, string toolbar)
        {
            return MvcHtmlString.Create("<script type=\"text/javascript\">CKEDITOR.replace('" + id + "'); CKEDITOR.config.toolbar = '" + toolbar + "';</script>");
        }
        public static MvcHtmlString Paging(this HtmlHelper html)
        {
            UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string action = html.ViewContext.RouteData.Values["action"].ToString();
            string query = HttpContext.Current.Request.QueryString["page"] == null ? HttpContext.Current.Request.QueryString["p"] : HttpContext.Current.Request.QueryString["page"];
            int page = html.ViewContext.RouteData.Values["page"] == null ? html.ViewContext.RouteData.Values["num"] == null ? Convert.ToInt32(query) : Convert.ToInt32(html.ViewContext.RouteData.Values["num"]) : Convert.ToInt32(html.ViewContext.RouteData.Values["page"]);
            page = page == 0 ? 1 : page;
            int take = html.ViewContext.ViewBag.take == null ? 0 : Convert.ToInt32(html.ViewContext.ViewBag.take);
            int count = html.ViewContext.ViewBag.count == null ? 0 : Convert.ToInt32(html.ViewContext.ViewBag.count);
            StringBuilder result = new StringBuilder();
            int pagecount = count % take == 0 ? count / take : count / take + 1;
            if (page > pagecount && page != 1)
            {
                if (pagecount == 0)
                    HttpContext.Current.Response.Redirect(url.Action(action, new { page = 1 }));
                else
                    HttpContext.Current.Response.Redirect(url.Action(action, new { page = pagecount }));
            }
            if (count * take != 0 && take < count)
            {
                int skip = take * (page - 1);
                string first = "<li class=\"first\"><a href=\"" + url.Action(action, new { page = 1 }) + "\" title=\"İlk Sayfa\"><img /></a></li>";
                string prev = "<li class=\"previous\"><a href=\"" + url.Action(action, new { page = page - 1 }) + "\" title=\"Önceki Sayfa\"><img /></a></li>";
                string dot1 = "..";
                string prev1 = "<li><a href=\"" + url.Action(action, new { page = page - 2 }) + "\" title=\"" + Convert.ToInt32(page - 2) + ". Sayfa\">" + Convert.ToInt32(page - 2) + "</a></li>";
                string prev2 = "<li><a href=\"" + url.Action(action, new { page = page - 1 }) + "\" title=\"" + Convert.ToInt32(page - 1) + ". Sayfa\">" + Convert.ToInt32(page - 1) + "</a></li>";
                string active = "<li class=\"active\">" + page + "</li>";
                string next1 = "<li><a href=\"" + url.Action(action, new { page = page + 1 }) + "\" title=\"" + Convert.ToInt32(page + 1) + ". Sayfa\">" + Convert.ToInt32(page + 1) + "</a></li>";
                string next2 = "<li><a href=\"" + url.Action(action, new { page = page + 2 }) + "\" title=\"" + Convert.ToInt32(page + 2) + ". Sayfa\">" + Convert.ToInt32(page + 2) + "</a></li>";
                string dot2 = "..";
                string next = "<li class=\"next\"><a href=\"" + url.Action(action, new { page = page + 1 }) + "\" title=\"Sonraki Sayfa\"><img /></a></li>";
                string last = "<li class=\"last\"><a href=\"" + url.Action(action, new { page = pagecount }) + "\" title=\"Son Sayfa\"><img /></a></li>";
                if (page == 1)
                {
                    first = "";
                    prev = "";
                    dot1 = "";
                    prev1 = "";
                    prev2 = "";
                }
                if (page == pagecount)
                {
                    next1 = "";
                    next2 = "";
                    dot2 = "";
                    next = "";
                    last = "";
                }
                if (pagecount < 3)
                {
                    dot1 = "";
                    if (pagecount == 2)
                    {
                        prev1 = "";
                    }
                    else
                    {
                        prev1 = "";
                        prev2 = "";
                    }
                }
                if (page == 2)
                {
                    dot1 = "";
                    prev1 = "";
                }
                if (page == 3)
                {
                    dot1 = "";
                }
                if (pagecount < page + 2)
                {
                    dot2 = "";
                    if (pagecount < page + 1)
                    {
                        next1 = "";
                        next2 = "";
                    }
                    else
                    {
                        next2 = "";
                    }
                }
                else if (pagecount == page + 2)
                {
                    dot2 = "";
                }
                result.Append("<div class=\"paging\"><ul>Toplam " + count + " öğe, " + pagecount + " sayfa " + first + prev + dot1 + prev1 + prev2 + active + next1 + next2 + dot2 + next + last + "</ul></div>");
            }
            return MvcHtmlString.Create(result.ToString());
        }
        //public static MvcHtmlString Paging(this HtmlHelper html, string onclick, int count, int take)
        //{
        //    string query = HttpContext.Current.Request.QueryString["page"] == null ? HttpContext.Current.Request.QueryString["p"] : HttpContext.Current.Request.QueryString["page"];
        //    int page = html.ViewContext.RouteData.Values["page"] == null ? html.ViewContext.RouteData.Values["num"] == null ? Convert.ToInt32(query) : Convert.ToInt32(html.ViewContext.RouteData.Values["num"]) : Convert.ToInt32(html.ViewContext.RouteData.Values["page"]);
        //    page = page == 0 ? 1 : page;
        //    StringBuilder result = new StringBuilder();
        //    int pagecount = count % take == 0 ? count / take : count / take + 1;
        //    if (page > pagecount && page != 1)
        //    {
        //        if (pagecount == 0)
        //            HttpContext.Current.Response.Redirect(onclick.Replace("window.location.href='", "").Replace("'", "").Replace(";", "").Replace("%7Bpage%7D", "" + 1));
        //        else
        //            HttpContext.Current.Response.Redirect(onclick.Replace("window.location.href='", "").Replace("'", "").Replace(";", "").Replace("%7Bpage%7D", "" + pagecount));
        //    }
        //    if (count != 0 && take < count)
        //    {
        //        int skip = take * (page - 1);
        //        string first = "<li class=\"first\"><a onclick=\"" + onclick.Replace("%7Bpage%7D", "1") + "\" title=\"İlk Sayfa\"><img /></a></li>";
        //        string prev = "<li class=\"previous\"><a onclick=\"" + onclick.Replace("%7Bpage%7D", "" + Convert.ToInt32(page - 1) + "") + "\" title=\"Önceki Sayfa\"><img /></a></li>";
        //        string dot1 = "..";
        //        string prev1 = "<li><a onclick=\"" + onclick.Replace("%7Bpage%7D", "" + Convert.ToInt32(page - 2) + "") + "\" title=\"" + Convert.ToInt32(page - 2) + ". Sayfa\">" + Convert.ToInt32(page - 2) + "</a></li>";
        //        string prev2 = "<li><a onclick=\"" + onclick.Replace("%7Bpage%7D", "" + Convert.ToInt32(page - 1) + "") + "\" title=\"" + Convert.ToInt32(page - 1) + ". Sayfa\">" + Convert.ToInt32(page - 1) + "</a></li>";
        //        string active = "<li class=\"active\">" + page + "</li>";
        //        string next1 = "<li><a onclick=\"" + onclick.Replace("%7Bpage%7D", "" + Convert.ToInt32(page + 1) + "") + "\" title=\"" + Convert.ToInt32(page + 1) + ". Sayfa\">" + Convert.ToInt32(page + 1) + "</a></li>";
        //        string next2 = "<li><a onclick=\"" + onclick.Replace("%7Bpage%7D", "" + Convert.ToInt32(page + 2) + "") + "\" title=\"" + Convert.ToInt32(page + 2) + ". Sayfa\">" + Convert.ToInt32(page + 2) + "</a></li>";
        //        string dot2 = "..";
        //        string next = "<li class=\"next\"><a onclick=\"" + onclick.Replace("%7Bpage%7D", "" + Convert.ToInt32(page + 1) + "") + "\" title=\"Sonraki Sayfa\"><img /></a></li>";
        //        string last = "<li class=\"last\"><a onclick=\"" + onclick.Replace("%7Bpage%7D", "" + pagecount + "") + "\" title=\"Son Sayfa\"><img /></a></li>";
        //        if (page == 1)
        //        {
        //            first = "";
        //            prev = "";
        //            dot1 = "";
        //            prev1 = "";
        //            prev2 = "";
        //        }
        //        if (page == pagecount)
        //        {
        //            next1 = "";
        //            next2 = "";
        //            dot2 = "";
        //            next = "";
        //            last = "";
        //        }
        //        if (pagecount < 3)
        //        {
        //            dot1 = "";
        //            if (pagecount == 2)
        //            {
        //                prev1 = "";
        //            }
        //            else
        //            {
        //                prev1 = "";
        //                prev2 = "";
        //            }
        //        }
        //        if (page == 2)
        //        {
        //            dot1 = "";
        //            prev1 = "";
        //        }
        //        if (page == 3)
        //        {
        //            dot1 = "";
        //        }
        //        if (pagecount < page + 2)
        //        {
        //            dot2 = "";
        //            if (pagecount < page + 1)
        //            {
        //                next1 = "";
        //                next2 = "";
        //            }
        //            else
        //            {
        //                next2 = "";
        //            }
        //        }
        //        else if (pagecount == page + 2)
        //        {
        //            dot2 = "";
        //        }
        //        result.Append("<div class=\"paging\"><ul>Toplam " + count + " öğe, " + pagecount + " sayfa " + first + prev + dot1 + prev1 + prev2 + active + next1 + next2 + dot2 + next + last + "</ul></div>");
        //    }
        //    return MvcHtmlString.Create(result.ToString());
        //}
        public static MvcHtmlString ShortDateTime(this HtmlHelper html, DateTime datetime, bool isshort = false)
        {
            string result = "";
            TimeSpan ts = DateTime.Now - datetime;
            int minute = (int)ts.TotalMinutes;
            int hour = (int)ts.TotalHours;
            int day = (int)ts.TotalDays; // DateTime.Now.Day - datetime.Day;
            int month = day / 30;
            int year = day / 365;
            if (datetime > DateTime.Now)
                result = datetime.ToString("d MMMMMMM dddd, HH:mm");
            else if (day == 1)
                result = "Dün " + datetime.ToString("HH:mm");
            else if (year >= 1)
                result = year + " yıl önce";
            else if (month >= 1)
                result = month + " ay önce";
            else if (day > 7)
                result = day + " gün önce";
            else if (minute < 1)
                result = "Birkaç saniye önce";
            else if (minute < 60)
                result = minute + " dakika önce";
            else if (hour < 24)
                result = "Bugün " + datetime.ToString("HH:mm");
            else if (day <= 7)
                result = "Geçen hafta";
            else
                result = datetime.ToString("d MMMM yyyy, HH:mm");
            if (isshort)
                return MvcHtmlString.Create("<text title=\"" + (year == 0 ? datetime.ToString("d MMMMMMM dddd, HH:mm") : datetime.ToString("d MMMMMMM yyyy dddd, HH:mm")) + "\">" + result +"</text>");
            else
                return MvcHtmlString.Create("<text title=\"" + result + "\">" + (year == 0 ? datetime.ToString("d MMMMMMM dddd") : datetime.ToString("d MMMMMMM yyyy dddd")) + "</text>");
        }
        public static bool IsOnline(this bgk_uye member)
        {
            TimeSpan ts = DateTime.Now - member.SonGiris;
            if (ts.Days == 0 && ts.TotalDays >= 0 && ts.TotalMinutes <= 1)
                return true;
            return false;
        }
        public static MvcHtmlString GetMemberState(this HtmlHelper html, int memberID)
        {
            BGKEntities Db = new BGKEntities();
            bgk_uye member = Db.bgk_uye.Find(memberID);
            if (member.IsOnline())
                return MvcHtmlString.Create("<p class=\"online-member\">&nbsp;</p>");
            else
                return MvcHtmlString.Create("<p class=\"offline-member\">&nbsp;</p>");
        }
        public static MvcHtmlString GetMemberCard(this bgk_uye member)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div style=\"display: none; background-color: #fff; padding: 5px; width: 150px; height: 75px; border: 1px solid #ccc\">" +
                      "<b>@member.AdSoyad</b><br />" +
                      member.GetMemberRole().Adi + "<br />" +
                      BGKFunction.GetGrade(member.Puan) +
                      "</div>" + member.AdSoyad);
            return MvcHtmlString.Create(sb.ToString());
        }
        public static bgk_yetki GetMemberRole(this bgk_uye member)
        {
            BGKEntities Db = new BGKEntities();
            return Db.bgk_yetki.SingleOrDefault(x => x.Kod == member.Yetki);
        }
        public static IEnumerable<bgk_uye> GetMembers(this bgk_yetki role)
        {
            BGKEntities Db = new BGKEntities();
            return Db.bgk_uye.Where(x => x.Yetki == role.Kod);
        }
    }
}