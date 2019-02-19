using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherBackgroundColor : MonoBehaviour {

    private Image image;

    private void Awake()
    {
        image = this.GetComponent<Image>();
    }

    // Use this for initialization
    void Start () {
        image.color = new Color(0, 0, 0, 0.3f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}


}
