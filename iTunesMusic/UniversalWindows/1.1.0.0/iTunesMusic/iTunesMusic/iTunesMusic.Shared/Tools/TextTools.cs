using System;
using System.Collections.Generic;
using System.Text;

namespace iTunesMusic.Tools
{
    class TextTools
    {
        public static string GetTidyText(string text)
        {
            return text;
        }
        public static string GetSearchQuery(string text)
        {
            text = text.Replace(" ", "+");
            return text;
        }
    }
}
