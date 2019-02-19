using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");
        //Debug.Log(h);
        float v = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(h, v, 0) * Time.deltaTime);
	}
}
