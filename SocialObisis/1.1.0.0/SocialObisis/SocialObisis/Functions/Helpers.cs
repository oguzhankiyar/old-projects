using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialObisis.Functions
{
    public static class Helpers
    {
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
    }
}