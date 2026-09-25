package com.ead.smartsolarmicrogrid.data;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;

// Local SQLite database that stores the logged-in user's session, so the
// app can stay logged in across restarts without asking for the password
// again. Only ever holds one row - a fresh login replaces it.
public class SessionDbHelper extends SQLiteOpenHelper {
    private static final String DATABASE_NAME = "smartsolar_local.db";
    private static final int DATABASE_VERSION = 1;
    private static final String TABLE_SESSION = "session";

    public SessionDbHelper(Context context) {
        super(context, DATABASE_NAME, null, DATABASE_VERSION);
    }

    @Override
    public void onCreate(SQLiteDatabase db) {
        db.execSQL("CREATE TABLE " + TABLE_SESSION + " (" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "token TEXT, " +
                "role TEXT, " +
                "nic TEXT, " +
                "full_name TEXT, " +
                "email TEXT)");
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        db.execSQL("DROP TABLE IF EXISTS " + TABLE_SESSION);
        onCreate(db);
    }

    // Replaces any existing session with this one (only one user is ever
    // logged in on a given device at a time).
    public void saveSession(String token, String role, String nic, String fullName, String email) {
        SQLiteDatabase db = getWritableDatabase();
        db.delete(TABLE_SESSION, null, null);

        ContentValues values = new ContentValues();
        values.put("token", token);
        values.put("role", role);
        values.put("nic", nic);
        values.put("full_name", fullName);
        values.put("email", email);
        db.insert(TABLE_SESSION, null, values);
        db.close();
    }

    // Returns the saved session, or null if nobody is logged in.
    public Session getSession() {
        SQLiteDatabase db = getReadableDatabase();
        Cursor cursor = db.query(TABLE_SESSION, null, null, null, null, null, null);

        Session session = null;
        if (cursor.moveToFirst()) {
            session = new Session();
            session.token = cursor.getString(cursor.getColumnIndexOrThrow("token"));
            session.role = cursor.getString(cursor.getColumnIndexOrThrow("role"));
            session.nic = cursor.getString(cursor.getColumnIndexOrThrow("nic"));
            session.fullName = cursor.getString(cursor.getColumnIndexOrThrow("full_name"));
            session.email = cursor.getString(cursor.getColumnIndexOrThrow("email"));
        }
        cursor.close();
        db.close();
        return session;
    }

    public void clearSession() {
        SQLiteDatabase db = getWritableDatabase();
        db.delete(TABLE_SESSION, null, null);
        db.close();
    }

    // Simple holder for a saved session's fields.
    public static class Session {
        public String token;
        public String role;
        public String nic;
        public String fullName;
        public String email;
    }
}
