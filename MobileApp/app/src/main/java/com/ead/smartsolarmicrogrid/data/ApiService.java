package com.ead.smartsolarmicrogrid.data;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.POST;

// Retrofit endpoint definitions. One interface method per WebService
// endpoint the app calls - add more here as new screens need them.
public interface ApiService {

    @POST("api/auth/login")
    Call<LoginResponse> login(@Body LoginRequest request);

    @POST("api/prosumers/register")
    Call<ProsumerResponse> registerProsumer(@Body RegisterProsumerRequest request);
}
