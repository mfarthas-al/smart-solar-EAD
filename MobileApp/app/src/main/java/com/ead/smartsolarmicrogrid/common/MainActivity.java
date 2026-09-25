package com.ead.smartsolarmicrogrid.common;

import android.content.Intent;
import android.os.Bundle;
import androidx.appcompat.app.AppCompatActivity;
import com.ead.smartsolarmicrogrid.data.SessionDbHelper;
import com.ead.smartsolarmicrogrid.operator.OperatorDashboardActivity;
import com.ead.smartsolarmicrogrid.prosumer.ProsumerDashboardActivity;

/**
 * Launcher entry point. Checks the local SQLite session on startup and
 * routes straight to the right dashboard if already logged in, otherwise
 * to Login - mirrors how the Web App checks localStorage on load.
 */
public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        SessionDbHelper dbHelper = new SessionDbHelper(this);
        SessionDbHelper.Session session = dbHelper.getSession();

        Intent intent;
        if (session == null) {
            intent = new Intent(this, LoginActivity.class);
        } else if ("GridOperator".equals(session.role)) {
            intent = new Intent(this, OperatorDashboardActivity.class);
        } else {
            intent = new Intent(this, ProsumerDashboardActivity.class);
        }

        startActivity(intent);
        finish();
    }
}
