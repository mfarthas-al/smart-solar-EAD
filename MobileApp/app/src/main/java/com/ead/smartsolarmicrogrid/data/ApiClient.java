package com.ead.smartsolarmicrogrid.data;

import android.content.Context;
import com.ead.smartsolarmicrogrid.R;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

// Builds one Retrofit instance pointed at the Web Service base URL from
// strings.xml. Call ApiClient.getService(context) wherever an API call
// is needed.
public class ApiClient {
    private static ApiService apiService;

    public static ApiService getService(Context context) {
        if (apiService == null) {
            String baseUrl = context.getString(R.string.api_base_url);
            Retrofit retrofit = new Retrofit.Builder()
                    .baseUrl(baseUrl)
                    .addConverterFactory(GsonConverterFactory.create())
                    .build();
            apiService = retrofit.create(ApiService.class);
        }
        return apiService;
    }
}
