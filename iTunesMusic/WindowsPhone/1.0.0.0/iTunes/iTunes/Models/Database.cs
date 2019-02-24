using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTunes.Models
{
    public class Database
    {
        public static List<Artist> Artists { get; private set; }
        public static List<Album> Albums { get; private set; }
        public static List<Song> Songs { get; private set; }
        public static List<Song> TopSongs { get; private set; }
        public static List<Album> TopAlbums { get; private set; }
        public static List<Song> Favorites { get; private set; }
        public static Settings Settings { get; set; }
        public static List<Country> Countries { get; set; }

        public static void Fill()
        {
            FillCountries();

            DataSaver<List<Artist>> ArtistsData = new DataSaver<List<Artist>>();
            Artists = ArtistsData.LoadMyData("Artists") == null ? new List<Artist>() : ArtistsData.LoadMyData("Artists");

            DataSaver<List<Album>> AlbumsData = new DataSaver<List<Album>>();
            Albums = AlbumsData.LoadMyData("Albums") == null ? new List<Album>() : AlbumsData.LoadMyData("Albums");

            DataSaver<List<Song>> SongsData = new DataSaver<List<Song>>();
            Songs = SongsData.LoadMyData("Songs") == null ? new List<Song>() : SongsData.LoadMyData("Songs");

            DataSaver<List<Song>> TopSongsData = new DataSaver<List<Song>>();
            TopSongs = TopSongsData.LoadMyData("TopSongs") == null ? new List<Song>() : TopSongsData.LoadMyData("TopSongs");

            DataSaver<List<Album>> TopAlbumsData = new DataSaver<List<Album>>();
            TopAlbums = TopAlbumsData.LoadMyData("TopAlbums") == null ? new List<Album>() : TopAlbumsData.LoadMyData("TopAlbums");

            DataSaver<List<Song>> FavoritesData = new DataSaver<List<Song>>();
            Favorites = FavoritesData.LoadMyData("Favorites") == null ? new List<Song>() : FavoritesData.LoadMyData("Favorites");

            DataSaver<Settings> SettingsData = new DataSaver<Settings>();
            Settings = SettingsData.LoadMyData("Settings") == null ? new Settings() : SettingsData.LoadMyData("Settings");
        }

        private static void FillCountries()
        {
            Countries = new List<Country>();
            Countries.Add(new Country() { Name = "Armenia", ShortName = "am" });
            Countries.Add(new Country() { Name = "Bahrain", ShortName = "bh" });
            Countries.Add(new Country() { Name = "Botswana", ShortName = "bw" });
            Countries.Add(new Country() { Name = "Cameroun", ShortName = "cm" });
            Countries.Add(new Country() { Name = "République Centrafricaine", ShortName = "cf" });
            Countries.Add(new Country() { Name = "Côte d'Ivoire", ShortName = "ci" });
            Countries.Add(new Country() { Name = "Egypt", ShortName = "eg" });
            Countries.Add(new Country() { Name = "Guinea-Bissau", ShortName = "gw" });
            Countries.Add(new Country() { Name = "Guinée", ShortName = "gn" });
            Countries.Add(new Country() { Name = "Guinée Equatoriale", ShortName = "gq" });
            Countries.Add(new Country() { Name = "India", ShortName = "in" });
            Countries.Add(new Country() { Name = "Israel", ShortName = "il" });
            Countries.Add(new Country() { Name = "Jordan", ShortName = "jo" });
            Countries.Add(new Country() { Name = "Kenya", ShortName = "ke" });
            Countries.Add(new Country() { Name = "Maroc", ShortName = "ma" });
            Countries.Add(new Country() { Name = "Maurice", ShortName = "mu" });
            Countries.Add(new Country() { Name = "Mozambique", ShortName = "mz" });
            Countries.Add(new Country() { Name = "Niger", ShortName = "ne" });
            Countries.Add(new Country() { Name = "Nigeria", ShortName = "ng" });
            Countries.Add(new Country() { Name = "Oman", ShortName = "om" });
            Countries.Add(new Country() { Name = "Qatar", ShortName = "qa" });
            Countries.Add(new Country() { Name = "Saudi Arabia", ShortName = "sa" });
            Countries.Add(new Country() { Name = "South Africa", ShortName = "za" });
            Countries.Add(new Country() { Name = "Uganda", ShortName = "ug" });
            Countries.Add(new Country() { Name = "United Arab Emirates", ShortName = "ae" });
            Countries.Add(new Country() { Name = "Australia", ShortName = "au" });
            Countries.Add(new Country() { Name = "香港", ShortName = "hk" });
            Countries.Add(new Country() { Name = "Indonesia", ShortName = "id" });
            Countries.Add(new Country() { Name = "日本", ShortName = "jp" });
            Countries.Add(new Country() { Name = "Malaysia", ShortName = "my" });
            Countries.Add(new Country() { Name = "Countries.Add(new Zealand", ShortName = "nz" });
            Countries.Add(new Country() { Name = "Philippines", ShortName = "ph" });
            Countries.Add(new Country() { Name = "Singapore", ShortName = "sg" });
            Countries.Add(new Country() { Name = "台灣", ShortName = "tw" });
            Countries.Add(new Country() { Name = "Thailand", ShortName = "th" });
            Countries.Add(new Country() { Name = "Vietnam", ShortName = "vn" });
            Countries.Add(new Country() { Name = "България", ShortName = "bg" });
            Countries.Add(new Country() { Name = "Česká republika", ShortName = "cz" });
            Countries.Add(new Country() { Name = "Danmark", ShortName = "dk" });
            Countries.Add(new Country() { Name = "Deutschland", ShortName = "de" });
            Countries.Add(new Country() { Name = "Eesti", ShortName = "ee" });
            Countries.Add(new Country() { Name = "España", ShortName = "es" });
            Countries.Add(new Country() { Name = "France", ShortName = "fr" });
            Countries.Add(new Country() { Name = "Ελλάδα", ShortName = "gr" });
            Countries.Add(new Country() { Name = "Ireland", ShortName = "ie" });
            Countries.Add(new Country() { Name = "Italia", ShortName = "it" });
            Countries.Add(new Country() { Name = "Latvija", ShortName = "lv" });
            Countries.Add(new Country() { Name = "Liechtenstein", ShortName = "li" });
            Countries.Add(new Country() { Name = "Lietuva", ShortName = "lt" });
            Countries.Add(new Country() { Name = "Luxembourg", ShortName = "lu" });
            Countries.Add(new Country() { Name = "Magyarország", ShortName = "hu" });
            Countries.Add(new Country() { Name = "Malta", ShortName = "mt" });
            Countries.Add(new Country() { Name = "Moldova", ShortName = "md" });
            Countries.Add(new Country() { Name = "Montenegro", ShortName = "me" });
            Countries.Add(new Country() { Name = "Nederland", ShortName = "nl" });
            Countries.Add(new Country() { Name = "Norge", ShortName = "no" });
            Countries.Add(new Country() { Name = "Österreich", ShortName = "at" });
            Countries.Add(new Country() { Name = "Polska", ShortName = "pl" });
            Countries.Add(new Country() { Name = "Portugal", ShortName = "pt" });
            Countries.Add(new Country() { Name = "România", ShortName = "ro" });
            Countries.Add(new Country() { Name = "Россия", ShortName = "ru" });
            Countries.Add(new Country() { Name = "Slovensko", ShortName = "sk" });
            Countries.Add(new Country() { Name = "Slovenia", ShortName = "si" });
            Countries.Add(new Country() { Name = "Suomi", ShortName = "fi" });
            Countries.Add(new Country() { Name = "Sverige", ShortName = "se" });
            Countries.Add(new Country() { Name = "Türkiye", ShortName = "tr" });
            Countries.Add(new Country() { Name = "UK", ShortName = "uk" });
            Countries.Add(new Country() { Name = "Argentina", ShortName = "la" });
            Countries.Add(new Country() { Name = "Bolivia", ShortName = "la" });
            Countries.Add(new Country() { Name = "Brasil", ShortName = "br" });
            Countries.Add(new Country() { Name = "Chile", ShortName = "la" });
            Countries.Add(new Country() { Name = "Colombia", ShortName = "la" });
            Countries.Add(new Country() { Name = "Costa Rica", ShortName = "la" });
            Countries.Add(new Country() { Name = "República Dominicana", ShortName = "la" });
            Countries.Add(new Country() { Name = "Ecuador", ShortName = "la" });
            Countries.Add(new Country() { Name = "El Salvador", ShortName = "la" });
            Countries.Add(new Country() { Name = "Guatemala", ShortName = "la" });
            Countries.Add(new Country() { Name = "Honduras", ShortName = "la" });
            Countries.Add(new Country() { Name = "México", ShortName = "mx" });
            Countries.Add(new Country() { Name = "Nicaragua", ShortName = "la" });
            Countries.Add(new Country() { Name = "Panamá", ShortName = "la" });
            Countries.Add(new Country() { Name = "Paraguay", ShortName = "la" });
            Countries.Add(new Country() { Name = "Perú", ShortName = "la" });
            Countries.Add(new Country() { Name = "Uruguay", ShortName = "la" });
            Countries.Add(new Country() { Name = "Venezuela", ShortName = "la" });
            Countries.Add(new Country() { Name = "Canada (English)", ShortName = "ca" });
            Countries.Add(new Country() { Name = "Puerto Rico (español)", ShortName = "la" });
            Countries.Add(new Country() { Name = "United States", ShortName = "us" });
            Countries = Countries.OrderBy(x => x.Name).ToList();
            Countries.Insert(0, new Country() { Name = "Worldwide", ShortName = "" });
        }
        public static void UpdateArtists()
        {
            DataSaver<List<Artist>> ArtistsData = new DataSaver<List<Artist>>();
            ArtistsData.SaveMyData(Artists, "Artists");
        }
        public static void UpdateAlbums()
        {
            DataSaver<List<Album>> AlbumsData = new DataSaver<List<Album>>();
            AlbumsData.SaveMyData(Albums, "Albums");
        }
        public static void UpdateSongs()
        {
            DataSaver<List<Song>> SongsData = new DataSaver<List<Song>>();
            SongsData.SaveMyData(Songs, "Songs");
        }
        public static void UpdateTopSongs()
        {
            DataSaver<List<Song>> SongsData = new DataSaver<List<Song>>();
            SongsData.SaveMyData(TopSongs, "TopSongs");
        }
        public static void UpdateTopAlbums()
        {
            DataSaver<List<Album>> AlbumsData = new DataSaver<List<Album>>();
            AlbumsData.SaveMyData(TopAlbums, "TopAlbums");
        }
        public static void UpdateFavorites()
        {
            DataSaver<List<Song>> SongsData = new DataSaver<List<Song>>();
            SongsData.SaveMyData(Favorites, "Favorites");
        }
        public static void UpdateSettings()
        {
            DataSaver<Settings> SettingsData = new DataSaver<Settings>();
            SettingsData.SaveMyData(Settings, "Settings");
        }
        public static void UpdateAll()
        {
            UpdateArtists();
            UpdateAlbums();
            UpdateSongs();
            UpdateTopSongs();
            UpdateTopAlbums();
            UpdateFavorites();
            UpdateSettings();
        }
        public static Artist AddArtist(Artist newArtist)
        {
            Artist artist = Database.Artists.SingleOrDefault(x => x.ID == newArtist.ID);
            if (artist == null)
            {
                Artists.Add(newArtist);
                return newArtist;
            }
            return artist;
        }
        public static Album AddAlbum(Album newAlbum)
        {
            Artist artist = Database.AddArtist(newAlbum.Artist);
            Album album = artist.Albums.SingleOrDefault(x => x.ID == newAlbum.ID);
            if (album == null)
            {
                artist.Albums.Add(newAlbum);
                if (Albums.SingleOrDefault(x => x.ID == newAlbum.ID) == null)
                    Albums.Add(newAlbum);
                return newAlbum;
            }
            album.Sort = newAlbum.Sort;
            return album;
        }
        public static Song AddSong(Song newSong)
        {
            Album album = Database.AddAlbum(newSong.Album);
            Artist artist = Database.AddArtist(newSong.Artist);
            Song song = album.Songs.SingleOrDefault(x => x.ID == newSong.ID);
            if (song == null)
            {
                album.Songs.Add(newSong);
                if (Songs.SingleOrDefault(x => x.ID == newSong.ID) == null)
                    Songs.Add(newSong);
                return newSong;
            }
            song.Sort = newSong.Sort;
            return song;
        }
        public static void AddFavorite(Song song)
        {
            var favSong = Favorites.SingleOrDefault(x => x.ID == song.ID);
            if (favSong == null)
                Favorites.Insert(0, AddSong(song));
        }
        public static void DeleteFavorite(Song song)
        {
            var favSong = Favorites.SingleOrDefault(x => x.ID == song.ID);
            if (favSong != null)
                Favorites.Remove(favSong);
        }
        public static void AddTopSong(Song song)
        {
            TopSongs.Add(Database.AddSong(song));
        }
        public static void AddTopAlbum(Album album)
        {
            TopAlbums.Add(Database.AddAlbum(album));
        }
        public static void EmptyTopSongs()
        {
            TopSongs = new List<Song>();
        }
        public static void EmptyTopAlbums()
        {
            TopAlbums = new List<Album>();
        }
    }
}
