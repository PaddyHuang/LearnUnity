package com.polin.simplecalc;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

import com.unity3d.player.UnityPlayerActivity;

public class DevisionActivity extends UnityPlayerActivity {

//    @Override
//    protected void onCreate(Bundle savedInstanceState) {
//        super.onCreate(savedInstanceState);
//        setContentView(R.layout.activity_devision);
//    }

    public static double Devide(double a, double b){
        if (b != 0){
            return a / b;
        }
        else {
            return 999;
        }
    }
}
