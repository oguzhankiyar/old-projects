using iTunesMusic.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using iTunesMusic.Entities.EntityListItems;
using System.IO;
using iTunesMusic.Parsings;

namespace iTunesMusic.Responses
{
    class SongResponse
    {
        public static List<SongListItem> GetTopSongs(HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            return SongParsing.ParseTopSongs(json);
        }

        public static Song GetDetails(HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            return SongParsing.ParseDetails(json);
        }

        public static List<SongSource> GetSources(HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            return SongParsing.ParseSources(json);
        }
    }
}
