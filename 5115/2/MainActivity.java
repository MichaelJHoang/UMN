package com.example.myfirstapp;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

public class MainActivity extends AppCompatActivity {

    public static final String EXTRA_MESSAGE = "com.example.myfirstapp.MESSAGE";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Log.d("MainActivity", "Hello Android World");
    }

    public void sendMessage (View view)
    {
        Intent intent = new Intent(this, DisplayMessageActivity.class);
        EditText editText = (EditText) findViewById(R.id.editText);
        EditText pwInput = (EditText) findViewById(R.id.passwordInput);
        String message = editText.getText().toString();
        String pw = pwInput.getText().toString();

        if (pw.isEmpty() == false)
        {
            if (message == null)
                message = "Random User";

            intent.putExtra(EXTRA_MESSAGE, message);
            startActivity(intent);
        }
        else
        {
            Toast.makeText(this, "Please enter in your password.", Toast.LENGTH_SHORT).show();
        }

    }
}