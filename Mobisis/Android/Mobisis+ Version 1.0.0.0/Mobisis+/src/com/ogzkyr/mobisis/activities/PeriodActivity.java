package com.ogzkyr.mobisis.activities;

import java.util.ArrayList;
import java.util.List;

import org.json.JSONArray;

import com.ogzkyr.mobisis.R;
import com.ogzkyr.mobisis.adapters.PeriodAdapter;
import com.ogzkyr.mobisis.extras.Function;
import com.ogzkyr.mobisis.models.Period;
import com.ogzkyr.mobisis.tasks.ChangePageAsync;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.ListView;

public class PeriodActivity extends BaseActivity {
	
	ListView lv;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_period);
		ActivitySettings();

		lv = (ListView)findViewById(R.id.listPeriods);

		List<Period> periods = new ArrayList<Period>();
		try {
			JSONArray array = new JSONArray(sharedData.getString("DonemlerJSON", null));
			for (int i = 0;i < array.length();i++)
				periods.add(Period.fromJSON(array.getJSONObject(i).toString()));
		} catch (Exception ex) {
			Function.ShowError(PeriodActivity.this);
		}
		lv.setAdapter(new PeriodAdapter(this, periods));
		lv.setOnItemClickListener(new OnItemClickListener() {
			@Override
			public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
				Object obj = lv.getItemAtPosition(position);
				Period period = (Period)obj;
				Intent intent = new Intent(PeriodActivity.this, LessonActivity.class);
				intent.putExtra("DonemNo", period.No);
				intent.putExtra("OgretimYiliKodu", period.OgretimYiliKodu);
				new ChangePageAsync(PeriodActivity.this, intent, "Dersleriniz yükleniyor...", 2000).execute();
			}
		});
	}

}
