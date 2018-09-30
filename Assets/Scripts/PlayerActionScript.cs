using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionScript : MonoBehaviour {

    public string player;
    public float stunCooldown;
    public float stunDuration;

    public float quietCooldown;
    public float quietDuration;

    private float stunTimer;
    private float quietTimer;
    private bool stunActivated = false;
    private bool quietActivated = false;

    private void Start()
    {
        stunTimer = -1 * stunCooldown;
        quietTimer = -1 * quietCooldown;
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

        if (!quietActivated)
        {
            if (Input.GetButtonDown(player + "Button2") && (Time.time > quietTimer + quietCooldown))
            {
                Debug.Log("quiet!");
                quietActivated = true;
                quietTimer = Time.time;
                EventManager.TriggerEvent("QUIET_ACTIVATED");
            }
        }
        else if (Time.time > quietTimer + quietDuration)
        {
            quietActivated = false;
            Debug.Log("End quiet");
            EventManager.TriggerEvent("QUIET_DEACTIVATED");
        }
	}

    void Stun()
    {
        // Trigger cat listener to:
        //      activate stunned animation
        //      stop moving
        //      stop counting points
        // Trigger players to activate stunning animation
    }

    void Quiet ()
    {
        // Trigger both players to activate quiet animations
        // stop gaining points
    }
}
