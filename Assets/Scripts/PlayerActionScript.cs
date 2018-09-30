using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionScript : MonoBehaviour {

    public string player;
    public float stunCooldown;
    public float quietCooldown;

    private float stunTimer;
    private float quietTimer;

    private void Start()
    {
        stunTimer = -1 * stunCooldown;
        quietTimer = -1 * quietCooldown;
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetButtonDown(player + "Button1") && (Time.time > stunTimer + stunCooldown))
        {
            Debug.Log("stun!");
            stunTimer = Time.time;
        }

        if(Input.GetButtonDown(player + "Button2") && (Time.time > quietTimer + quietCooldown)) 
        {
            Debug.Log("quiet!");
            quietTimer = Time.time;
        }
	}

    void Stun()
    {

    }

    void Quiet ()
    {

    }
}
