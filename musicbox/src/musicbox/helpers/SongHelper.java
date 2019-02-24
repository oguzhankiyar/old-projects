package musicbox.helpers;

import java.util.ArrayList;
import java.util.List;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import musicbox.models.Album;
import musicbox.models.Artist;
import musicbox.models.Song;

public class SongHelper {

	public static List<Song> GetTopSongs() {
		List<Song> songs = new ArrayList<Song>();
		
		String jsonString = new HttpHelper().getString("https://itunes.apple.com/rss/topsongs/limit=100/json");
		JSONObject rootJson;
		try {
			rootJson = new JSONObject(jsonString);
			JSONArray songsJson = rootJson.getJSONObject("feed").getJSONArray("entry");
			for (int i = 0;i < songsJson.length();i++) {
				JSONObject songJson = songsJson.getJSONObject(i);
				Song song = new Song();
				song.Id = songJson.getJSONObject("id").getJSONObject("attributes").getString("im:id");
				song.Name = songJson.getJSONObject("im:name").getString("label");
				songs.add(song);
			}
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return songs;
	}
	
	public static List<Song> GetSongs(String id) {
		List<Song> songs = new ArrayList<Song>();
		
		String jsonString = new HttpHelper().getString("https://itunes.apple.com/lookup?id=" + id + "&entity=song");
		JSONObject rootJson;
		try {
			rootJson = new JSONObject(jsonString);
			JSONArray songsJson = rootJson.getJSONArray("results");
			for (int i = 1;i < songsJson.length();i++) {
				JSONObject songJson = songsJson.getJSONObject(i);
				Song song = new Song();
				song.Id = "" + songJson.getInt("trackId");
				song.Name = songJson.getString("trackName");
				song.ImageUrl = songJson.getString("artworkUrl100");
				
				Album album = new Album();
				album.Id = "" + songJson.getInt("collectionId");
				album.Name = songJson.getString("collectionName");
				song.Album = album;
				
				Artist artist = new Artist();
				artist.Id = "" + songJson.getInt("artistId");
				artist.Name = songJson.getString("artistName");
				song.Artist = artist;
				songs.add(song);
			}
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return songs;
	}
	
	public static Song GetSongDetails(String songId) {
		Song song = new Song();
		
		String jsonString = new HttpHelper().getString("https://itunes.apple.com/lookup?id=" + songId);
		JSONObject rootJson;
		try {
			rootJson = new JSONObject(jsonString);
			JSONObject songJson = rootJson.getJSONArray("results").getJSONObject(0);
			song.Id = "" + songJson.getInt("trackId");
			song.Name = songJson.getString("trackName");
			song.ImageUrl = songJson.getString("artworkUrl100");
			song.PreviewUrl = songJson.getString("previewUrl");
			
			Album album = new Album();
			album.Id = "" + songJson.getInt("collectionId");
			album.Name = songJson.getString("collectionName");
			song.Album = album;
			
			Artist artist = new Artist();
			artist.Id = "" + songJson.getInt("artistId");
			artist.Name = songJson.getString("artistName");
			song.Artist = artist;
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return song;
	}
}
