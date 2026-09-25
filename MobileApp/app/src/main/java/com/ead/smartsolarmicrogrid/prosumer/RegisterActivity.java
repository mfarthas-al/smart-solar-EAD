package com.ead.smartsolarmicrogrid.prosumer;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;
import androidx.appcompat.app.AppCompatActivity;
import com.ead.smartsolarmicrogrid.R;
import com.ead.smartsolarmicrogrid.data.ApiClient;
import com.ead.smartsolarmicrogrid.data.ErrorResponse;
import com.ead.smartsolarmicrogrid.data.ProsumerResponse;
import com.ead.smartsolarmicrogrid.data.RegisterProsumerRequest;
import com.google.gson.Gson;
import java.io.IOException;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class RegisterActivity extends AppCompatActivity {

    private EditText editNic;
    private EditText editFullName;
    private EditText editEmail;
    private EditText editPassword;
    private EditText editContactNumber;
    private EditText editPropertyAddress;
    private EditText editSolarCapacity;
    private TextView textRegisterError;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);

        editNic = findViewById(R.id.editNic);
        editFullName = findViewById(R.id.editFullName);
        editEmail = findViewById(R.id.editEmail);
        editPassword = findViewById(R.id.editPassword);
        editContactNumber = findViewById(R.id.editContactNumber);
        editPropertyAddress = findViewById(R.id.editPropertyAddress);
        editSolarCapacity = findViewById(R.id.editSolarCapacity);
        textRegisterError = findViewById(R.id.textRegisterError);

        Button buttonRegister = findViewById(R.id.buttonRegister);
        buttonRegister.setOnClickListener(v -> attemptRegister());
    }

    // Submits the registration form. On success the account is created as
    // Pending - it can't log in until a Backoffice user activates it.
    private void attemptRegister() {
        textRegisterError.setVisibility(View.GONE);

        String nic = editNic.getText().toString().trim();
        String fullName = editFullName.getText().toString().trim();
        String email = editEmail.getText().toString().trim();
        String password = editPassword.getText().toString();
        String contactNumber = editContactNumber.getText().toString().trim();
        String propertyAddress = editPropertyAddress.getText().toString().trim();
        String capacityText = editSolarCapacity.getText().toString().trim();
        Double solarCapacity = capacityText.isEmpty() ? null : Double.parseDouble(capacityText);

        RegisterProsumerRequest request = new RegisterProsumerRequest(
                nic, fullName, email, password, contactNumber, propertyAddress, solarCapacity);

        ApiClient.getService(this).registerProsumer(request).enqueue(new Callback<ProsumerResponse>() {
            @Override
            public void onResponse(Call<ProsumerResponse> call, Response<ProsumerResponse> response) {
                if (response.isSuccessful()) {
                    Toast.makeText(RegisterActivity.this,
                            "Registered! Wait for a Backoffice officer to activate your account.",
                            Toast.LENGTH_LONG).show();
                    finish();
                } else {
                    showError(parseErrorMessage(response));
                }
            }

            @Override
            public void onFailure(Call<ProsumerResponse> call, Throwable t) {
                showError("Could not reach the server. Is the WebService running?");
            }
        });
    }

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
        return "Registration failed. Please check your details and try again.";
    }

    private void showError(String message) {
        textRegisterError.setText(message);
        textRegisterError.setVisibility(View.VISIBLE);
    }
}
