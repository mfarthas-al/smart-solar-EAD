package com.ead.smartsolarmicrogrid.common;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import androidx.appcompat.app.AppCompatActivity;
import com.ead.smartsolarmicrogrid.R;
import com.ead.smartsolarmicrogrid.data.ApiClient;
import com.ead.smartsolarmicrogrid.data.ErrorResponse;
import com.ead.smartsolarmicrogrid.data.LoginRequest;
import com.ead.smartsolarmicrogrid.data.LoginResponse;
import com.ead.smartsolarmicrogrid.data.SessionDbHelper;
import com.ead.smartsolarmicrogrid.operator.OperatorDashboardActivity;
import com.ead.smartsolarmicrogrid.prosumer.ProsumerDashboardActivity;
import com.google.gson.Gson;
import java.io.IOException;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class LoginActivity extends AppCompatActivity {

    private EditText editEmail;
    private EditText editPassword;
    private TextView textError;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        editEmail = findViewById(R.id.editEmail);
        editPassword = findViewById(R.id.editPassword);
        textError = findViewById(R.id.textError);

        Button buttonLogin = findViewById(R.id.buttonLogin);
        buttonLogin.setOnClickListener(v -> attemptLogin());

        Button buttonGoToRegister = findViewById(R.id.buttonGoToRegister);
        buttonGoToRegister.setOnClickListener(v ->
                startActivity(new Intent(this, com.ead.smartsolarmicrogrid.prosumer.RegisterActivity.class)));
    }

    // Calls the login endpoint and, on success, saves the session to SQLite
    // and moves to the right dashboard for the returned role.
    private void attemptLogin() {
        String email = editEmail.getText().toString().trim();
        String password = editPassword.getText().toString();
        textError.setVisibility(View.GONE);

        ApiClient.getService(this).login(new LoginRequest(email, password)).enqueue(new Callback<LoginResponse>() {
            @Override
            public void onResponse(Call<LoginResponse> call, Response<LoginResponse> response) {
                if (response.isSuccessful() && response.body() != null) {
                    LoginResponse body = response.body();

                    SessionDbHelper dbHelper = new SessionDbHelper(LoginActivity.this);
                    dbHelper.saveSession(body.token, body.role, body.nic, body.fullName, body.email);

                    Intent intent = "GridOperator".equals(body.role)
                            ? new Intent(LoginActivity.this, OperatorDashboardActivity.class)
                            : new Intent(LoginActivity.this, ProsumerDashboardActivity.class);
                    startActivity(intent);
                    finish();
                } else {
                    showError(parseErrorMessage(response));
                }
            }

            @Override
            public void onFailure(Call<LoginResponse> call, Throwable t) {
                showError("Could not reach the server. Is the WebService running?");
            }
        });
    }

    // The server's error body is {"message": "..."} - same shape the API
    // returns for both wrong credentials and inactive accounts.
    private String parseErrorMessage(Response<?> response) {
        try {
            if (response.errorBody() != null) {
                ErrorResponse error = new Gson().fromJson(response.errorBody().string(), ErrorResponse.class);
                if (error != null && error.message != null) {
                    return error.message;
                }
            }
        } catch (IOException e) {
            // fall through to the generic message below
        }
        return "Invalid email or password";
    }

    private void showError(String message) {
        textError.setText(message);
        textError.setVisibility(View.VISIBLE);
    }
}
