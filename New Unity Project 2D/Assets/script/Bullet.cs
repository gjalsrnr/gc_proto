using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 1.0f;
    public float damage;
    private bool fire;
    //public float lifeTime? distance?

    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }

	// Use this for initialization
	void Start () {
        speed = 3.0f;
        fire = false;
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetButtonDown("Jump"))
        //   fire = true;
        //if(fire)
        this.transform.Translate(speed * Time.deltaTime, 0, 0, Space.Self);
	}
}
