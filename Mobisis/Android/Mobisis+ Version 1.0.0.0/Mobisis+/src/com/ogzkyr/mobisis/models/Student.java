package com.ogzkyr.mobisis.models;

import org.json.JSONObject;

public class Student {
	
	public String AdSoyad;
	public String Fakulte;
	public String Bolum;
	public int Sinif;
	public double GANO;

	public static Student fromJSON(String json) {
		Student student = new Student();
		try {
			JSONObject obj = new JSONObject(json);
			student.AdSoyad = obj.getString("AdSoyad");
			student.Fakulte = obj.getString("Fakulte");
			student.Bolum = obj.getString("Bolum");
			student.Sinif = obj.getInt("Sinif");
			student.GANO = obj.getDouble("GANO");
			return student;
		} catch (Exception e) {
			return null;
		}
	}
	
}
