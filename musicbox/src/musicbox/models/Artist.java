package musicbox.models;

import java.util.*;

public class Artist extends BaseObject {
	public String Id;
	public String Name;
	public String PrimaryGenre;
	public List<Album> Albums;
	public List<Song> Songs;
}