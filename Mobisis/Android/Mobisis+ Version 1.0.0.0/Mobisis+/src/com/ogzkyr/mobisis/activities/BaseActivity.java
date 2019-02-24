package com.ogzkyr.mobisis.activities;

import com.ogzkyr.mobisis.R;
import com.ogzkyr.mobisis.extras.LeftMenu;
import com.ogzkyr.mobisis.models.PageStack;
import com.ogzkyr.mobisis.tasks.ChangePageAsync;

import android.app.Activity;
import android.app.Service;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.IBinder;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.View.OnTouchListener;
import android.view.inputmethod.InputMethodManager;
import android.widget.RelativeLayout;
import android.widget.TextView;

public class BaseActivity extends Activity {
   
	public RelativeLayout ActivityLayout;
	static PageStack pageStack = new PageStack();
	SharedPreferences sharedData;
	
	public void ActivitySettings(){

		pageStack.addPage(getIntent());
		
		sharedData = getSharedPreferences("mobisis", 0);
		
		ActivityLayout = (RelativeLayout)findViewById(R.id.Activity);
		ActivityLayout.setOnTouchListener(new OnTouchListener() {
			public boolean onTouch(View v, MotionEvent event) {
				if(v == ActivityLayout){
					LeftMenu.Hide();
					HideKeyboard();
					return true;
				}
				return false;
			}
		});
		
    	final TextView tv;
    	tv = (TextView)findViewById(R.id.MenuLink);
    	tv.setPadding(20, findViewById(R.id.Title).getHeight() / 2 + 10, 10, 10);
    	tv.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View arg0) {
				View tl;
				tl = (View)findViewById(R.id.Container);
				tl.setClickable(false);
				
				HideKeyboard();
		    	LeftMenu.Toggle(BaseActivity.this);
			}
		});
    }
	public void ChangePage(Intent intent){
		new ChangePageAsync(this, intent).execute();
	}
	public void ChangePage(Intent intent, String message, long time){
		new ChangePageAsync(this, intent, message, time).execute();
	}
	public void HideKeyboard(){
		InputMethodManager imm;
		imm = (InputMethodManager)this.getSystemService(Service.INPUT_METHOD_SERVICE);
		imm.hideSoftInputFromWindow((IBinder)ActivityLayout.getWindowToken(), 0);
	}
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent event) {
	    if (keyCode == KeyEvent.KEYCODE_BACK) {
	    	try {
		    	if (pageStack.getPageIntent() != null) {
			    	pageStack.deletePage();
			    	Intent head = pageStack.getPageIntent();
			    	pageStack.deletePage();
			    	startActivity(head);
		    	}
		    	else {
		    		System.exit(0);
		    	}
	    	}
	    	catch(Exception ex) {
	    		System.exit(0);
	    	}
	        return true;
	    }
	    return super.onKeyDown(keyCode, event);
	}
}
