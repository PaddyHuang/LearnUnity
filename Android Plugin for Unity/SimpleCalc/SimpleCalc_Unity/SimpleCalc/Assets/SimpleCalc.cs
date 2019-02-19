using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleCalc : MonoBehaviour {

    private Button add;
    private Button subtract;
    private Button multiply;
    private Button devide;
    private Text text;
    private InputField inputField1;
    private InputField inputField2;
    private Button done;

    private int index = -1;

    // Use this for initialization
    void Start () {
        add = transform.Find("Add").GetComponent<Button>();
        subtract = transform.Find("Subtract").GetComponent<Button>();
        multiply = transform.Find("Multiply").GetComponent<Button>();
        devide = transform.Find("Devide").GetComponent<Button>();
        text = transform.Find("Text").GetComponent<Text>();
        inputField1 = transform.Find("InputField1").GetComponent<InputField>();
        inputField2 = transform.Find("InputField2").GetComponent<InputField>();
        done = transform.Find("Done").GetComponent<Button>();

        add.onClick.AddListener(Add);
        subtract.onClick.AddListener(Subtract);
        multiply.onClick.AddListener(Multiply);
        devide.onClick.AddListener(Devide);
        done.onClick.AddListener(Done);
    }
	
    public void Add(){ index = 0; }
    public void Subtract() { index = 1; }
    public void Multiply() { index = 2; }
    public void Devide() { index = 3; }
    
    public void Done()
    {
        //Debug.Log(string.Format("a:{0}, b: {1}, operator:{2}", Convert.ToDouble(inputField1.text), Convert.ToDouble(inputField2.text), index));
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("Calc", index, Convert.ToDouble(inputField1.text), Convert.ToDouble(inputField2.text));
    }

    public void FromAndroid(string content)
    {
        text.text = string.Format("Res: {0}", content);
    }
}
