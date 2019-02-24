package com.ogzkyr.mobisis.activities;

import java.util.ArrayList;
import java.util.List;

import org.json.JSONArray;
import org.json.JSONObject;

import com.ogzkyr.mobisis.R;
import com.ogzkyr.mobisis.adapters.LessonAdapter;
import com.ogzkyr.mobisis.extras.Function;
import com.ogzkyr.mobisis.models.Lesson;
import com.ogzkyr.mobisis.tasks.ChangePageAsync;

import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;
import android.widget.AdapterView.OnItemClickListener;

public class LessonActivity extends BaseActivity {
	
	ListView lv;
	TextView title;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_lesson);
		ActivitySettings();
		
		lv = (ListView)findViewById(R.id.listLessons);
		title = (TextView)findViewById(R.id.Title);

		Bundle bundleData = getIntent().getExtras();
		
		final List<Lesson> lessons = new ArrayList<Lesson>();
		try {
			String DonemNo = null;
			String OgretimYiliKodu = null;
			try {
				DonemNo = "" + bundleData.getInt("DonemNo");
				OgretimYiliKodu = "" + bundleData.getInt("OgretimYiliKodu");
			} catch (Exception e) {
				DonemNo = sharedData.getString("sonDonem_No", null);
				OgretimYiliKodu = sharedData.getString("sonDonem_OgretimYiliKodu", null);
			}
			String result = sharedData.getString("DonemJSON_" + DonemNo + "_" + OgretimYiliKodu + "", null);
			JSONObject obj = new JSONObject(result);
			title.setText(obj.getString("OgretimYili") + " " + obj.getString("Adi"));

			final JSONArray array = obj.getJSONArray("Dersler");
			for (int i = 0;i < array.length();i++)
				lessons.add(Lesson.fromJSON(array.getJSONObject(i).toString()));
			lv.setAdapter(new LessonAdapter(this, lessons));
			lv.setOnItemClickListener(new OnItemClickListener() {
				@Override
				public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
					Intent intent = new Intent(LessonActivity.this, LessonDetailsActivity.class);
					try {
						intent.putExtra("DersJSON", array.getJSONObject(position).toString());
					} catch (Exception e) {
						Function.ShowError(LessonActivity.this);
					}
					new ChangePageAsync(LessonActivity.this, intent, "Ders yükleniyor...", 2000).execute();
				}
			});
			AsyncTask<String, Void, Boolean> task = new AsyncTask<String, Void, Boolean>() {
				List<Lesson> newLessons = new ArrayList<Lesson>();
				@Override
				protected void onPreExecute() {
					Toast.makeText(LessonActivity.this, "Dersler güncelleniyor..", Toast.LENGTH_SHORT).show();
				}
				@Override
				protected Boolean doInBackground(String... params) {
					String studentID = sharedData.getString("ServiceOgrenciNo", sharedData.getString("OgrenciNo", null));
					String password = sharedData.getString("ServiceSifre", sharedData.getString("Sifre", null));
					if (studentID == null || password == null)
						return false;
					try {
						String json = Function.Connect("http://service.ogzkyr.net/Service.svc/CurrentPeriod/?n=" + studentID + "&p=" + password + "&r=android");

						JSONObject currentPeriod = new JSONObject(json);
						JSONArray lessonArray = currentPeriod.getJSONArray("Dersler");
						for (int i=0;i<lessonArray.length();i++) {
							Lesson lesson = Lesson.fromJSON(lessonArray.getJSONObject(i).toString());
							newLessons.add(lesson);
						}
					} catch (Exception e) {
						return false;
					}
					return true;
				}
				@Override
				protected void onPostExecute(Boolean result) {
					if (result)
						lv.setAdapter(new LessonAdapter(LessonActivity.this, newLessons));
					Toast.makeText(LessonActivity.this, "Dersler güncellendi..", Toast.LENGTH_SHORT).show();
				}
			};
			task.execute();
		} catch (Exception ex) {
			Function.ShowError(LessonActivity.this);
		}
	}
	
}
