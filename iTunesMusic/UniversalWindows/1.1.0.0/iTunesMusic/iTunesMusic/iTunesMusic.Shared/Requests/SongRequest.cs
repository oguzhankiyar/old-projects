using iTunesMusic.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using iTunesMusic.Entities.EntityListItems;
using System.Net;
using iTunesMusic.Responses;
using iTunesMusic.Tools;

namespace iTunesMusic.Requests
{
    class SongRequest
    {
        public static async Task<List<SongListItem>> GetTopSongs()
        {
            HttpWebRequest request = HttpWebRequest.Create(Constants.URLs.TopSongs) as HttpWebRequest;
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            return SongResponse.GetTopSongs(response);
        }

        public static async Task<Song> GetDetails(int id)
        {
            string url = string.Format(Constants.URLs.SongDetails, id);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            return SongResponse.GetDetails(response);
        }

        public static async Task<List<SongSource>> GetSources(string query, int page = 1)
        {
            string url = string.Format(Constants.URLs.SongSources, TextTools.GetTidyText(query), page);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            return SongResponse.GetSources(response);
        }
    }
}
