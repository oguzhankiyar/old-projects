package com.ogzkyr.mobisis.activities;

import com.ogzkyr.mobisis.R;
import com.ogzkyr.mobisis.extras.Function;
import com.ogzkyr.mobisis.models.Lesson;

import android.os.Bundle;
import android.view.View;
import android.widget.TextView;

public class LessonDetailsActivity extends BaseActivity {

	TextView tvAdi, tvVize1, tvVize2, tvVize3, tvFinal, tvButunleme, tvOrtalama, tvDurum, tvHarfNotu, tvKredi,
	tvVize2Text, tvVize3Text, tvButunlemeText, tvOrtalamaText, tvHarfNotuText, tvDurumText;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_lesson_details);
		ActivitySettings();
		
		tvAdi = (TextView)findViewById(R.id.tvAdi);
		
		tvVize2Text = (TextView)findViewById(R.id.tvVize2Text);
		tvVize3Text = (TextView)findViewById(R.id.tvVize3Text);
		tvButunlemeText = (TextView)findViewById(R.id.tvButunlemeText);
		tvOrtalamaText = (TextView)findViewById(R.id.tvOrtalamaText);
		tvHarfNotuText = (TextView)findViewById(R.id.tvHarfNotuText);
		tvDurumText = (TextView)findViewById(R.id.tvDurumText);

		tvKredi = (TextView)findViewById(R.id.tvKredi);
		tvVize1 = (TextView)findViewById(R.id.tvVize1);
		tvVize2 = (TextView)findViewById(R.id.tvVize2);
		tvVize3 = (TextView)findViewById(R.id.tvVize3);
		tvFinal = (TextView)findViewById(R.id.tvFinal);
		tvButunleme = (TextView)findViewById(R.id.tvButunleme);
		tvOrtalama = (TextView)findViewById(R.id.tvOrtalama);
		tvHarfNotu = (TextView)findViewById(R.id.tvHarfNotu);
		tvDurum = (TextView)findViewById(R.id.tvDurum);
		
		try {
			String lessonJSON = getIntent().getExtras().getString("DersJSON");
			Lesson lesson = Lesson.fromJSON(lessonJSON);
			
			tvAdi.setText(lesson.Kod + " " + lesson.Adi);

			tvKredi.setText("" + lesson.Kredi);
			tvVize1.setText("" + (lesson.Vize1 == null ? " - " : "" + lesson.Vize1));
			tvVize2.setText("" + lesson.Vize2);
			tvVize3.setText("" + lesson.Vize3);
			tvFinal.setText("" + (lesson.Final == null ? " - " : "" + lesson.Final));
			tvButunleme.setText("" + lesson.Butunleme);
			tvOrtalama.setText("" + (lesson.Ortalama == null ? " - " : "" + lesson.Ortalama));
			tvHarfNotu.setText("" + lesson.HarfNotu);
			tvDurum.setText("" + lesson.Durum);
			
			if (lesson.Vize2 == null) {
				tvVize2Text.setVisibility(View.GONE);
				tvVize2.setVisibility(View.GONE);
			}
			if (lesson.Vize3 == null) {
				tvVize3Text.setVisibility(View.GONE);
				tvVize3.setVisibility(View.GONE);
			}
			if (lesson.Butunleme == null) {
				tvButunlemeText.setVisibility(View.GONE);
				tvButunleme.setVisibility(View.GONE);
			}
			if (lesson.Durum == null) {
				tvDurumText.setVisibility(View.GONE);
				tvDurum.setVisibility(View.GONE);
			}
			if (lesson.HarfNotu == null) {
				tvHarfNotuText.setVisibility(View.GONE);
				tvHarfNotu.setVisibility(View.GONE);
			}
		} catch (Exception e) {
			Function.ShowError(LessonDetailsActivity.this);
		}
	}
	
}
