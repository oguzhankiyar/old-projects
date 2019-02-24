using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using iTunesMusic.Responses;
using iTunesMusic.Tools;
using System.Threading.Tasks;
using iTunesMusic.Entities;
using iTunesMusic.Entities.EntityListItems;

namespace iTunesMusic.Requests
{
    class SearchRequest
    {
        public static async Task<List<SearchListItem>> GetAll(string query)
        {
            string url = string.Format(Constants.URLs.SearchAll, TextTools.GetSearchQuery(query));
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            return SearchResponse.GetAll(response);
        }

        public static async Task<List<SearchListItem>> GetArtists(string query)
        {
            string url = string.Format(Constants.URLs.SearchArtists, TextTools.GetSearchQuery(query));
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            return SearchResponse.GetArtists(response);
        }

        public static async Task<List<SearchListItem>> GetAlbums(string query)
        {
            string url = string.Format(Constants.URLs.SearchAlbums, TextTools.GetSearchQuery(query));
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            return SearchResponse.GetAlbums(response);
        }

        public static async Task<List<SearchListItem>> GetSongs(string query)
        {
            string url = string.Format(Constants.URLs.SearchSongs, TextTools.GetSearchQuery(query));
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            return SearchResponse.GetSongs(response);
        }

        public static async Task<List<SearchListItem>> GetPlaylists(string query)
        {
            string url = string.Format(Constants.URLs.SearchPlaylists, TextTools.GetSearchQuery(query));
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            return SearchResponse.GetPlaylists(response);
        }
    }
}
