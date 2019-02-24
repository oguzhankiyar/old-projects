package com.ogzkyr.mobisis.models;

import android.content.Intent;

public class PageStack {
	PageStackItem head;
	
	public PageStack() {
		this.head = null;
	}
	public void addPage(Intent intent) {
		PageStackItem newItem = new PageStackItem(intent);
		if (head == null)
			head = newItem;
		else {
			newItem.next = head;
			head = newItem;
		}
	}
	public void deletePage() {
		head = head.next;
	}
	public Intent getPageIntent() {
		return head.intent;
	}
}