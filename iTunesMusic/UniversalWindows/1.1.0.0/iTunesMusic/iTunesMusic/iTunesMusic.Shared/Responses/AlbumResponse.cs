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
    class AlbumResponse
    {
        public static List<AlbumListItem> GetTopAlbums(HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            return AlbumParsing.ParseTopAlbums(json);
        }

        public static Album GetDetails(HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            return AlbumParsing.ParseDetails(json);
        }

        public static List<SongListItem> GetSongs(HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            return AlbumParsing.ParseSongs(json);
        }
    }
}
