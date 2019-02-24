package com.ogzkyr.mobisis.adapters;

import java.util.List;

import com.ogzkyr.mobisis.R;
import com.ogzkyr.mobisis.models.Period;

import android.app.Activity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

public class PeriodAdapter extends BaseAdapter {

	private List<Period> periods;
	private static LayoutInflater inflater = null;
	
	public PeriodAdapter(Activity act, List<Period> list){
		this.periods = list;
		inflater = (LayoutInflater)act.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
	}
	@Override
	public int getCount() {
		return periods.size();
	}

	@Override
	public Period getItem(int arg0) {
		return periods.get(arg0);
	}

	@Override
	public long getItemId(int arg0) {
		return arg0;
	}

	@Override
	public View getView(int position, View view, ViewGroup viewGroup) {
		if (view == null)
			view = inflater.inflate(R.layout.period_list_row, null);
		TextView periodName = (TextView)view.findViewById(R.id.periodName);
		TextView periodInfo = (TextView)view.findViewById(R.id.periodInfo);

		Period period = (Period)getItem(position);
        
		periodName.setText(period.OgretimYili + " " + period.Adi);
		periodInfo.setText("" + period.GANO);
		
		return view;
	}

}
