package com.ogzkyr.mobisis.extras;

import com.ogzkyr.mobisis.R;

import android.app.Activity;
import android.widget.TextView;

public class MessageDialog {
	
	public static void Show(Activity act, String message){
		act.setContentView(R.layout.layout_dialog);
		TextView text = (TextView)act.findViewById(R.id.tvDialogMessage);
		text.setText(message);
	}
}
