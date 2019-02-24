package com.ogzkyr.mobisis.models;

import java.util.List;

import org.json.JSONObject;

public class Lesson {
	
	public String Kod;
	public String Adi;
	public int Sinif;
	public Integer Vize1;
	public Integer Vize2;
	public Integer Vize3;
	public Integer Final;
	public Integer Butunleme;
	public Integer Ortalama;
	public String HarfNotu;
	public String Durum;
	public double Kredi;
	public double BasariKatsayisi;
	public boolean OrtalamayaEtki;

	public static Lesson fromJSON(String json) {
		Lesson lesson = new Lesson();
		try {
			JSONObject obj = new JSONObject(json);
			lesson.Kod = obj.getString("Kod");
			lesson.Adi = obj.getString("Adi");
			lesson.Vize1 = obj.isNull("Vize1") ? null : obj.getInt("Vize1");
			lesson.Vize2 = obj.isNull("Vize2") ? null : obj.getInt("Vize2");
			lesson.Vize3 = obj.isNull("Vize3") ? null : obj.getInt("Vize3");
			lesson.Final = obj.isNull("Final") ? null : obj.getInt("Final");
			lesson.Butunleme = obj.isNull("Butunleme") ? null : obj.getInt("Butunleme");
			lesson.Ortalama = obj.isNull("Ortalama") ? null : obj.getInt("Ortalama");
			lesson.HarfNotu = obj.isNull("HarfNotu") ? null : obj.getString("HarfNotu");
			lesson.Durum = obj.isNull("Durum") ? null : obj.getString("Durum");
			lesson.Kredi = obj.getDouble("Kredi");
			lesson.BasariKatsayisi = obj.getDouble("BasariKatsayisi");
			lesson.OrtalamayaEtki = obj.getBoolean("OrtalamayaEtki");
			return lesson;
		} catch (Exception e) {
			return null;
		}
	}
	public static boolean isContainsLesson(List<Lesson> lessons, Lesson lesson) {
		for (Lesson les : lessons) {
			if (les.Kod.equalsIgnoreCase(lesson.Kod))
				return true;
		}
		return false;
	}
}
