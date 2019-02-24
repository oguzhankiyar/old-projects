package com.ogzkyr.mobisis.adapters;

import java.util.List;

import com.ogzkyr.mobisis.R;
import com.ogzkyr.mobisis.models.Lesson;

import android.app.Activity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

public class LessonAdapter extends BaseAdapter {

	private List<Lesson> lessons;
	private static LayoutInflater inflater = null;
	
	public LessonAdapter(Activity act, List<Lesson> list){
		this.lessons = list;
		inflater = (LayoutInflater)act.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
	}
	@Override
	public int getCount() {
		return lessons.size();
	}

	@Override
	public Lesson getItem(int arg0) {
		return lessons.get(arg0);
	}

	@Override
	public long getItemId(int arg0) {
		return arg0;
	}

	@Override
	public View getView(int position, View view, ViewGroup viewGroup) {
		if (view == null)
			view = inflater.inflate(R.layout.lesson_list_row, null);
		TextView lessonName = (TextView)view.findViewById(R.id.lessonName);
		TextView lessonInfo1 = (TextView)view.findViewById(R.id.lessonInfo1);
		TextView lessonInfo2 = (TextView)view.findViewById(R.id.lessonInfo2);
		TextView lessonInfo3 = (TextView)view.findViewById(R.id.lessonInfo3);

		Lesson lesson = (Lesson)getItem(position);
		lessonName.setText(lesson.Adi);
		String text1 = "", text2 = "", text3 = "";
		if (lesson.Vize1 == null) text1 += "V1: - \n";
		else text1 += "V1: " + lesson.Vize1 + "\n";

		if (lesson.Vize2 == null) text1 += "V2: - \n";
		else text1 += "V2: " + lesson.Vize2 + "\n";

		if (lesson.Vize3 == null) text1 += "V3: - ";
		else text1 += "V3: " + lesson.Vize3;

		if (lesson.Final == null) text2 += "FNL: - \n";
		else text2 += "FNL: " + lesson.Final + "\n";

		if (lesson.Butunleme == null) text2 += "BÜT: - \n";
		else text2 += "BÜT: " + lesson.Butunleme + "\n";

		if (lesson.Ortalama == null) text2 += "ORT: - ";
		else text2 += "ORT: " + lesson.Ortalama;
		
		if (lesson.HarfNotu != null) text3 += lesson.HarfNotu + "\n";
		else text3 += "\n";
		
		if (lesson.Durum != null) text3 += lesson.Durum;
		
		lessonInfo1.setText(text1);
		lessonInfo2.setText(text2);
		lessonInfo3.setText(text3);
		return view;
	}

}
