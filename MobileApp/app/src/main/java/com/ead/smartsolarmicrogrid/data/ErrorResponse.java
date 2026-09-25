package com.ead.smartsolarmicrogrid.data;

// Matches the {"message": "..."} shape the WebService returns on errors
// (e.g. 401 invalid credentials, 409 duplicate NIC).
public class ErrorResponse {
    public String message;
}
