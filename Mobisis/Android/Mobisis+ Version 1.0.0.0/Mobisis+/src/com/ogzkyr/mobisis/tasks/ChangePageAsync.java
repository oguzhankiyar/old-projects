package com.ogzkyr.mobisis.tasks;

import com.ogzkyr.mobisis.extras.MessageDialog;

import android.app.Activity;
import android.content.Intent;
import android.os.AsyncTask;

public class ChangePageAsync extends AsyncTask<String, Void, String> {
	Activity act;
	String message;
	Intent intent;
	long time;
	public ChangePageAsync(Activity act, Intent intent){
		this.act = act;
		this.intent = intent;
		this.message = null;
		this.time = 0;
	}
	public ChangePageAsync(Activity act, Intent intent, String message, long time){
		this.act = act;
		this.intent = intent;
		this.message = message;
		this.time = time;
	}
	@Override
    protected void onPreExecute() {
		if (message != null)
			MessageDialog.Show(act, message);
    }
	@Override
	protected String doInBackground(String... params) {
		if (time != 0)
			try { Thread.sleep(time); } catch (InterruptedException e) { }
        return null;
    }
	@Override
    protected void onPostExecute(String result) {
		act.startActivity(intent);
    }
}
