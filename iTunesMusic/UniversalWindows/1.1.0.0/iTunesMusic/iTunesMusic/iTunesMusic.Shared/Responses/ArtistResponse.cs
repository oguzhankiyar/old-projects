using iTunesMusic.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;
using iTunesMusic.Parsings;
using iTunesMusic.Entities.EntityListItems;

namespace iTunesMusic.Responses
{
    class ArtistResponse
    {
        public static Artist GetDetails(HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            return ArtistParsing.ParseDetails(json);
        }

        public static List<AlbumListItem> GetAlbums(HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            return ArtistParsing.ParseAlbums(json);
        }

        public static List<SongListItem> GetSongs(HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            return ArtistParsing.ParseSongs(json);
        }
    }
}
