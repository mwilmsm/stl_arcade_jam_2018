using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionScript : MonoBehaviour {

    public string player;
    public float stunCooldown;
    public float stunDuration;

    private float stunTimer;
    private bool stunActivated = false;

    private void Start()
    {
        stunTimer = -1 * stunCooldown;
    }

    // Update is called once per frame
    void Update () {
        if (!stunActivated)
        {
            if (Input.GetButtonDown(player + "Button1") && (Time.time > stunTimer + stunCooldown))
            {
                Debug.Log("stun!");
                stunActivated = true;
                stunTimer = Time.time;
                EventManager.TriggerEvent("STUN_ACTIVATED");
            }
        }
        else if(Time.time > stunTimer + stunDuration)
        {
            stunActivated = false;
            Debug.Log("End stun");
            EventManager.TriggerEvent("STUN_DEACTIVATED");
        }
	}
}
