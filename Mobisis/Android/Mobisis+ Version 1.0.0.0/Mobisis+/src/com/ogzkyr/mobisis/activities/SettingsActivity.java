package com.ogzkyr.mobisis.activities;

import com.ogzkyr.mobisis.R;
import com.ogzkyr.mobisis.extras.Function;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.CompoundButton.OnCheckedChangeListener;
import android.widget.EditText;
import android.widget.SeekBar;
import android.widget.SeekBar.OnSeekBarChangeListener;
import android.widget.TableLayout;
import android.widget.TextView;

public class SettingsActivity extends BaseActivity {
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_settings);
		ActivitySettings();
		HideKeyboard();
		
		final SharedPreferences.Editor editor = sharedData.edit();
		
		final CheckBox serviceState = (CheckBox)findViewById(R.id.cbServiceState);
		final EditText studentID = (EditText)findViewById(R.id.tbStudentID);
		final EditText password = (EditText)findViewById(R.id.tbPassword);
		final SeekBar serviceDelay = (SeekBar)findViewById(R.id.sbServiceDelay);
		final TextView delay = (TextView)findViewById(R.id.tvDelay);
		final CheckBox notificationSound = (CheckBox)findViewById(R.id.cbNotificationSound);
		final CheckBox notificationVibrate = (CheckBox)findViewById(R.id.cbNotificationVibrate);
		final TableLayout serviceSettings = (TableLayout)findViewById(R.id.tlServiceSettings);
		Button save = (Button)findViewById(R.id.btnSave);
		
		
		serviceState.setChecked(Function.isServiceRun(SettingsActivity.this));
		serviceSettings.setVisibility(serviceState.isChecked() ? View.VISIBLE : View.GONE);
		
		
		studentID.setText(sharedData.getString("ServiceOgrenciNo", sharedData.getString("OgrenciNo", null)));
		password.setText(sharedData.getString("ServiceSifre", sharedData.getString("Sifre", null)));
		
		delay.setText("" + sharedData.getInt("ServiceDelay", 60 * 1000 * 5) / 60000 + " dakika");
		serviceDelay.setProgress(sharedData.getInt("ServiceDelay", 60 * 1000 * 5) / 60000);
		notificationSound.setChecked(sharedData.getBoolean("NotificationSound", false));
		notificationVibrate.setChecked(sharedData.getBoolean("NotificationVibrate", false));

		final AsyncTask<Boolean, Void, String> task = new AsyncTask<Boolean, Void, String>() {
			@Override
			protected String doInBackground(Boolean... params) {
	    		Function.stopObisisService(SettingsActivity.this);
				if (params[0])
		    		Function.startObisisService(SettingsActivity.this);
		        else
		    		Function.stopObisisService(SettingsActivity.this);
				return null;
			}
		};
		
		serviceDelay.setOnSeekBarChangeListener(new OnSeekBarChangeListener() {
			@Override
			public void onStopTrackingTouch(SeekBar seekBar) { }
			@Override
			public void onStartTrackingTouch(SeekBar seekBar) { }
			@Override
			public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
				if (progress == 0)
					seekBar.setProgress(1);
				else
					delay.setText("" + progress + " dakika");
			}
		});
		
		serviceState.setOnCheckedChangeListener(new OnCheckedChangeListener() {
		    public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
				serviceSettings.setVisibility(isChecked ? View.VISIBLE : View.GONE);
		    }
		});

		save.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				editor.putString("ServiceOgrenciNo", studentID.getText().toString());
				editor.putString("ServiceSifre", password.getText().toString());
				editor.putInt("ServiceDelay", serviceDelay.getProgress() * 60000);
				editor.putBoolean("NotificationSound", notificationSound.isChecked());
				editor.putBoolean("NotificationVibrate", notificationVibrate.isChecked());
		    	editor.putBoolean("ServiceState", serviceState.isChecked());
    			task.execute(serviceState.isChecked());
				editor.commit();
				ChangePage(new Intent(SettingsActivity.this, SettingsActivity.class), "Ayarlarınız başarıyla kaydedildi!", 0);
			}
		});
	}

}