using iTunesMusic.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using iTunesMusic.Entities.EntityListItems;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;
using System.Linq;

namespace iTunesMusic.Parsings
{
    class SongParsing
    {
        public static List<SongListItem> ParseTopSongs(string json)
        {
            try
            {
                List<SongListItem> songs = new List<SongListItem>();
                JObject obj = JObject.Parse(json);
                JArray array = (obj["feed"])["entry"] as JArray;
                for (int i = 0; i < array.Count; i++)
                {
                    JObject songObject = array[i] as JObject;
                    SongListItem song = new SongListItem();
                    song.Sort = i + 1;
                    song.Id = Convert.ToInt32(((songObject["id"])["attributes"])["im:id"].ToString());
                    song.Name = (songObject["im:name"])["label"].ToString();
                    int colID = 0;
                    try
                    {
                        string collectionID = (((songObject["im:collection"])["link"])["attributes"])["href"].ToString();
                        string[] split = collectionID.Split('/');
                        string[] split2 = split[split.Length - 1].Split('?');
                        colID = Convert.ToInt32(split2[0].Replace("id", ""));
                    }
                    catch { }
                    int artID = 0;
                    try
                    {
                        string artistID = ((songObject["im:artist"])["attributes"])["href"].ToString();
                        string[] split = artistID.Split('/');
                        string[] split2 = split[split.Length - 1].Split('?');
                        artID = Convert.ToInt32(split2[0].Replace("id", ""));
                    }
                    catch { }
                    try
                    {
                        song.ArtistName = (songObject["im:artist"])["label"].ToString();
                        song.ImageSource = new ImageSource()
                        {
                            Sizex30 = ((songObject["im:image"])[0])["label"].ToString(),
                            Sizex60 = ((songObject["im:image"])[1])["label"].ToString(),
                            Sizex100 = ((songObject["im:image"])[2])["label"].ToString()
                        };
                    }
                    catch { }
                    song.ReleaseDate = Convert.ToDateTime((songObject["im:releaseDate"])["label"].ToString());
                    song.Sort = i + 1;
                    songs.Add(song);
                }
                return songs;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Song ParseDetails(string json)
        {
            try
            {
                Song song = new Song();
                JObject obj = JObject.Parse(json);
                JArray array = obj["results"] as JArray;
                JObject songObject = array[0] as JObject;
                song.Id = Convert.ToInt32(songObject["trackId"].ToString());
                song.Name = songObject["trackName"].ToString();
                song.ImageSource = new ImageSource()
                {
                    Sizex30 = songObject["artworkUrl30"].ToString(),
                    Sizex60 = songObject["artworkUrl60"].ToString(),
                    Sizex100 = songObject["artworkUrl100"].ToString()
                };
                try
                {
                    song.PreviewSource = songObject["previewUrl"].ToString();
                }
                catch(Exception) { }
                song.Artist = new Artist() { Id = Convert.ToInt32(songObject["artistId"].ToString()), Name = songObject["artistName"].ToString() };
                song.Album = new Album() { Id = Convert.ToInt32(songObject["collectionId"].ToString()), Name = songObject["collectionName"].ToString() };
                song.Price = songObject["trackPrice"].ToString();
                song.Genre = new Genre() { Name = songObject["primaryGenreName"].ToString() };
                song.ReleaseDate = Convert.ToDateTime(songObject["releaseDate"].ToString());
                return song;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<SongSource> ParseSources(string html)
        {
            try
            {
                List<SongSource> sources = new List<SongSource>();
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                HtmlNode node = doc.DocumentNode;
                IEnumerable<HtmlNode> col = node.ChildNodes.Descendants("li");
                foreach (var item in col)
                {
                    if (!string.IsNullOrEmpty(item.InnerText.ToString().Replace("\r", "").Replace("\n", "").Replace("\t", "")))
                    {
                        try
                        {
                            SongSource source = new SongSource();
                            IEnumerable<HtmlNode> nodem = item.ChildNodes.Where(x => x.Name == "div");
                            IEnumerable<HtmlNode> nodesm;
                            if (nodem.First().Elements("div").Where(x => x.Attributes["class"].Value.Contains("newPlayer")).Count() != 0)
                            {
                                source.Source = string.Format(Constants.URLs.SongSourceFile, nodem.First().Elements("div").Where(x => x.Attributes["class"].Value.Contains("newPlayer")).First().Attributes["fcode"].Value.ToString());
                            }
                            if (nodem.First().Elements("a").Where(x => x.Attributes["class"].Value.Contains("nhsTrackTitle")).Count() != 0)
                            {
                                nodesm = nodem.First().Elements("a").Where(x => x.Attributes["class"].Value.Contains("nhsTrackTitle"));
                                source.Name = nodesm.First().InnerText;
                                sources.Add(source);
                            }
                            else if (nodem.First().Elements("div").Where(x => x.Attributes["class"].Value.Contains("nhsTrunTrackedTitle")).Count() != 0)
                            {
                                nodesm = nodem.First().Elements("div").Where(x => x.Attributes["class"].Value.Contains("nhsTrunTrackedTitle"));
                                source.Name = nodesm.First().InnerText;
                                sources.Add(source);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                return sources;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
