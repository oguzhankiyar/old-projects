package com.ogzkyr.mobisis.activities;

import com.ogzkyr.mobisis.R;
import com.ogzkyr.mobisis.extras.Function;
import com.ogzkyr.mobisis.tasks.LoginAsync;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;

public class LoginActivity extends BaseActivity {
	
	Button btnLogin;
	EditText txtStudentID, txtPassword;
	CheckBox cbRememberMe;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_login);
		ActivitySettings();

		btnLogin = (Button)findViewById(R.id.button1);
		txtStudentID = (EditText)findViewById(R.id.editText1);
		txtPassword = (EditText)findViewById(R.id.editText2);
		cbRememberMe = (CheckBox)findViewById(R.id.checkBox1);
		
		txtStudentID.requestFocus();
		txtStudentID.setText(sharedData.getString("OgrenciNo", null));
		txtPassword.setText(sharedData.getString("Sifre", null));
		
		txtStudentID.addTextChangedListener(new TextWatcher() {
			@Override
			public void onTextChanged(CharSequence s, int start, int before, int count) {
				txtPassword.setText(null);
			}
			@Override
			public void afterTextChanged(Editable s) { }
			@Override
			public void beforeTextChanged(CharSequence s, int start, int count, int after) { }
			
		});
		
		btnLogin.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				String studentID = txtStudentID.getText().toString();
				String password = txtPassword.getText().toString();
				HideKeyboard();
				if (!Function.isInternetConnected(LoginActivity.this)) {
					ChangePage(new Intent(LoginActivity.this, LoginActivity.class), "İnternet bağlantınızı kontrol ediniz!", 2000);
				}
				else if (studentID.length() == 0 || password.length() == 0) {
					ChangePage(new Intent(LoginActivity.this, LoginActivity.class), "Boş alan bıraktınız!", 2000);
				}
				else{
					SharedPreferences.Editor editor = sharedData.edit();
					if(cbRememberMe.isChecked()) {
						editor.putString("OgrenciNo", studentID);
						editor.putString("Sifre", password);
					}
					editor.commit();
					new LoginAsync(LoginActivity.this).execute("http://service.ogzkyr.net/Service.svc/Login/?n=" + studentID + "&p=" + password + "&r=android");
				}
			}
		});
	}
	
}
