package com.ead.smartsolarmicrogrid.prosumer;

import android.content.Intent;
import android.os.Bundle;
import android.widget.Button;
import android.widget.TextView;
import androidx.appcompat.app.AppCompatActivity;
import com.ead.smartsolarmicrogrid.R;
import com.ead.smartsolarmicrogrid.common.LoginActivity;
import com.ead.smartsolarmicrogrid.data.SessionDbHelper;

public class ProsumerDashboardActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_prosumer_dashboard);

        SessionDbHelper dbHelper = new SessionDbHelper(this);
        SessionDbHelper.Session session = dbHelper.getSession();

        TextView textWelcome = findViewById(R.id.textWelcome);
        if (session != null) {
            textWelcome.setText("Welcome, " + session.fullName + " (" + session.nic + ")");
        }

        Button buttonLogout = findViewById(R.id.buttonLogout);
        buttonLogout.setOnClickListener(v -> {
            dbHelper.clearSession();
            startActivity(new Intent(this, LoginActivity.class));
            finish();
        });
    }
}
