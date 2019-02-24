package com.ogzkyr.mobisis.activities;

import com.ogzkyr.mobisis.R;
import com.ogzkyr.mobisis.extras.Function;
import com.ogzkyr.mobisis.models.Student;

import android.os.Bundle;
import android.widget.TextView;

public class InfoActivity extends BaseActivity {
	
	TextView tvAdSoyad, tvFakulte, tvBolum, tvSinif, tvGANO;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_info);
		ActivitySettings();

		tvAdSoyad = (TextView)findViewById(R.id.tvAdSoyad);
		tvFakulte = (TextView)findViewById(R.id.tvFakulte);
		tvBolum = (TextView)findViewById(R.id.tvBolum);
		tvSinif = (TextView)findViewById(R.id.tvSinif);
		tvGANO = (TextView)findViewById(R.id.tvGANO);
		
		try {
			Student student = Student.fromJSON(sharedData.getString("OgrenciJSON", null));
			if (student == null)
				throw new Exception();
			tvAdSoyad.setText(student.AdSoyad);
			tvFakulte.setText(student.Fakulte);
			tvBolum.setText(student.Bolum);
			tvSinif.setText("" + student.Sinif);
			tvGANO.setText("" + student.GANO);
		} catch(Exception ex) {
			Function.ShowError(InfoActivity.this);
		}
	}

}
