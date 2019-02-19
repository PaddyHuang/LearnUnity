package com.polin.simplecalc;

import android.app.Activity;
import android.widget.Toast;

import com.unity3d.player.UnityPlayerActivity;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;

public class MainActivity extends UnityPlayerActivity {

    /**
     * unity项目启动时的的上下文
     */
    private Activity _unityActivity;
    /**
     * 获取unity项目的上下文
     * @return
     */
    Activity GetActivity(){
        if(null == _unityActivity) {
            try {
                Class<?> classtype = Class.forName("com.unity3d.player.UnityPlayer");
                Activity activity = (Activity) classtype.getDeclaredField("currentActivity").get(classtype);
                _unityActivity = activity;
            } catch (ClassNotFoundException e) {

            } catch (IllegalAccessException e) {

            } catch (NoSuchFieldException e) {

            }
        }
        return _unityActivity;
    }

    /**
     * 调用Unity的方法
     * @param gameObjectName    调用的GameObject的名称
     * @param functionName      方法名
     * @param args              参数
     * @return                  调用是否成功
     */
    boolean CallUnity(String gameObjectName, String functionName, String args){
        try {
            Class<?> classtype = Class.forName("com.unity3d.player.UnityPlayer");
            Method method =classtype.getMethod("UnitySendMessage", String.class,String.class,String.class);
            method.invoke(classtype,gameObjectName,functionName,args);
            return true;
        } catch (ClassNotFoundException e) {

        } catch (NoSuchMethodException e) {

        } catch (IllegalAccessException e) {

        } catch (InvocationTargetException e) {

        }
        return false;
    }

    public void Calc(int operator, double a, double b){
        if (operator >=0 && operator < 4){
            switch (operator){
                case 0:
//                    Toast.makeText(GetActivity(), String.valueOf(a), Toast.LENGTH_SHORT).show();
                    CallUnity("Canvas","FromAndroid",String.valueOf(AdditionActivity.Add(a,b)));
                    break;
                case 1:
                    CallUnity("Canvas","FromAndroid",String.valueOf(SubtractionActivity.Subtract(a,b)));
                    break;
                case 2:
                    CallUnity("Canvas","FromAndroid",String.valueOf(MultiplicationActivity.Multiply(a,b)));
                    break;
                case  3:
                    CallUnity("Canvas","FromAndroid",String.valueOf(DevisionActivity.Devide(a,b)));
                    break;
                default:
                    break;
            }
        }
//        else{
//            return false;
//        }
//        return true;
    }
}
