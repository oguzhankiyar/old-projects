package com.ogzkyr.mobisis.service;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import com.ogzkyr.mobisis.R;
import com.ogzkyr.mobisis.extras.Function;
import com.ogzkyr.mobisis.models.Lesson;
import com.ogzkyr.mobisis.tasks.*;

import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.IBinder;
import android.support.v4.app.NotificationCompat;

public class ObisisService extends Service {

	@Override
	public int onStartCommand(Intent intent, int flags, int startId) {
		if (Function.isInternetConnected(ObisisService.this)) {
			SharedPreferences sharedData = getSharedPreferences("mobisis", 0);
			SharedPreferences.Editor editor = sharedData.edit();
			
			String studentID = sharedData.getString("ServiceOgrenciNo", sharedData.getString("OgrenciNo", null));
			String password = sharedData.getString("ServiceSifre", sharedData.getString("Sifre", null));
			if (studentID != null || password != null) {
				try {
					String json = new ConnectAsync().execute("http://service.ogzkyr.net/Service.svc/CurrentPeriod/?n=" + studentID + "&p=" + password + "&r=android").get();
					
					JSONObject currentPeriod = new JSONObject(json);
					String periodData = currentPeriod.toString();
					String lessonData = sharedData.getString("DonemJSON_" + currentPeriod.getString("No") + "_" + currentPeriod.getString("OgretimYiliKodu"), null);
					JSONObject oldPeriod = new JSONObject(lessonData);
					
					if (!lessonData.equalsIgnoreCase(periodData)) {
			        	CompareLessons(oldPeriod, currentPeriod);
						editor.putString("DonemJSON_" + currentPeriod.getString("No") + "_" + currentPeriod.getString("OgretimYiliKodu"), periodData);
						editor.commit();
					}
				} catch (Exception e) { }
			}
		}
		return START_STICKY;
	}
	public void CompareLessons(JSONObject oldPeriod, JSONObject newPeriod){
		try {
			JSONArray oldArray = oldPeriod.getJSONArray("Dersler");
			JSONArray newArray = newPeriod.getJSONArray("Dersler");
			for (int i=0;i<newArray.length();i++){
				JSONObject oldObject = oldArray.getJSONObject(i);
				JSONObject newObject = newArray.getJSONObject(i);
				Lesson oldLesson = Lesson.fromJSON(oldObject.toString());
				Lesson newLesson = Lesson.fromJSON(newObject.toString());
				String text = "";
				if (oldLesson.Vize1 != newLesson.Vize1)
					text += "1. Vize: " + newLesson.Vize1 + " ";
				if (oldLesson.Vize2 != newLesson.Vize2)
					text += "2. Vize: " + newLesson.Vize2 + " ";
				if (oldLesson.Vize3 != newLesson.Vize3)
					text += "3. Vize: " + newLesson.Vize3 + " ";
				if (oldLesson.Final != newLesson.Final)
					text += "Final: " + newLesson.Final + " ";
				if (oldLesson.Butunleme != newLesson.Butunleme)
					text += "Bütünleme: " + newLesson.Butunleme + " ";
				if (text != "") {
					Intent intent = new Intent("android.intent.action.LESSONDETAILS");
					intent.putExtra("DersJSON", newObject.toString());
					showNotification(Integer.parseInt(newLesson.Kod.split(" ")[1]),
							newLesson.Adi,
							newLesson.Adi,
							text,
							intent);
				}
			}
		} catch (JSONException e) {
			e.printStackTrace();
		}
	}
	
	public void showNotification(int id, String tickerText, String title, String text, Intent intent) {
		NotificationCompat.Builder builder = new NotificationCompat.Builder(getApplicationContext());

		PendingIntent pendingIntent = PendingIntent.getActivity(this, PendingIntent.FLAG_UPDATE_CURRENT, intent, 0);

		SharedPreferences sharedData = getSharedPreferences("mobisis", 0);
		builder.setAutoCancel(true)
	     .setDefaults(Notification.DEFAULT_ALL)
	     .setWhen(System.currentTimeMillis())         
	     .setSmallIcon(R.drawable.ic_launcher)
	     .setTicker(tickerText)
	     .setContentTitle(title)
	     .setContentText(text)
	     .setContentIntent(pendingIntent);
		if (sharedData.getBoolean("NotificationSound", false))
			builder.setDefaults(Notification.DEFAULT_SOUND);
		if (sharedData.getBoolean("NotificationVibrate", false))
			builder.setDefaults(Notification.DEFAULT_VIBRATE);
		
		NotificationManager notificationManager = (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);
		
		notificationManager.notify(id, builder.build());
	}

	@Override
	public IBinder onBind(Intent intent) {
		return null;
	}
}