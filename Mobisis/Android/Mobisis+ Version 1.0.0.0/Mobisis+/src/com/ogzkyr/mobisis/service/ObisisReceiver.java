package com.ogzkyr.mobisis.service;


import com.ogzkyr.mobisis.extras.Function;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

public class ObisisReceiver extends BroadcastReceiver {
    @Override
    public void onReceive(Context context, Intent intent) {
        try{
        	wait(20000);
        }catch(Exception ex){
        	ex.printStackTrace();
        }finally {
    		Function.startObisisService(context);
        }
    }
}
