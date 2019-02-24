package musicbox.helpers;

import java.util.ArrayList;
import java.util.List;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import musicbox.models.Album;
import musicbox.models.Artist;
import musicbox.models.BaseObject;
import musicbox.models.Song;

public class SearchHelper {
	public static List<BaseObject> Search(String query) {
		List<BaseObject> items = new ArrayList<BaseObject>();
		
		String jsonString = new HttpHelper().getString("https://itunes.apple.com/search?term=" + query + "&entity=song,album,allArtist");
		JSONObject rootJson;
		try {
			rootJson = new JSONObject(jsonString);
			JSONArray resultsJson = rootJson.getJSONArray("results");
			for (int i = 1;i < resultsJson.length();i++) {
				JSONObject resultJson = resultsJson.getJSONObject(i);
				
				if (resultJson.getString("wrapperType").equals("artist")) {
					Artist artist = new Artist();
					artist.Id = "" + resultJson.getInt("artistId");
					artist.Name = resultJson.getString("artistName");
					items.add(artist);
				}
				else if (resultJson.getString("wrapperType").equals("collection")) {
					Album album = new Album();
					album.Id = "" + resultJson.getInt("collectionId");
					album.Name = resultJson.getString("collectionName");
					items.add(album);
				}
				else if (resultJson.getString("wrapperType").equals("track")) {
					Song song = new Song();
					song.Id = "" + resultJson.getInt("trackId");
					song.Name = resultJson.getString("trackName");
					song.ImageUrl = resultJson.getString("artworkUrl100");
					items.add(song);
				}
			}
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		return items;
	}
}
