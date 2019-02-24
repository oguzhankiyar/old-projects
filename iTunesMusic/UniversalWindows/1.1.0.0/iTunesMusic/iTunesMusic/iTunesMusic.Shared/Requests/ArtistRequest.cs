using iTunesMusic.Entities;
using iTunesMusic.Parsings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using iTunesMusic.Responses;
using iTunesMusic.Entities.EntityListItems;

namespace iTunesMusic.Requests
{
    class ArtistRequest
    {
        public static async Task<Artist> GetDetails(int id)
        {
            string url = string.Format(Constants.URLs.ArtistDetails, id);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            return ArtistResponse.GetDetails(response);
        }

        public static async Task<List<AlbumListItem>> GetAlbums(int id)
        {
            string url = string.Format(Constants.URLs.ArtistAlbums, id);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            return ArtistResponse.GetAlbums(response);
        }

        public static async Task<List<SongListItem>> GetSongs(int id)
        {
            string url = string.Format(Constants.URLs.ArtistSongs, id);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            return ArtistResponse.GetSongs(response);
        }
    }
}
