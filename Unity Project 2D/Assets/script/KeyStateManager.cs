using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyStateManager : MonoBehaviour {

    bool isPress;
    bool isUp;

	// Use this for initialization
	void Start () {
        isPress = false;
        isUp = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void KeyPress(KeyCode key)
    {
        if (Input.GetKey(key))
        {

        }
    }

    void KeyUp(KeyCode key)
    {
        if (Input.GetKey(key))
        {

        }
    }
}
