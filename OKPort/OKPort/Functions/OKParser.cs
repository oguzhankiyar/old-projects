#define PageIDTest
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using System.Xml.Linq;
using OKPort.Models;

namespace OKPort.Functions
{
    public class OKParser
    {
        string result = string.Empty;
        OKDbEntities db = new OKDbEntities();
        Page page;

        public static string GetHtml(string code)
        {
            if (string.IsNullOrEmpty(code))
                return string.Empty;
            OKParser parser = new OKParser();
            code = code.Replace("\n", "").Replace("\r", "");
            code = HttpUtility.HtmlDecode(code);
            parser.result = code;
            parser.ParseGeneralTags();
            parser.Parse(parser.result);
            return parser.result;
        }

        private void ParseGeneralTags()
        {
            var pages = db.Pages.ToList();
            page = pages.SingleOrDefault(x => x.ShortURL == GetRouteValue("pageURL"));

#if PageIDTest
            if (page == null)
                page = pages.SingleOrDefault(x => x.ID == 2);
#endif

            result = result.Replace("{Page.Image}", page.ImageURL.ToString());
            result = result.Replace("{Page.Name}", page.Name.ToString());
            result = result.Replace("{Page.Description}", page.Description.ToString());
            result = result.Replace("{Url(Page)}", "/" + page.ShortURL.ToString());

            result = result.Replace("{Url.PageUrl}", GetRouteValue("pageURL"));
            result = result.Replace("{Url.SectionUrl}", GetRouteValue("sectionURL"));
            result = result.Replace("{Url.CategoryUrl}", GetRouteValue("categoryURL"));
            result = result.Replace("{Url.PostUrl}", GetRouteValue("postURL"));

            result = result.Replace("{Current.PageId}", Function.GetPageId(GetRouteValue("PageURL")).ToString());
            result = result.Replace("{Current.SectionId}", Function.GetSectionId(GetRouteValue("SectionURL")).ToString());
            result = result.Replace("{Current.CategoryId}", Function.GetCategoryId(GetRouteValue("CategoryURL")).ToString());
            result = result.Replace("{Current.PostId}", Function.GetPostId(GetRouteValue("PostId")).ToString());
        }

