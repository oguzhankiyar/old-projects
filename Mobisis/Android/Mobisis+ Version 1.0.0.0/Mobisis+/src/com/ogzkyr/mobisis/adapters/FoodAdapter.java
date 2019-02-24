package com.ogzkyr.mobisis.adapters;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import com.ogzkyr.mobisis.R;

import android.content.Context;
import android.os.Parcelable;
import android.support.v4.view.PagerAdapter;
import android.support.v4.view.ViewPager;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.TextView;

public class FoodAdapter extends PagerAdapter {

	Context ctx;
	ArrayList<View> views;
	LayoutInflater layoutInflater;
	public static int currentIndex = -1;
		
	public FoodAdapter(Context ctx, JSONArray array) {
		this.ctx = ctx;
		this.views = new ArrayList<View>();
		this.layoutInflater = (LayoutInflater)ctx.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
		currentIndex = 0;
		for (int i=0;i<array.length();i++) {
			JSONObject obj = null;
			try {
				obj = array.getJSONObject(i);
			} catch (JSONException e) {
				e.printStackTrace();
			}
			views.add(createView(obj));
		}
	}

	public View createView(JSONObject obj) {
		View view = layoutInflater.inflate(R.layout.layout_food, null);

		TextView tvDate = (TextView)view.findViewById(R.id.tvDate);
		TextView tvFirstFood = (TextView)view.findViewById(R.id.tvFirstFood);
		TextView tvSecondFood = (TextView)view.findViewById(R.id.tvSecondFood);
		TextView tvThirdFood = (TextView)view.findViewById(R.id.tvThirdFood);
		TextView tvFirstCal = (TextView)view.findViewById(R.id.tvFirstCal);
		TextView tvSecondCal = (TextView)view.findViewById(R.id.tvSecondCal);
		TextView tvThirdCal = (TextView)view.findViewById(R.id.tvThirdCal);
		
		try {
			JSONArray foodArray = obj.getJSONArray("Yemekler");
			JSONObject firstObj = foodArray.getJSONObject(0);
			JSONObject secondObj = foodArray.getJSONObject(1);
			JSONObject thirdObj = foodArray.getJSONObject(2);
			
			SimpleDateFormat format = new SimpleDateFormat("dd.MM.yyyy");
			SimpleDateFormat dateFormat = new SimpleDateFormat("d MMM EEEE");
			Date date = format.parse(obj.getString("Tarih"));
			Date currentDate = format.parse(format.format(new Date()));
			
			if (date.compareTo(currentDate) < 0)
				currentIndex++;

			tvDate.setText(dateFormat.format(date));
			tvFirstFood.setText(firstObj.getString("Adi"));
			tvSecondFood.setText(secondObj.getString("Adi"));
			tvThirdFood.setText(thirdObj.getString("Adi"));
			tvFirstCal.setText(firstObj.getString("Kalori") + " kcal");
			tvSecondCal.setText(secondObj.getString("Kalori") + " kcal");
			tvThirdCal.setText(thirdObj.getString("Kalori") + " kcal");
		} catch (Exception e) {
			e.printStackTrace();
		}
		return view;
	}
	@Override
	public void destroyItem(View arg0, int arg1, Object arg2) {
		((ViewPager) arg0).removeView((View) arg2);
	}
	 
	@Override
	public void finishUpdate(View arg0) {
	// TODO Auto-generated method stub
	 
	}
	@Override
	public int getCount() {
		return views.size();
	}
	 
	@Override
	public Object instantiateItem(View arg0, int arg1) {
		View v = views.get(arg1);
		((ViewPager)arg0).addView(v);
		return v;
	}
	 
	@Override
	public boolean isViewFromObject(View arg0, Object arg1) {
		return arg0 == (View)arg1;
	}
	 
	@Override
	public void restoreState(Parcelable arg0, ClassLoader arg1) {
	// TODO Auto-generated method stub
	 
	}
	 
	@Override
	public Parcelable saveState() {
	// TODO Auto-generated method stub
	return null;
	}
	 
	@Override
	public void startUpdate(View arg0) {
	// TODO Auto-generated method stub
	 
	}

}
