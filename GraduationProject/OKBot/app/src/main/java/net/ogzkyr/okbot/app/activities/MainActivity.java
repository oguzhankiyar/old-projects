package net.ogzkyr.okbot.app.activities;

import android.app.Activity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;
import net.ogzkyr.okbot.app.R;
import net.ogzkyr.okbot.app.Settings;
import net.ogzkyr.okbot.app.tasks.ConnectDeviceAsync;

import java.io.IOException;

public class MainActivity extends Activity {

    ConnectDeviceAsync task;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Button btnConnect = (Button) findViewById(R.id.btnConnect);
        Button btnDisconnect = (Button) findViewById(R.id.btnDisconnect);
        Button btnTurnOn = (Button) findViewById(R.id.btnTurnOn);
        Button btnTurnOff = (Button) findViewById(R.id.btnTurnOff);

        btnConnect.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                connect();
            }
        });

        btnDisconnect.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                disconnect();
            }
        });

        btnTurnOn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                write("TO");
            }
        });

        btnTurnOff.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                write("TF");
            }
        });

        getWindow().addFlags(128);
    }

    private void connect() {
        task = new ConnectDeviceAsync(MainActivity.this);
        task.execute(Settings.DEVICE_MAC_ADDRESS);
    }

    private void disconnect() {
        try {
            task.socket.close();
            Toast.makeText(MainActivity.this, "Disconnected.", Toast.LENGTH_SHORT).show();
        } catch (IOException e) {
            Toast.makeText(MainActivity.this, "Disconnection Failed.", Toast.LENGTH_SHORT).show();
        }
    }

    private void write(String data) {
        try {
            task.socket.getOutputStream().write(data.getBytes());
            Toast.makeText(MainActivity.this, "Sended.", Toast.LENGTH_SHORT).show();
        } catch (IOException e) {
            Toast.makeText(MainActivity.this, "Data Sending Failed.", Toast.LENGTH_SHORT).show();
        }
    }
}
