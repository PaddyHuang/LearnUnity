using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

    public GameObject bullet;
    private float speed = 20;

	// Use this for initialization
	void Start () {
        //GameObject.Instantiate(bullet, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject gd =  GameObject.Instantiate(bullet, transform.position, transform.rotation);
            Rigidbody rigidbody = gd.GetComponent<Rigidbody>();
            rigidbody.velocity = transform.forward * speed;
        }
	}
}
