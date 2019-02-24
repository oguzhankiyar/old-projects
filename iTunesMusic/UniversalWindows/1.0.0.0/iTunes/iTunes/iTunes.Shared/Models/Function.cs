using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTunes.Models
{
    class Function
    {
        public static bool isFavorited(Song song)
        {
            return (Database.Favorites.SingleOrDefault(x => x.ID == song.ID) != null);
        }
        public static string GetURL(string type)
        {
            if (type == "TopSongs")
            {
                if (Database.Settings.DefaultCountry == null)
                    return ("https://itunes.apple.com/rss/topsongs/limit=100/json");
                else
                    return string.Format("https://itunes.apple.com/{0}/rss/topsongs/limit=100/json", Database.Settings.DefaultCountry.ShortName);
            }
            else if (type == "TopAlbums")
            {
                if (Database.Settings.DefaultCountry == null)
                    return ("https://itunes.apple.com/rss/topalbums/limit=100/json");
                else
                    return string.Format("https://itunes.apple.com/{0}/rss/topalbums/limit=100/json", Database.Settings.DefaultCountry.ShortName);
            }
            return null;
        }
    }
}
