package com.ogzkyr.mobisis.models;

import org.json.JSONObject;

public class Connection {

	public int OgrenciNo;
	public String Sifre;
	public boolean Durum;
	public String Mesaj;
	
	public static Connection fromJSON(String json) {
		Connection con = new Connection();
		try {
			JSONObject obj = new JSONObject(json);
			con.OgrenciNo = obj.getInt("OgrenciNo");
			con.Sifre = obj.getString("Sifre");
			con.Durum = obj.getBoolean("Durum");
			con.Mesaj = obj.isNull("Mesaj") ? null : obj.getString("Mesaj");
			return con;
		} catch (Exception e) {
			return null;
		}
	}
	
}
