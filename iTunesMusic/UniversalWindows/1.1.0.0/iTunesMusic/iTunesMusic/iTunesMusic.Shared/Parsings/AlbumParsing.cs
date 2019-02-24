using iTunesMusic.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using iTunesMusic.Entities.EntityListItems;
using Newtonsoft.Json.Linq;

namespace iTunesMusic.Parsings
{
    class AlbumParsing
    {
        public static List<AlbumListItem> ParseTopAlbums(string json)
        {
            try
            {
                List<AlbumListItem> albums = new List<AlbumListItem>();
                JObject obj = JObject.Parse(json);
                JArray array = (obj["feed"])["entry"] as JArray;
                for (int i = 0; i < array.Count; i++)
                {
                    JObject albumObject = array[i] as JObject;
                    AlbumListItem album = new AlbumListItem();
                    album.Id = Convert.ToInt32(((albumObject["id"])["attributes"])["im:id"].ToString());
                    album.Name = (albumObject["im:name"])["label"].ToString();
                    album.ImageSource = new ImageSource()
                    {
                        Sizex30 = ((albumObject["im:image"])[0])["label"].ToString(),
                        Sizex60 = ((albumObject["im:image"])[1])["label"].ToString(),
                        Sizex100 = ((albumObject["im:image"])[2])["label"].ToString()
                    };
                    album.ArtistName = (albumObject["im:artist"])["label"].ToString();
                    album.ReleaseDate = Convert.ToDateTime((albumObject["im:releaseDate"])["label"].ToString());
                    album.Sort = i + 1;
                    albums.Add(album);
                }
                return albums;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Album ParseDetails(string json)
        {
            Album album = new Album();
            try
            {
                JObject obj = JObject.Parse(json);
                JArray array = obj["results"] as JArray;
                JObject albumObject = array[0] as JObject;
                album.Id = Convert.ToInt32(albumObject["artistId"].ToString());
                album.Name = albumObject["collectionName"].ToString();
                album.Artist = new Artist() { Id = Convert.ToInt32(albumObject["artistId"].ToString()), Name = albumObject["artistName"].ToString() };
                album.ImageSource = new ImageSource()
                {
                    Sizex60 = albumObject["artworkUrl60"].ToString(),
                    Sizex100 = albumObject["artworkUrl100"].ToString()
                };
                album.ReleaseDate = Convert.ToDateTime(albumObject["releaseDate"].ToString());
                album.Genre = new Genre() { Id = Convert.ToInt32(albumObject["primaryGenreId"].ToString()), Name = albumObject["primaryGenreName"].ToString() };
            }
            catch (Exception) { }
            return album;
        }

        public static List<SongListItem> ParseSongs(string json)
        {
            try
            {
                List<SongListItem> songs = new List<SongListItem>();
                JObject obj = JObject.Parse(json);
                JArray array = obj["results"] as JArray;
                for (int i = 1; i < array.Count; i++)
                {
                    JObject songObject = array[i] as JObject;
                    SongListItem song = new SongListItem();
                    song.Id = Convert.ToInt32(songObject["trackId"].ToString());
                    song.Name = songObject["trackName"].ToString();
                    song.ArtistName = songObject["artistName"].ToString();
                    song.ImageSource = new ImageSource()
                    {
                        Sizex30 = songObject["artworkUrl30"].ToString(),
                        Sizex60 = songObject["artworkUrl60"].ToString(),
                        Sizex100 = songObject["artworkUrl100"].ToString()
                    };
                    songs.Add(song);
                }
                return songs;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
