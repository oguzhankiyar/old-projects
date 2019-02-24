package com.ogzkyr.mobisis.tasks;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;

import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;

import android.os.AsyncTask;

public class ConnectAsync extends AsyncTask<String, Void, String> {

	protected String doInBackground(String... urls) {
		String URL = urls[0];
        HttpClient httpclient = new DefaultHttpClient();
        HttpResponse response;
        try {
            HttpPost httpPost = new HttpPost(URL);
            response = httpclient.execute(httpPost);
            InputStream is = response.getEntity().getContent();
            StringBuilder sb = new StringBuilder();
            BufferedReader reader = new BufferedReader(new InputStreamReader(is, "utf-8"), 8);
            sb = new StringBuilder();
            sb.append(reader.readLine() + "\n");
            String line = "0";

            while ((line = reader.readLine()) != null) {
                sb.append(line + "\n");
            }

            is.close();
            return sb.toString();
        }catch (Exception e) {
    		e.printStackTrace();
    		return null;
        }
	}
}