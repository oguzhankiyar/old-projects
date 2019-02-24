package com.ogzkyr.mobisis.extras;

import com.ogzkyr.mobisis.R;
import com.ogzkyr.mobisis.activities.*;
import com.ogzkyr.mobisis.tasks.*;

import android.app.Activity;
import android.content.Intent;
import android.os.AsyncTask;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.TableLayout;
import android.widget.TextView;
import android.widget.TableLayout.LayoutParams;

public class LeftMenu {
	public static boolean isOpen = false;
	static TableLayout tl = null;
	public static void Show(final Activity act){
		if (!isOpen){
			LayoutInflater inflater = (LayoutInflater)act.getLayoutInflater();
			View view = inflater.inflate(R.layout.menu_layout, null, false);
			View v = act.findViewById(R.id.Title);
			tl = new TableLayout(act);
			tl.setPadding(0, v.getHeight(), 0, 0);
			tl.addView(view);
			LayoutParams params = new LayoutParams(LayoutParams.MATCH_PARENT, LayoutParams.WRAP_CONTENT);
			act.addContentView(tl, params);
			
			((TextView)view.findViewById(R.id.linkLogin)).setOnClickListener(onClick(new ChangePageAsync(act, new Intent(act, LoginActivity.class))));
			((TextView)view.findViewById(R.id.linkInfo)).setOnClickListener(onClick(new ChangePageAsync(act, new Intent(act, InfoActivity.class), "Bilgileriniz yükleniyor...", 1000)));
			((TextView)view.findViewById(R.id.linkPeriod)).setOnClickListener(onClick(new ChangePageAsync(act, new Intent(act, PeriodActivity.class), "Dönemleriniz yükleniyor...", 0)));
			((TextView)view.findViewById(R.id.linkLesson)).setOnClickListener(onClick(new ChangePageAsync(act, new Intent(act, LessonActivity.class), "Dersleriniz yükleniyor...", 0)));
			((TextView)view.findViewById(R.id.linkFood)).setOnClickListener(onClick(new FoodAsync(act)));
			((TextView)view.findViewById(R.id.linkSettings)).setOnClickListener(onClick(new ChangePageAsync(act, new Intent(act, SettingsActivity.class), "Ayarlar yükleniyor...", 1000)));
			((TextView)view.findViewById(R.id.linkAbout)).setOnClickListener(onClick(new ChangePageAsync(act, new Intent(act, AboutActivity.class), "Sayfa yükleniyor...", 1000)));
		}
		else{
			tl.setVisibility(View.VISIBLE);
		}
		isOpen = true;
	}
	public static void Hide(){
		if (isOpen){
			tl.setVisibility(View.GONE);
			isOpen = false;
		}
	}
	public static void Toggle(Activity act){
		if (isOpen)
			Hide();
		else
			Show(act);
	}
	public static OnClickListener onClick(final AsyncTask<String, Void, String> async){
		return new OnClickListener() {
			@Override
			public void onClick(View v) {
				async.execute();
			}
		};
	}
}