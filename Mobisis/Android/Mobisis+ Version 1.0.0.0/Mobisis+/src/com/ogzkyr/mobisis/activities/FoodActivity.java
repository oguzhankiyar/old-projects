package com.ogzkyr.mobisis.activities;

import com.ogzkyr.mobisis.R;
import com.ogzkyr.mobisis.adapters.FoodAdapter;
import com.ogzkyr.mobisis.extras.Function;

import android.os.Bundle;
import android.support.v4.view.PagerAdapter;
import android.support.v4.view.ViewPager;


public class FoodActivity extends BaseActivity {
	
	public static PagerAdapter adapter;
	ViewPager viewPager;
	
	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_food);
		ActivitySettings();
		
		try {
			viewPager = (ViewPager)findViewById(R.id.viewPager);
			viewPager.setAdapter(adapter);
			viewPager.setCurrentItem(FoodAdapter.currentIndex);
		} catch (Exception e) {
			Function.ShowError(FoodActivity.this);
		}
	}
	
}