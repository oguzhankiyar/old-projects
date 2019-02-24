package musicbox.helpers;

import org.json.JSONException;
import org.json.JSONObject;

import musicbox.models.Artist;

public class ArtistHelper {
	public static Artist GetArtistDetails(String artistId) {
		Artist artist = new Artist();
		
		String jsonString = new HttpHelper().getString("https://itunes.apple.com/lookup?id=" + artistId);
		JSONObject rootJson;
		try {
			rootJson = new JSONObject(jsonString);
			JSONObject artistJson = rootJson.getJSONArray("results").getJSONObject(0);
			artist.Id = "" + artistJson.getInt("artistId");
			artist.Name = artistJson.getString("artistName");
			artist.PrimaryGenre = artistJson.getString("primaryGenreName");
			
			artist.Albums = AlbumHelper.GetAlbums(artist.Id);
			artist.Songs = SongHelper.GetSongs(artist.Id);
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return artist;
	}
}
