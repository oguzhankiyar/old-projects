package com.ogzkyr.mobisis.models;

import java.util.ArrayList;
import java.util.List;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class Period {
	
	public int No;
	public String Adi;
	public String OgretimYili;
	public int OgretimYiliKodu;
	public double GANO;

	public static Period fromJSON(String json) {
		Period period = new Period();
		try {
			JSONObject obj = new JSONObject(json);
			period.No = obj.getInt("No");
			period.Adi = obj.getString("Adi");
			period.OgretimYili = obj.getString("OgretimYili");
			period.OgretimYiliKodu = obj.getInt("OgretimYiliKodu");
			period.GANO = obj.getDouble("GANO");
			return period;
		} catch (Exception e) {
			return null;
		}
	}
	
	public static List<Lesson> GetLessons(String json){
		List<Lesson> lessons = new ArrayList<Lesson>();
		JSONObject jobj;
		try {
			jobj = new JSONObject(json);
			JSONArray array = jobj.getJSONArray("Dersler");
			for (int i = 0;i < array.length();i++){
				JSONObject obj = array.getJSONObject(i);
				Lesson lesson = new Lesson();
				lesson.Kod = obj.getString("Kod");
				lesson.Adi = obj.getString("Adi");
				lesson.Vize1 = obj.getInt("Vize1");
				lesson.Vize2 = obj.getInt("Vize2");
				lesson.Vize3 = obj.getInt("Vize3");
				lesson.Final = obj.getInt("Final");
				lesson.Butunleme = obj.getInt("Butunleme");
				lesson.Ortalama = obj.getInt("Ortalama");
				lesson.HarfNotu = obj.getString("HarfNotu");
				lesson.Durum = obj.getString("Durum");
				lessons.add(lesson);
			}
		} catch (JSONException e) {
			e.printStackTrace();
		}
		return lessons;
	}
	
}
