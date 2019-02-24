package musicbox.models;

import java.util.*;

public class Album extends BaseObject {
	public String Id;
	public String Name;
	public String ImageUrl;
	public Artist Artist;
	public List<Song> Songs;
}