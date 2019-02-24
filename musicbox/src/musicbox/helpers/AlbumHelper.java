package musicbox.helpers;

import java.util.ArrayList;
import java.util.List;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import musicbox.models.Album;
import musicbox.models.Artist;

public class AlbumHelper {
	public static List<Album> GetTopAlbums() {
		List<Album> albums = new ArrayList<Album>();
		
		String jsonString = new HttpHelper().getString("https://itunes.apple.com/rss/topalbums/limit=100/json");
		JSONObject rootJson;
		try {
			rootJson = new JSONObject(jsonString);
			JSONArray songsJson = rootJson.getJSONObject("feed").getJSONArray("entry");
			for (int i = 0;i < songsJson.length();i++) {
				JSONObject songJson = songsJson.getJSONObject(i);
				Album album = new Album();
				album.Id = songJson.getJSONObject("id").getJSONObject("attributes").getString("im:id");
				album.Name = songJson.getJSONObject("im:name").getString("label");
				albums.add(album);
			}
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return albums;
	}
	
	public static List<Album> GetAlbums(String id) {
		List<Album> albums = new ArrayList<Album>();

		String jsonString = new HttpHelper().getString("https://itunes.apple.com/lookup?id=" + id + "&entity=album");
		JSONObject rootJson;
		try {
			rootJson = new JSONObject(jsonString);
			JSONArray albumsJson = rootJson.getJSONArray("results");
			for (int i = 1;i < albumsJson.length();i++) {
				JSONObject albumJson = albumsJson.getJSONObject(i);
				Album album = new Album();
				album.Id = "" + albumJson.getInt("collectionId");
				album.Name = albumJson.getString("collectionName");
				album.ImageUrl = albumJson.getString("artworkUrl100");
				
				Artist artist = new Artist();
				artist.Id = "" + albumJson.getInt("artistId");
				artist.Name = albumJson.getString("artistName");
				album.Artist = artist;
				
				albums.add(album);
			}
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return albums;
	}
	
	public static Album GetAlbumDetails(String albumId) {
		Album album = new Album();
		
		String jsonString = new HttpHelper().getString("https://itunes.apple.com/lookup?id=" + albumId);
		JSONObject rootJson;
		try {
			rootJson = new JSONObject(jsonString);
			JSONObject albumJson = rootJson.getJSONArray("results").getJSONObject(0);
			album.Id = "" + albumJson.getInt("collectionId");
			album.Name = albumJson.getString("collectionName");
			album.ImageUrl = albumJson.getString("artworkUrl100");

			Artist artist = new Artist();
			artist.Id = "" + albumJson.getInt("artistId");
			artist.Name = albumJson.getString("artistName");
			album.Artist = artist;
			
			album.Songs = SongHelper.GetSongs(album.Id);
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return album;
	}
}
