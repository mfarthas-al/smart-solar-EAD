package com.ead.smartsolarmicrogrid.data;

// Matches WebService's LoginRequest DTO. Gson fills these fields straight
// from the JSON, no getters/setters needed.
public class LoginRequest {
    public String email;
    public String password;

    public LoginRequest(String email, String password) {
        this.email = email;
        this.password = password;
    }
}
