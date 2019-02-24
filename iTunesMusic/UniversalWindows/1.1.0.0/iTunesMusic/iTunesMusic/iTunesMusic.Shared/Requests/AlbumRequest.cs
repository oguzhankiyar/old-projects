using iTunesMusic.Entities;
using iTunesMusic.Parsings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using iTunesMusic.Entities.EntityListItems;
using System.Net;
using iTunesMusic.Responses;

namespace iTunesMusic.Requests
{
    class AlbumRequest
    {
        public static async Task<List<AlbumListItem>> GetTopAlbums()
        {
            HttpWebRequest request = HttpWebRequest.Create(Constants.URLs.TopAlbums) as HttpWebRequest;
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            return AlbumResponse.GetTopAlbums(response);
        }

        public static async Task<Album> GetDetails(int id)
        {
            string url = string.Format(Constants.URLs.AlbumDetails, id);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            return AlbumResponse.GetDetails(response);
        }

        public static async Task<List<SongListItem>> GetSongs(int id)
        {
            string url = string.Format(Constants.URLs.AlbumSongs, id);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            return AlbumResponse.GetSongs(response);
        }
    }
}
