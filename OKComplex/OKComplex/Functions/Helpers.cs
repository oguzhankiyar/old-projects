using OKComplex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OKComplex.Functions
{
    public static class Helpers
    {
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
        public static MvcHtmlString ViewTopic(this HtmlHelper html, string content)
        {
            return MvcHtmlString.Create(content.ToString());
        }
        public static MvcHtmlString ViewComment(this HtmlHelper html, comment comment)
        {
            UrlHelper url = new UrlHelper(html.ViewContext.RequestContext, html.RouteCollection);
            string result = "";
            string quote = comment.QuoteId;
            if (!string.IsNullOrEmpty(quote))
            {
                OKDbEntities Db = new OKDbEntities();
                int id = Convert.ToInt32(quote.Split('-')[1]);
                if (quote.Split('-')[0] == "t")
                {
                    var quotetopic = Db.topic.Find(id);
                    result = "<div style=\"max-height: 300px; overflow-x: hidden; overflow-y: scroll;\"><blockquote><b>" + OK.UserName(quotetopic.user) + "</b>, <small>" + html.ShortDateTime((DateTime)quotetopic.CreationDate) + "</small><small style=\"float: right;\"><a href=\"" + url.Action("GoTopic", new { id = quotetopic.Id }) + "\">Konuya Git</a></small><div class=\"comment\">" + html.ViewTopic(quotetopic.Content) + "</blockquote></div>";
                }
                else
                {
                    var quotecomment = Db.comment.Find(id);
                    result = "<div style=\"max-height: 300px; overflow-x: hidden; overflow-y: scroll;\"><blockquote><b>" + OK.UserName(quotecomment.user) + "</b>, <small>" + html.ShortDateTime((DateTime)quotecomment.CreationDate) + "</small><small style=\"float: right;\"><a href=\"" + url.Action("GoComment", new { id = quotecomment.Id }) + "\">Cevaba Git</a></small><div class=\"comment\">" + html.ViewComment(quotecomment) + "</blockquote></div>";
                }
            }
            result += comment.Content;
            return MvcHtmlString.Create(result.ToString());
        }
        public static MvcHtmlString MyEditor(this HtmlHelper html, string id, string toolbar)
        {
            return MvcHtmlString.Create("<script type=\"text/javascript\">CKEDITOR.replace('" + id + "'); CKEDITOR.config.toolbar = '" + toolbar + "';</script>");
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
        public static MvcHtmlString Paging(this HtmlHelper html, string onclick, int count, int take)
        {
            string query = HttpContext.Current.Request.QueryString["page"] == null ?  HttpContext.Current.Request.QueryString["p"] : HttpContext.Current.Request.QueryString["page"];
            int page = html.ViewContext.RouteData.Values["page"] == null ? html.ViewContext.RouteData.Values["num"] == null ? Convert.ToInt32(query) : Convert.ToInt32(html.ViewContext.RouteData.Values["num"]) : Convert.ToInt32(html.ViewContext.RouteData.Values["page"]);
            page = page == 0 ? 1 : page;
            StringBuilder result = new StringBuilder();
            int pagecount = count % take == 0 ? count / take : count / take + 1;
            if (page > pagecount && page != 1)
            {
                if (pagecount == 0)
                    HttpContext.Current.Response.Redirect(onclick.Replace("window.location.href='", "").Replace("'", "").Replace("%7Bpage%7D", "" + 1));
                else
                    HttpContext.Current.Response.Redirect(onclick.Replace("window.location.href='", "").Replace("'", "").Replace("%7Bpage%7D", "" + pagecount));
            }
            if (count != 0 && take < count)
            {
                int skip = take * (page - 1);
                string first = "<li class=\"first\"><a onclick=\"" + onclick.Replace("%7Bpage%7D", "1") + "\" title=\"İlk Sayfa\"><img /></a></li>";
                string prev = "<li class=\"previous\"><a onclick=\"" + onclick.Replace("%7Bpage%7D", "" + Convert.ToInt32(page - 1) + "") + "\" title=\"Önceki Sayfa\"><img /></a></li>";
                string dot1 = "..";
                string prev1 = "<li><a onclick=\"" + onclick.Replace("%7Bpage%7D", "" + Convert.ToInt32(page - 2) + "") + "\" title=\"" + Convert.ToInt32(page - 2) + ". Sayfa\">" + Convert.ToInt32(page - 2) + "</a></li>";
                string prev2 = "<li><a onclick=\"" + onclick.Replace("%7Bpage%7D", "" + Convert.ToInt32(page - 1) + "") + "\" title=\"" + Convert.ToInt32(page - 1) + ". Sayfa\">" + Convert.ToInt32(page - 1) + "</a></li>";
                string active = "<li class=\"active\">" + page + "</li>";
                string next1 = "<li><a onclick=\"" + onclick.Replace("%7Bpage%7D", "" + Convert.ToInt32(page + 1) + "") + "\" title=\"" + Convert.ToInt32(page + 1) + ". Sayfa\">" + Convert.ToInt32(page + 1) + "</a></li>";
                string next2 = "<li><a onclick=\"" + onclick.Replace("%7Bpage%7D", "" + Convert.ToInt32(page + 2) + "") + "\" title=\"" + Convert.ToInt32(page + 2) + ". Sayfa\">" + Convert.ToInt32(page + 2) + "</a></li>";
                string dot2 = "..";
                string next = "<li class=\"next\"><a onclick=\"" + onclick.Replace("%7Bpage%7D", "" + Convert.ToInt32(page + 1) + "") + "\" title=\"Sonraki Sayfa\"><img /></a></li>";
                string last = "<li class=\"last\"><a onclick=\"" + onclick.Replace("%7Bpage%7D", "" + pagecount + "") + "\" title=\"Son Sayfa\"><img /></a></li>";
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
        public static MvcHtmlString UserInfo(this HtmlHelper html, int id)
        {
            OKDbEntities Db = new OKDbEntities();
            string result = "";
            UrlHelper url = new UrlHelper(html.ViewContext.RequestContext, html.RouteCollection);
            var user = Db.user.Single(x => x.Id == id);
            result = "<div class=\"image\" onclick=\"$.OK.Modal('" + url.Action("UserInfo", "Account", new { id = id }) + "');\"><img src=\"" + user.ImageFile + "\" /></div>" +
                     "<div class=\"name\" onclick=\"$.OK.Modal('" + url.Action("UserInfo", "Account", new { id = id }) + "');\">" + OK.UserName(user) + "<img src=\"" + url.Content("~/Areas/Forum/Themes/" + OK.Config("site-theme") + "/Images/" + OK.UserState(user.Id, (DateTime)user.LastLoginDate) + ".png") + "\" /></div>" +
                     "<div class=\"level\">" + user.type.Name + "</div>" +
                     "<div class=\"rating\">" + html.UserRating((int)user.Rating) + "</div>";
            return MvcHtmlString.Create("<div class=\"user-info\">" + result + "</div>");
        }
        public static MvcHtmlString UserRating(this HtmlHelper html, int rating)
        {
            string result = "";
            int rate = OK.UserRating(rating);
            for (int i = 0; i < rate; i++)
            {
                result += "<span style=\"background-position: 0 -32px;\"> </span>";
            }
            for (int i = 0; i < 5 - rate; i++)
            {
                result += "<span> </span>";
            }
            return MvcHtmlString.Create(result);
        }
        public static MvcHtmlString LikeTable(this HtmlHelper html, int id, string type, string rating, int userid)
        {
            UrlHelper url = new UrlHelper(html.ViewContext.RequestContext, html.RouteCollection);
            string result = "";
            result += "<div class=\"bars\"><div class=\"like\" title=\"" + OK.TopicPoint(rating, 3) + "%\" style=\"width: " + 180 * OK.TopicPoint(rating, 3) / 100 + "px;\"></div><div class=\"dislike\" title=\"" + OK.TopicPoint(rating, 4) + "%\" style=\"width: " + 180 * OK.TopicPoint(rating, 4) / 100 + "px;\"></div></div>";
            string msg = "";
            if (OK.TopicPoint(rating, 1) + OK.TopicPoint(rating, 2) == 0)
            {
                msg = "İlk oylayan sen ol";
            }
            else if (OK.RatingScore(id, type, userid) == 0)
            {
                msg = "Bunu değerlendir";
            }
            else if (OK.RatingScore(id, type, userid) == 1)
            {
                msg = "Bunu beğendin";
            }
            else if (OK.RatingScore(id, type, userid) == -1)
            {
                msg = "Bunu beğenmedin";
            }
            result += "<div class=\"info\"><div class=\"like-b\" title=\"Beğen\" onclick=\"UpdateRating('" + id + "', '+1', '" + type + "');\">&nbsp;</div><div class=\"like-c\">" + OK.TopicPoint(rating, 1) + "</div><div class=\"msg\">" + msg + "</div><div class=\"dislike-b\" title=\"Beğenme\" onclick=\"UpdateRating('" + id + "', '-1', '" + type + "');\">&nbsp;</div><div class=\"dislike-c\">" + OK.TopicPoint(rating, 2) + "</div></div>";
            return MvcHtmlString.Create(result);
        }
        public static MvcHtmlString ShortDateTime(this HtmlHelper html, DateTime datetime)
        {
            string result = "";
            TimeSpan ts = DateTime.Now - datetime;
            int minute = (int)ts.TotalMinutes;
            int hour = (int)ts.TotalHours;
            int day = (int)ts.TotalDays; // DateTime.Now.Day - datetime.Day;
            int month = day / 30;
            int year = day / 365;
            if (day == 1)
            {
                result = "Dün " + datetime.ToString("HH:mm");
            }
            else if (year >= 1)
            {
                result = year + " yıl önce";
            }
            else if (month >= 1)
            {
                result = month + " ay önce";
            }
            else if (day > 7)
            {
                result = day + " gün önce";
            }
            else if (minute < 1)
            {
                result = "Birkaç saniye önce";
            }
            else if (minute < 60)
            {
                result = minute + " dakika önce";
            }
            else if (hour < 24)
            {
                result = "Bugün " + datetime.ToString("HH:mm");
            }
            else if (day <= 7)
            {
                result = "Geçen hafta";
            }
            else
            {
                result = datetime.ToString("d MMMM yyyy, HH:mm");
            }
            return MvcHtmlString.Create("<text title=\"" + datetime.ToString("d MMMM yyyy dddd, HH:mm") + "\">" + result + "</text>");
        }
    }
}