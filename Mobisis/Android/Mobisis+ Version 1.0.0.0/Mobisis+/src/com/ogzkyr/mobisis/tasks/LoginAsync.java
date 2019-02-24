package com.ogzkyr.mobisis.tasks;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import com.ogzkyr.mobisis.activities.InfoActivity;
import com.ogzkyr.mobisis.activities.LoginActivity;
import com.ogzkyr.mobisis.extras.Function;
import com.ogzkyr.mobisis.extras.MessageDialog;
import com.ogzkyr.mobisis.models.Connection;

import android.app.Activity;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.AsyncTask;

public class LoginAsync extends AsyncTask<String, Void, String> {
	
	Activity act;
	
	public LoginAsync(Activity act){
		this.act = act;
	}

    protected void onPreExecute() {
    	MessageDialog.Show(act, "Giriş yapılıyor...");
    }
	
	protected String doInBackground(String... urls) {
        try {
        	return Function.Connect(urls[0]);
        }catch (Exception e) {
    		return null;
        }
	}

    protected void onPostExecute(String result){
		try {
			JSONObject obj = new JSONObject(result);
			Connection con = Connection.fromJSON(result);
			if(con.Durum == true) {
				SharedPreferences sharedData = act.getSharedPreferences("mobisis", 0);
				SharedPreferences.Editor editor = sharedData.edit();
				JSONObject obj2 = obj.getJSONObject("Ogrenci");
				editor.putString("OgrenciJSON", obj2.toString());
				
				String periodsJSON = obj2.getJSONArray("Donemler").toString();
				editor.putString("DonemlerJSON", periodsJSON);
				JSONArray periodsArray = new JSONArray(periodsJSON);
				for (int i = 0;i < periodsArray.length();i++){
					JSONObject periodObject = periodsArray.getJSONObject(i);
					editor.putString("DonemJSON_" + periodObject.getString("No") + "_" + periodObject.getString("OgretimYiliKodu"), periodObject.toString());
					if (i == 0) {
						editor.putString("sonDonem_No", periodObject.getString("No"));
						editor.putString("sonDonem_OgretimYiliKodu", periodObject.getString("OgretimYiliKodu"));
					}
				}
				editor.commit();
				
				if (con.Mesaj != null)
					new ChangePageAsync(act, new Intent(act, InfoActivity.class), con.Mesaj, 2000).execute();
				else
					new ChangePageAsync(act, new Intent(act, InfoActivity.class)).execute();
			}
			else {
				if (con.Mesaj != null)
					new ChangePageAsync(act, new Intent(act, LoginActivity.class),con.Mesaj, 2000).execute();
				else
					new ChangePageAsync(act, new Intent(act, LoginActivity.class), "Bilgileriniz hatalı!\nTekrar deneyiniz.", 2000).execute();
			}
		} catch (JSONException ex) {
			new ChangePageAsync(act, new Intent(act, LoginActivity.class), "Sunucuya bağlanılamıyor!\nDaha sonra tekrar deneyiniz.", 2000).execute();
		}
	}
    
}
