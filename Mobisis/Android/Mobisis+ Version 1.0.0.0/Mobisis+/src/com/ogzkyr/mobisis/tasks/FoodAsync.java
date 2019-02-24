package com.ogzkyr.mobisis.tasks;

import org.json.JSONArray;

import com.ogzkyr.mobisis.activities.FoodActivity;
import com.ogzkyr.mobisis.activities.LoginActivity;
import com.ogzkyr.mobisis.adapters.FoodAdapter;
import com.ogzkyr.mobisis.extras.Function;
import com.ogzkyr.mobisis.extras.MessageDialog;

import android.app.Activity;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.AsyncTask;
import android.support.v4.view.PagerAdapter;

public class FoodAsync extends AsyncTask<String, Void, String> {

	Activity act;
	SharedPreferences data;
	SharedPreferences.Editor editor;
	JSONArray array;
	PagerAdapter adapter;
	
	public FoodAsync(Activity activity) {
		this.act = activity;
		this.data = activity.getSharedPreferences("mobisis", 0);
		this.editor = data.edit();
		this.array = null;
	}
    protected void onPreExecute() {
    	MessageDialog.Show(act, "Yemek listesi yükleniyor..");
    }
	protected String doInBackground(String... urls) {
		try {
			String json = data.getString("YemekJSON", null);
			if (json != null) {
				array = new JSONArray(json);
				adapter = new FoodAdapter(act, array);
				if (FoodAdapter.currentIndex + 1 != array.length())
					return "1";
			}
			json = Function.Connect("http://service.ogzkyr.net/Service.svc/GetFood/?r=android");
			array = new JSONArray(json);
			editor.putString("YemekJSON", json);
			editor.commit();
			
			adapter = new FoodAdapter(act, array);
			
			if (FoodAdapter.currentIndex + 1 != array.length())
				return "1";
			else
				return "-1";
			
		} catch (Exception e) { }
		return null;
	}
	protected void onPostExecute(String result){
		if (result == null)
			new ChangePageAsync(act, new Intent(act, LoginActivity.class), "Yemek listesi şuanda çekilemiyor..", 2000).execute();	
		else {
			FoodActivity.adapter = adapter;
			if (result == "-1")
				new ChangePageAsync(act, new Intent(act, FoodActivity.class), "Bugüne ait yemek listesine erişilemiyor..", 1000).execute();
			else	
				new ChangePageAsync(act, new Intent(act, FoodActivity.class)).execute();
		}
	}

}
