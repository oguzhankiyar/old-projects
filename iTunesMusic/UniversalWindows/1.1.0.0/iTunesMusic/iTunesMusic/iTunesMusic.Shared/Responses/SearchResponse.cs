using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;
using iTunesMusic.Entities;
using iTunesMusic.Parsings;
using Windows.UI;
using Windows.UI.Xaml.Media;
using iTunesMusic.Entities.EntityListItems;

namespace iTunesMusic.Responses
{
    class SearchResponse
    {
        public static List<SearchListItem> GetAll(HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            return SearchParsing.ParseAll(json);
        }

        public static List<SearchListItem> GetArtists(HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            return SearchParsing.ParseArtists(json);
        }

        public static List<SearchListItem> GetAlbums(HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            return SearchParsing.ParseAlbums(json);
        }

        public static List<SearchListItem> GetSongs(HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            return SearchParsing.ParseSongs(json);
        }

        public static List<SearchListItem> GetPlaylists(HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            return SearchParsing.ParsePlaylists(json);
        }
    }
}
