package com.ogzkyr.mobisis.models;

import android.content.Intent;

public class PageStackItem {
	Intent intent;
	PageStackItem next;
	
	public PageStackItem(Intent intent) {
		this.intent = intent;
		this.next = null;
	}
}