        private string GetRouteValue(string key)
        {
            RouteData routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current));
            if (routeData.Values[key] == null)
                return string.Empty;
            return routeData.Values[key].ToString();
        }

        private void Parse(string code)
        {
            if (string.IsNullOrEmpty(code))
                return;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<root>" + code + "</root>");
                XmlNodeList nodes = doc.GetElementsByTagName("root")[0].ChildNodes;
                foreach (XmlNode node in nodes)
                {
                    string inner = string.Empty;
                    switch (node.Name)
                    {
                        case "widgets":
                            inner = ParseWidgetList(node);
                            break;
                        case "box":
                            inner = ParseBox(node);
                            break;
                        case "links":
                            inner = ParseLinkList(node);
                            break;
                        case "sections":
                            inner = ParseSectionList(node);
                            break;
                        case "categories":
                            inner = ParseCategoryList(node);
                            break;
                        case "posts":
                            inner = ParsePostList(node);
                            break;
                        case "comments":
                            inner = ParseCommentList(node);
                            break;
                        case "ratings":
                            inner = ParseRatingList(node);
                            break;
                        case "subscriptions":
                            inner = ParseSubscriptionList(node);
                            break;
                        case "section":
                            inner = ParseSection(node);
                            break;
                        case "category":
                            inner = ParseCategory(node);
                            break;
                        case "post":
                            inner = ParsePost(node);
                            break;
                        case "#text":
                            break;
                        default:
                            inner = node.InnerXml;
                            break;
                    }
                    if (!GetIfConditionValue(GetPropertyValue(node, "if")))
                        result = result.Replace(node.OuterXml, string.Empty);
                    else if (node.Name != "#text")
                        Parse(inner);
                }
            }
            catch (Exception)
            {
                result = "Syntax Error!";
            }
        }

        private bool GetIfConditionValue(string value)
        {
            if (value == null || value == "1")
                return true;
            string value1, value2;
            if (value.Contains("=="))
            {
                value = value.Replace("==", "|");
                value1 = value.Split('|')[0].Trim();
                value2 = value.Split('|')[1].Trim();
                return (value1 == value2);
            }
            else if (value.Contains("!="))
            {
                value = value.Replace("!=", "|");
                value1 = value.Split('|')[0].Trim();
                value2 = value.Split('|')[1].Trim();
                return (value1 != value2);
            }
            else if (value.Contains("<="))
            {
                value = value.Replace("<=", "|");
                value1 = value.Split('|')[0].Trim();
                value2 = value.Split('|')[1].Trim();
                return (Convert.ToInt32(value1) <= Convert.ToInt32(value2));
            }
            else if (value.Contains(">="))
            {
                value = value.Replace(">=", "|");
                value1 = value.Split('|')[0].Trim();
                value2 = value.Split('|')[1].Trim();
                return (Convert.ToInt32(value1) >= Convert.ToInt32(value2));
            }
            else if (value.Contains("<"))
            {
                value = value.Replace("<", "|");
                value1 = value.Split('|')[0].Trim();
                value2 = value.Split('|')[1].Trim();
                return (Convert.ToInt32(value1) < Convert.ToInt32(value2));
            }
            else if (value.Contains(">"))
            {
                value = value.Replace(">", "|");
                value1 = value.Split('|')[0].Trim();
                value2 = value.Split('|')[1].Trim();
                return (Convert.ToInt32(value1) > Convert.ToInt32(value2));
            }
            return false;
        }

        private string ParseBox(XmlNode node)
        {
            string inner = "<div class=\"box\">" + node.InnerXml + "</div>";
            result = result.Replace(node.OuterXml, inner);
            return inner;
        }

        private string ParseLinkList(XmlNode node)
        {
            string inner = string.Empty;
            var links = page.Links.Where(x => x.IsApproval).ToList();

            int skip;
            if (Int32.TryParse(GetPropertyValue(node, "skip"), out skip))
                links = links.Skip(skip).ToList();

            int take;
            if (Int32.TryParse(GetPropertyValue(node, "take"), out take))
                links = links.Take(take).ToList();

            string order = GetPropertyValue(node, "order");
            if (order == "sort")
                links = links.OrderBy(x => x.Sort).ToList();
            else if (order == "!sort")
                links = links.OrderByDescending(x => x.Sort).ToList();
            else if (order == "!name")
                links = links.OrderByDescending(x => x.Name).ToList();
            else
                links = links.OrderBy(x => x.Name).ToList();


            foreach (var link in links)
                inner += GetLinkInner(node.InnerXml, link);

            result = result.Replace(node.OuterXml, inner);
            return inner;
        }

        private string ParseSubscriptionList(XmlNode node)
        {
            string inner = string.Empty;
            var subscriptions = page.Subscriptions.ToList();

            int skip;
            if (Int32.TryParse(GetPropertyValue(node, "skip"), out skip))
                subscriptions = subscriptions.Skip(skip).ToList();

            int take;
            if (Int32.TryParse(GetPropertyValue(node, "take"), out take))
                subscriptions = subscriptions.Take(take).ToList();

            string order = GetPropertyValue(node, "order");
            if (order == "subscriptionDate")
                subscriptions = subscriptions.OrderBy(x => x.SubscriptionDate).ToList();
            else if (order == "!subscriptionDate")
                subscriptions = subscriptions.OrderByDescending(x => x.SubscriptionDate).ToList();
            else if (order == "name")
                subscriptions = subscriptions.OrderBy(x => x.User.FirstName).ToList();
            else
                subscriptions = subscriptions.OrderByDescending(x => x.User.FirstName).ToList();

            foreach (var subscription in subscriptions)
                inner += GetSubscriptionInner(node.InnerXml, subscription);

            result = result.Replace(node.OuterXml, inner);
            return inner;
        }

        private string ParseWidgetList(XmlNode node)
        {
            string inner = string.Empty;
            var widgets = db.PageWidgets.Where(x => x.PageID == page.ID).OrderByDescending(x => x.ID).ToList();

            string section = GetPropertyValue(node, "section");
            if (section != null)
            {
                if (section == "home")
                    widgets = widgets.Where(x => x.SectionID == null).ToList();
                else
                    widgets = widgets.Where(x => (x.Section != null && x.Section.ShortURL == section)).ToList();
            }

            string region = GetPropertyValue(node, "region");
            if (region != null)
                widgets = widgets.Where(x => x.Region == region).ToList();

            string position = GetPropertyValue(node, "position");
            if (position != null)
                widgets = widgets.Where(x => x.Position.ToString() == position).ToList();

            int skip;
            if (Int32.TryParse(GetPropertyValue(node, "skip"), out skip))
                widgets = widgets.Skip(skip).ToList();

            int take;
            if (Int32.TryParse(GetPropertyValue(node, "take"), out take))
                widgets = widgets.Take(take).ToList();

            widgets = widgets.OrderBy(x => x.Sort).ToList();

            foreach (var widget in widgets)
                inner += GetWidgetInner(node.InnerXml, widget.Widget);

            result = result.Replace(node.OuterXml, inner);
            return inner;
        }

        private string ParseRatingList(XmlNode node)
        {
            string inner = string.Empty;
            var ratings = db.Ratings.Where(x => x.Post.Category.Section.PageID == page.ID).ToList();

            string post = GetPropertyValue(node, "post");
            if (post != null)
                ratings = ratings.Where(x => x.PostID.ToString() == post).ToList();

            int skip;
            if (Int32.TryParse(GetPropertyValue(node, "skip"), out skip))
                ratings = ratings.Skip(skip).ToList();

            int take;
            if (Int32.TryParse(GetPropertyValue(node, "take"), out take))
                ratings = ratings.Take(take).ToList();

            string order = GetPropertyValue(node, "order");
            if (order == "actionDate")
                ratings = ratings.OrderBy(x => x.ActionDate).ToList();
            else
                ratings = ratings.OrderByDescending(x => x.ActionDate).ToList();

            foreach (var rating in ratings)
                inner += GetRatingInner(node.InnerXml, rating);

            result = result.Replace(node.OuterXml, inner);
            return inner;
        }

        private string ParseCommentList(XmlNode node)
        {
            string inner = string.Empty;
            var comments = db.Comments.Where(x => x.Post.Category.Section.PageID == page.ID && x.IsApproval).ToList();

            string post = GetPropertyValue(node, "post");
            if (post != null)
                comments = comments.Where(x => x.PostID.ToString() == post).ToList();

            int skip;
            if (Int32.TryParse(GetPropertyValue(node, "skip"), out skip))
                comments = comments.Skip(skip).ToList();

            int take;
            if (Int32.TryParse(GetPropertyValue(node, "take"), out take))
                comments = comments.Take(take).ToList();

            string order = GetPropertyValue(node, "order");
            if (order == "popular")
                comments = comments.OrderBy(x => CommentFunctions.GetPopularPoint(x)).ToList();
            else if (order == "!popular")
                comments = comments.OrderByDescending(x => CommentFunctions.GetPopularPoint(x)).ToList();
            else if (order == "!writingDate")
                comments = comments.OrderBy(x => x.WritingDate).ToList();
            else
                comments = comments.OrderByDescending(x => x.WritingDate).ToList();

            foreach (var comment in comments)
                inner += GetCommentInner(node.InnerXml, comment);

            result = result.Replace(node.OuterXml, inner);
            return inner;
        }

        private string ParsePostList(XmlNode node)
        {
            string inner = string.Empty;
            var posts = db.Posts.Where(x => x.Category.Section.PageID == page.ID && x.IsApproval).ToList();

            string category = GetPropertyValue(node, "category");
            if (category != null)
                posts = posts.Where(x => x.CategoryID.ToString() == category).ToList();

            string user = GetPropertyValue(node, "user");
            if (user != null)
                posts = posts.Where(x => x.WriterID.ToString() == user).ToList();

            int skip;
            if (Int32.TryParse(GetPropertyValue(node, "skip"), out skip))
                posts = posts.Skip(skip).ToList();

            int take;
            if (Int32.TryParse(GetPropertyValue(node, "take"), out take))
                posts = posts.Take(take).ToList();

            string order = GetPropertyValue(node, "order");
            if (order == "title")
                posts = posts.OrderBy(x => x.Title).ToList();
            else if (order == "!title")
                posts = posts.OrderByDescending(x => x.Title).ToList();
            else if (order == "!creationDate")
                posts = posts.OrderBy(x => x.CreationDate).ToList();
            else
                posts = posts.OrderByDescending(x => x.CreationDate).ToList();

            foreach (var post in posts)
                inner += GetPostInner(node.InnerXml, post);

            result = result.Replace(node.OuterXml, inner);
            return inner;
        }

        private string ParsePost(XmlNode node)
        {
            Post post = null;

            int id;
            if (Int32.TryParse(GetPropertyValue(node, "id"), out id))
                post = db.Posts.Find(id);

            string url = GetPropertyValue(node, "url");
            if (url != null)
                post = db.Posts.SingleOrDefault(x => x.ShortURL == url);
            
            string inner = GetPostInner(node.InnerXml, post);
            result = result.Replace(node.OuterXml, inner);
            return inner;
        }

        private string ParseCategory(XmlNode node)
        {
            Category category = null;

            int id;
            if (Int32.TryParse(GetPropertyValue(node, "id"), out id))
                category = db.Categories.Find(id);

            string url = GetPropertyValue(node, "url");
            if (url != null)
                category = db.Categories.SingleOrDefault(x => x.ShortURL == url);
            
            string inner = GetCategoryInner(node.InnerXml, category);
            result = result.Replace(node.OuterXml, inner);        
            return inner;
        }

        private string ParseSection(XmlNode node)
        {
            Section section = null;

            int id;
            if (Int32.TryParse(GetPropertyValue(node, "id"), out id))
                section = db.Sections.Find(id);

            string url = GetPropertyValue(node, "url");
            if (url != null)
                section = db.Sections.SingleOrDefault(x => x.ShortURL == url);
            
            string inner = GetSectionInner(node.InnerXml, section);
            result = result.Replace(node.OuterXml, inner);
            return inner;
        }

        private string ParseCategoryList(XmlNode node)
        {
            string inner = string.Empty;
            var categories = db.Categories.Where(x => x.Section.PageID == page.ID && x.IsApproval).ToList();

            // TODO: eğer numerik bir değer ise id olarak ara, numerik değilse shortURL olarak ara
            string section = GetPropertyValue(node, "section");
            if (section != null)
                categories = categories.Where(x => x.SectionID.ToString() == section).ToList();

            string parent = GetPropertyValue(node, "parent");
            if (parent != null)
                categories = categories.Where(x => x.ParentID.ToString() == parent).ToList();

            int skip;
            if (Int32.TryParse(GetPropertyValue(node, "skip"), out skip))
                categories = categories.Skip(skip).ToList();

            int take;
            if (Int32.TryParse(GetPropertyValue(node, "take"), out take))
                categories = categories.Take(take).ToList();

            string order = GetPropertyValue(node, "order");
            if (order == "sort")
                categories = categories.OrderBy(x => x.Sort).ToList();
            else if (order == "!sort")
                categories = categories.OrderByDescending(x => x.Sort).ToList();
            else if (order == "!name")
                categories = categories.OrderByDescending(x => x.Name).ToList();
            else
                categories = categories.OrderBy(x => x.Name).ToList();

            foreach (var category in categories)
            {
                if (parent != null)
                    inner += GetSubCategoryInner(node.InnerXml, category);
                else
                    inner += GetCategoryInner(node.InnerXml, category);
            }

            result = result.Replace(node.OuterXml, inner);
            return inner;
        }

        private string ParseSectionList(XmlNode node)
        {
            string inner = string.Empty;
            var sections = page.Sections.Where(x => x.IsApproval).ToList();

            int skip;
            if (Int32.TryParse(GetPropertyValue(node, "skip"), out skip))
                sections = sections.Skip(skip).ToList();

            int take;
            if (Int32.TryParse(GetPropertyValue(node, "take"), out take))
                sections = sections.Take(take).ToList();

            foreach (var section in sections)
                inner += GetSectionInner(node.InnerXml, section);

            result = result.Replace(node.OuterXml, inner);
            return inner;
        }
        
        private string GetPropertyValue(XmlNode node, string propertyName)
        {
            if (node.Name == "#text")
                return null;
            if (node.Attributes[propertyName] == null)
                return null;
            if (node.Attributes[propertyName].Value == "")
                return null;
            return node.Attributes[propertyName].Value;
        }

        private string GetWidgetInner(string inner, Widget widget)
        {
            if (widget == null)
                return string.Empty;
            /*
            result = result.Replace("{Widget}", GetHtml(widget.Content));
            result = result.Replace("{Widget.Name}", widget.Title);
            */

            inner = inner.Replace("{Widget}", GetHtml(widget.Content));
            inner = inner.Replace("{Widget.Name}", widget.Title);
            return inner;
        }

        private string GetSectionInner(string inner, Section section)
        {
            if (section == null)
                return string.Empty;
            /*
            result = result.Replace("{Section.Name}", section.Name);
            result = result.Replace("{Section.Id}", section.ID.ToString());
            result = result.Replace("{Section.Category.Count}", section.Categories.Count().ToString());
            result = result.Replace("{Url(Section)}", section.Page.ShortURL + "/" + section.ShortURL);
            */
            inner = inner.Replace("{Section.Name}", section.Name);
            inner = inner.Replace("{Section.Id}", section.ID.ToString());
            inner = inner.Replace("{Section.Category.Count}", section.Categories.Count().ToString());
            inner = inner.Replace("{Url(Section)}", "/" + section.Page.ShortURL + "/" + section.ShortURL);
            return inner;
        }

        private string GetCategoryInner(string inner, Category category)
        {
            if (category == null)
                return string.Empty;
            /*
            result = result.Replace("{Category.Name}", category.Name);
            result = result.Replace("{Category.Id}", category.ID.ToString());
            result = result.Replace("{Category.Post.Count}", category.Posts.Count().ToString());
            result = result.Replace("{Url(Category)}", category.Section.Page.ShortURL + "/" + category.Section.ShortURL + "/" + category.ShortURL);
            */
            inner = inner.Replace("{Category.Name}", category.Name);
            inner = inner.Replace("{Category.Id}", category.ID.ToString());
            inner = inner.Replace("{Category.Post.Count}", category.Posts.Count().ToString());
            inner = inner.Replace("{Url(Category)}", "/" + category.Section.Page.ShortURL + "/" + category.Section.ShortURL + "/" + category.ShortURL);
            return inner;
        }

        private string GetSubCategoryInner(string inner, Category subCategory)
        {
            if (subCategory == null)
                return string.Empty;
            var category = db.Categories.Find(subCategory.ParentID);
            /*
            result = result.Replace("{SubCategory.Name}", subCategory.Name);
            result = result.Replace("{SubCategory.Id}", subCategory.ID.ToString());
            result = result.Replace("{SubCategory.Post.Count}", subCategory.Posts.Count().ToString());
            result = result.Replace("{Url(SubCategory)}", subCategory.Section.Page.ShortURL + "/" + subCategory.Section.ShortURL + "/" + category.ShortURL + "/" + subCategory.ShortURL);
            */

            inner = inner.Replace("{SubCategory.Name}", subCategory.Name);
            inner = inner.Replace("{SubCategory.Id}", subCategory.ID.ToString());
            inner = inner.Replace("{SubCategory.Post.Count}", subCategory.Posts.Count().ToString());
            inner = inner.Replace("{Url(SubCategory)}", "/" + subCategory.Section.Page.ShortURL + "/" + subCategory.Section.ShortURL + "/" + category.ShortURL + "/" + subCategory.ShortURL);
            return inner;
        }

        private string GetPostInner(string inner, Post post)
        {
            if (post == null)
                return string.Empty;
            
            /*
            result = result.Replace("{Post.Title}", post.Title);
            result = result.Replace("{Post.Content}", post.Content);
            result = result.Replace("{Post.Id}", post.ID.ToString());
            result = result.Replace("{Post.Comment.Count}", post.Comments.Count().ToString());
            result = result.Replace("{Url(Post)}", post.Category.Section.Page.ShortURL + "/" + post.Category.Section.ShortURL + "/" + post.Category.ShortURL + "/" + post.ShortURL);
            */

            inner = inner.Replace("{Post.Title}", post.Title);
            inner = inner.Replace("{Post.Content}", post.Content);
            inner = inner.Replace("{Post.Id}", post.ID.ToString());
            inner = inner.Replace("{Post.Comment.Count}", post.Comments.Count().ToString());
            inner = inner.Replace("{Url(Post)}", "/" + post.Category.Section.Page.ShortURL + "/" + post.Category.Section.ShortURL + "/" + post.Category.ShortURL + "/" + post.ShortURL);
            return inner;
        }

        private string GetCommentInner(string inner, Comment comment)
        {
            if (comment == null)
                return string.Empty;
            /*
            result = result.Replace("{Comment.Content}", comment.Content);
            result = result.Replace("{Comment.Id}", comment.ID.ToString());
            */

            inner = inner.Replace("{Comment.Content}", comment.Content);
            inner = inner.Replace("{Comment.Id}", comment.ID.ToString());
            return inner;
        }

        private string GetRatingInner(string inner, Rating rating)
        {
            /*
            result = result.Replace("{Rating.User.Name}", rating.User.FirstName);
            */

            inner = inner.Replace("{Rating.User.Name}", rating.User.FirstName);
            return inner;
        }

        private string GetLinkInner(string inner, Link link)
        {
            /*
            result = result.Replace("{Link.Name}", link.Name);
            result = result.Replace("{Link.Address}", link.Address.ToString());
            result = result.Replace("{Link.Target}", link.Target.ToString());
            */

            inner = inner.Replace("{Link.Name}", link.Name);
            //inner = inner.Replace("{Link.Address}", link.Address.ToString());
            inner = inner.Replace("{Link.Target}", link.Target.ToString());
            inner = inner.Replace("{Url(Link)}", "/" + page.ShortURL + link.Address.ToString());
            return inner;
        }

        private string GetSubscriptionInner(string inner, Subscription subscription)
        {
            /*
            result = result.Replace("{Subscription.User.Name}", subscription.User.FirstName);
            result = result.Replace("{Subscription.Date}", subscription.SubscriptionDate.ToShortDateString());
            */

            inner = inner.Replace("{Subscription.User.Name}", subscription.User.FirstName);
            inner = inner.Replace("{Subscription.Date}", subscription.SubscriptionDate.ToShortDateString());
            return inner;
        }
    }
}