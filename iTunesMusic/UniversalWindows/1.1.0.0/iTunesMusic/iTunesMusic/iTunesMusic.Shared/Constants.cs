using System;
using System.Collections.Generic;
using System.Text;
using iTunesMusic.Entities;
using iTunesMusic.Tools;

namespace iTunesMusic
{
    class Constants
    {
        public class URLs
        {
            public static string TopSongs
            {
                get
                {
                    if (Database.Settings.DefaultCountry == null)
                        return ("https://itunes.apple.com/rss/topsongs/limit=100/json");
                    return (string.Format("https://itunes.apple.com/{0}/rss/topsongs/limit=100/json", Database.Settings.DefaultCountry.ShortName));
                }
            }
            public static string TopAlbums
            {
                get
                {
                    if (Database.Settings.DefaultCountry == null)
                        return ("https://itunes.apple.com/rss/topalbums/limit=100/json");
                    return (string.Format("https://itunes.apple.com/{0}/rss/topalbums/limit=100/json", Database.Settings.DefaultCountry.ShortName));
                }
            }

            public const string SearchAll = "https://itunes.apple.com/search?entity=song,album,musicArtist&limit=100&term={0}";
            public const string SearchArtists = "https://itunes.apple.com/search?entity=musicArtist&limit=100&term={0}";
            public const string SearchAlbums = "https://itunes.apple.com/search?entity=album&limit=100&term={0}";
            public const string SearchSongs = "https://itunes.apple.com/search?entity=song&limit=100&term={0}";
            public const string SearchPlaylists = "http://music.ogzkyr.net/SearchPlaylists";

            public const string ArtistDetails = "https://itunes.apple.com/lookup?entity=musicArtist&id={0}";
            public const string ArtistAlbums = "https://itunes.apple.com/lookup?entity=album&limit=100&id={0}";
            public const string ArtistSongs = "https://itunes.apple.com/lookup?entity=song&limit=100&id={0}";

            public const string AlbumDetails = "http://itunes.apple.com/lookup?entity=album&id={0}";
            public const string AlbumSongs = "http://itunes.apple.com/lookup?entity=song&limit=100&id={0}";

            public const string SongDetails = "https://itunes.apple.com/lookup?entity=song&id={0}";
            public const string SongSources = "http://www.hulkshare.com/search/{0}?type=music&page{1}";
            public const string SongSourceFile = "http://trckr.hulkshare.com/hulkdl/{0}/music.mp3?z=1";

            public const string Playlists = "http://music.ogzkyr.net/GetPlaylists";
        }

        public class DataFiles
        {
            public const string BaseDirectory = "iTunesMusic";
            public const string Settings = "SettingsData";
            public const string TopSongs = "TopSongsData";
            public const string TopAlbums = "TopAlbumsData";
            public const string Artists = "ArtistsData";
            public const string Albums = "AlbumsData";
            public const string Songs = "SongsData";
            public const string Playlists = "PlaylistsData";
        }

    }
}
