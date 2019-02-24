using iTunesMusic.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using iTunesMusic.Entities.EntityListItems;

namespace iTunesMusic.Parsings
{
    class ArtistParsing
    {
        public static Artist ParseDetails(string json)
        {
            try
            {
                Artist artist = new Artist();
                JObject obj = JObject.Parse(json);
                JArray array = obj["results"] as JArray;
                JObject artistObject = array[0] as JObject;
                artist.Id = Convert.ToInt32(artistObject["artistId"].ToString());
                artist.Name = artistObject["artistName"].ToString();
                artist.Genre = new Genre() { Id = Convert.ToInt32(artistObject["primaryGenreId"].ToString()), Name = artistObject["primaryGenreName"].ToString() };
                return artist;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<AlbumListItem> ParseAlbums(string json)
        {
            try
            {
                List<AlbumListItem> albums = new List<AlbumListItem>();
                JObject obj = JObject.Parse(json);
                JArray array = obj["results"] as JArray;
                for (int i = 1; i < array.Count; i++)
                {
                    JObject albumObject = array[i] as JObject;
                    AlbumListItem album = new AlbumListItem();
                    album.Id = Convert.ToInt32(albumObject["collectionId"].ToString());
                    album.Name = albumObject["collectionName"].ToString();
                    album.ArtistName = albumObject["artistName"].ToString();
                    album.ImageSource = new ImageSource()
                    {
                        Sizex60 = albumObject["artworkUrl60"].ToString(),
                        Sizex100 = albumObject["artworkUrl100"].ToString()
                    };
                    album.ReleaseDate = Convert.ToDateTime(albumObject["releaseDate"].ToString());
                    albums.Add(album);
                }
                return albums;
            }
            catch (Exception)
            {
                return null;
            }
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
