package com.ogzkyr.mobisis.extras;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;

import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;

import com.ogzkyr.mobisis.activities.LoginActivity;
import com.ogzkyr.mobisis.service.ObisisService;
import com.ogzkyr.mobisis.tasks.ChangePageAsync;

import android.app.Activity;
import android.app.ActivityManager;
import android.app.AlarmManager;
import android.app.PendingIntent;
import android.app.ActivityManager.RunningServiceInfo;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.SystemClock;

public class Function {
	static AlarmManager alarm;
	static PendingIntent pIntent;
	public static boolean isInternetConnected(Context ctx){
		ConnectivityManager conMgr = (ConnectivityManager)ctx.getSystemService(Context.CONNECTIVITY_SERVICE);
		NetworkInfo i = conMgr.getActiveNetworkInfo();
		  if (i == null)
		    return false;
		  if (!i.isConnected())
		    return false;
		  if (!i.isAvailable())
		    return false;
		  return true;
	}
	public static void ShowError(Activity act) {
		new ChangePageAsync(act, new Intent(act, LoginActivity.class), "Bir sorun oluştu.\nDaha sonra tekrar deneyiniz..", 2000).execute();
	}
	public static String Connect(String URL){
        HttpClient httpclient = new DefaultHttpClient();
        HttpResponse response;
        try {
            HttpPost httpPost = new HttpPost(URL);
            response = httpclient.execute(httpPost);
            StringBuilder sb = new StringBuilder();
            InputStream is = response.getEntity().getContent();
            BufferedReader reader = new BufferedReader(new InputStreamReader(is, "utf-8"), 8);
            sb = new StringBuilder();
            sb.append(reader.readLine() + "\n");
            String line = "0";

            while ((line = reader.readLine()) != null) {
                sb.append(line + "\n");
            }

            is.close();
            return sb.toString();
        }catch (Exception e) {
    		e.printStackTrace();
    		return null;
        }
	}
	public static void startObisisService(Context ctx){
		SharedPreferences sharedData = ctx.getSharedPreferences("mobisis", 0);
		if (!isServiceRun(ctx) && sharedData.getBoolean("ServiceState", false)) {
			if (sharedData.getString("OgrenciNo", null) != null && sharedData.getString("Sifre", null) != null) {
				int delay = sharedData.getInt("ServiceDelay", 60 * 1000 * 5);
				Intent intent = new Intent(ctx, ObisisService.class);
				pIntent = PendingIntent.getService(ctx, 0, intent, 0);
				alarm = (AlarmManager)ctx.getSystemService(Context.ALARM_SERVICE);
				alarm.setRepeating(AlarmManager.RTC_WAKEUP, SystemClock.currentThreadTimeMillis(), delay, pIntent);
			}
		}
	}
	public static void stopObisisService(Context ctx){
		if (isServiceRun(ctx)) {
			ctx.stopService(new Intent(ctx, ObisisService.class));
			alarm.cancel(pIntent);
		}
	}
	public static boolean isServiceRun(Context ctx){
		ActivityManager serviceManager = (ActivityManager)ctx.getSystemService(Context.ACTIVITY_SERVICE);
		for(RunningServiceInfo service : serviceManager.getRunningServices(Integer.MAX_VALUE)) {
			if(ObisisService.class.getName().equals(service.service.getClassName())) {
				return true;
			}
		}
		return false;
	}

}
