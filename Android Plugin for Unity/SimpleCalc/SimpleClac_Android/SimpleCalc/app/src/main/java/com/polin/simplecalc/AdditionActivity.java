package com.polin.simplecalc;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

import com.unity3d.player.UnityPlayerActivity;

public class AdditionActivity extends UnityPlayerActivity {

//    @Override
//    protected void onCreate(Bundle savedInstanceState) {
//        super.onCreate(savedInstanceState);
//        setContentView(R.layout.activity_add);
//    }

    public static double Add(double a, double b){
        return a + b;
    }
}
