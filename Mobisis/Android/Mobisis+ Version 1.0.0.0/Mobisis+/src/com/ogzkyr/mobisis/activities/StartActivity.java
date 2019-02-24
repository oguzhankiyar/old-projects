package com.ogzkyr.mobisis.activities;

import com.ogzkyr.mobisis.R;

import android.os.Bundle;
import android.app.Activity;
import android.content.Intent;

public class StartActivity extends Activity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_start);
		Thread timer = new Thread() {
			public void run() {
				try{
					sleep(2000);
				} catch(Exception e) {
					e.printStackTrace();
				} finally {
					startActivity(new Intent(StartActivity.this, LoginActivity.class));
				}
			}
		};
		timer.start();
	}
	
}
