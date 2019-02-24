using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using iTunesMusic.Entities;
using iTunesMusic.Entities.EntityListItems;
using iTunesMusic.Tools;

namespace iTunesMusic.Parsings
{
    class SearchParsing
    {
        public static List<SearchListItem> ParseAll(string json)
        {
            try
            {
                List<SearchListItem> results = new List<SearchListItem>();
                JObject obj = JObject.Parse(json);
                JArray array = obj["results"] as JArray;
                for (int i = 0; i < array.Count; i++)
                {
                    JObject itemObject = array[i] as JObject;
                    SearchListItem item = new SearchListItem();
                    if (itemObject["wrapperType"].ToString() == "artist")
                    {
                        item.Id = Convert.ToInt32(itemObject["artistId"].ToString());
                        item.Name = TextTools.GetTidyText(itemObject["artistName"].ToString());
                        item.Type = "Artist";
                    }
                    else if (itemObject["wrapperType"].ToString() == "collection")
                    {
                        item.Id = Convert.ToInt32(itemObject["collectionId"].ToString());
                        item.Name = TextTools.GetTidyText(itemObject["collectionName"].ToString());
                        item.Info = TextTools.GetTidyText(itemObject["artistName"].ToString());
                        item.Type = "Album";
                    }
                    else if (itemObject["wrapperType"].ToString() == "track")
                    {
                        item.Id = Convert.ToInt32(itemObject["trackId"].ToString());
                        item.Name = TextTools.GetTidyText(itemObject["trackName"].ToString());
                        item.Info = TextTools.GetTidyText(itemObject["artistName"].ToString());
                        item.Type = "Song";
                    }

                    try
                    {
                        item.ImageSource = new ImageSource()
                        {
                            Sizex30 = itemObject["artworkUrl30"].ToString(),
                            Sizex60 = itemObject["artworkUrl60"].ToString(),
                            Sizex100 = itemObject["artworkUrl100"].ToString()
                        };
                    }
                    catch (Exception) { }
                    results.Add(item);
                }
                return results;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static List<SearchListItem> ParseArtists(string json)
        {
            try
            {
                List<SearchListItem> results = new List<SearchListItem>();
                JObject obj = JObject.Parse(json);
                JArray array = obj["results"] as JArray;
                for (int i = 0; i < array.Count; i++)
                {
                    JObject itemObject = array[i] as JObject;
                    SearchListItem item = new SearchListItem();
                    item.Name = TextTools.GetTidyText(itemObject["artistName"].ToString());
                    item.ImageSource = new ImageSource()
                    {
                        Sizex30 = itemObject["artworkUrl30"].ToString(),
                        Sizex60 = itemObject["artworkUrl60"].ToString(),
                        Sizex100 = itemObject["artworkUrl100"].ToString()
                    };
                    item.Type = "Artist";
                }
                return results;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<SearchListItem> ParseAlbums(string json)
        {
            try
            {
                List<SearchListItem> results = new List<SearchListItem>();
                JObject obj = JObject.Parse(json);
                JArray array = obj["results"] as JArray;
                for (int i = 0; i < array.Count; i++)
                {
                    JObject itemObject = array[i] as JObject;
                    SearchListItem item = new SearchListItem();
                    item.Name = TextTools.GetTidyText(itemObject["collectionName"].ToString());
                    item.Info = TextTools.GetTidyText(itemObject["artistName"].ToString());
                    item.ImageSource = new ImageSource()
                    {
                        Sizex30 = itemObject["artworkUrl30"].ToString(),
                        Sizex60 = itemObject["artworkUrl60"].ToString(),
                        Sizex100 = itemObject["artworkUrl100"].ToString()
                    };
                    item.Type = "Album";
                }
                return results;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<SearchListItem> ParseSongs(string json)
        {
            try
            {
                List<SearchListItem> results = new List<SearchListItem>();
                JObject obj = JObject.Parse(json);
                JArray array = obj["results"] as JArray;
                for (int i = 0; i < array.Count; i++)
                {
                    JObject itemObject = array[i] as JObject;
                    SearchListItem item = new SearchListItem();
                    item.Name = TextTools.GetTidyText(itemObject["trackName"].ToString());
                    item.Info = TextTools.GetTidyText(itemObject["artistName"].ToString());
                    item.ImageSource = new ImageSource()
                    {
                        Sizex30 = itemObject["artworkUrl30"].ToString(),
                        Sizex60 = itemObject["artworkUrl60"].ToString(),
                        Sizex100 = itemObject["artworkUrl100"].ToString()
                    };
                    item.Type = "Song";
                }
                return results;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<SearchListItem> ParsePlaylists(string json)
        {
            return null;
        }

    }
}
