package com.ead.smartsolarmicrogrid.common;

import android.os.Bundle;
import androidx.appcompat.app.AppCompatActivity;

/**
 * Launcher entry point. Will route to a real login screen once auth is implemented;
 * for now it just confirms the app scaffold runs.
 */
public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(com.ead.smartsolarmicrogrid.R.layout.activity_main);
    }
}
