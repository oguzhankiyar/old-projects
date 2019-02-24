package net.ogzkyr.okbot.app.tasks;

import android.app.ProgressDialog;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothClass;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothSocket;
import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;
import android.widget.Toast;
import net.ogzkyr.okbot.app.Settings;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;

public class ConnectDeviceAsync extends AsyncTask<String, String, Boolean> {

    public static BluetoothSocket socket = null;
    private Context context;
    private ProgressDialog progressDialog;

    public ConnectDeviceAsync(Context ctx) {
        this.context = ctx;
    }

    @Override
    protected void onPreExecute() {
        progressDialog = ProgressDialog.show(context, "Connecting...", "Please Wait");
    }

    @Override
    protected Boolean doInBackground(String... params) {
        String address = params[0];

        try {
            BluetoothAdapter adapter = BluetoothAdapter.getDefaultAdapter();
            BluetoothDevice device = adapter.getRemoteDevice(address);
            socket = device.createInsecureRfcommSocketToServiceRecord(Settings.UUID);
            Method m = device.getClass().getMethod("createInsecureRfcommSocket", new Class[]{int.class});
            socket = (BluetoothSocket) m.invoke(device, 1);
            adapter.cancelDiscovery();
            if (device.getBondState() != BluetoothDevice.BOND_BONDED)
            socket.connect();
            socket.getOutputStream().write("TF".getBytes());
            socket.getOutputStream().write("TO".getBytes());
        } catch (Exception e) {
            Log.d("OKBotLog", e.toString());
            return false;
        }
        return true;
    }

    @Override
    protected void onPostExecute(Boolean result) {
        progressDialog.dismiss();
        if (result) {
            Toast.makeText(context, "Connected.", Toast.LENGTH_SHORT).show();
        } else {
            Toast.makeText(context, "Connection Failed.", Toast.LENGTH_SHORT).show();
        }
    }
}
