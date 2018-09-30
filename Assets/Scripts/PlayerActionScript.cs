﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionScript : MonoBehaviour
{

    public string player;
    public float stunCooldown;
    public float stunDuration;

    private float stunTimer;
    private bool stunActivated = false;

    private AllyMovementScript AllyMovementScript;
    private bool player2Active;

    private GameStatusScript GameStatusScript;


    private void Start()
    {
        stunTimer = -1 * stunCooldown;
        player2Active = false;
        AllyMovementScript = GameObject.Find("Ally1").GetComponentInChildren<AllyMovementScript>();
        GameStatusScript = GameObject.Find("GameStatus").GetComponent<GameStatusScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stunActivated)
        {
            if (Input.GetButtonDown(player + "Button2") && (Time.time > stunTimer + stunCooldown))
            {
                stunActivated = true;
                stunTimer = Time.time;
                EventManager.TriggerEvent("STUN_ACTIVATED");
                StunTheCat();
            }
        }
        else if (Time.time > stunTimer + stunDuration)
        {
            stunActivated = false;
            EventManager.TriggerEvent("STUN_DEACTIVATED");
        }

        if (Input.GetButtonDown(player + "Button1"))
        {
            //do thing
            GameStatusScript.KeepThemSecrets();
        }


        //Check if player two is trying to join
        if (!player2Active)
        {
            if (Input.GetButtonDown("Player2Button1") || Input.GetButton("Player2Button2"))
            {
                AllyMovementScript.Player2Joined();
                player2Active = true;
            }
        }
    }

    public void StunTheCat()
    {
        PlaySound("StunningSound");
    }

public virtual void PlaySound(string soundName)
    {
        AudioSource[] sounds = GetComponents<AudioSource>();

        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].clip.name.Contains(soundName)) 
            {
                sounds[i].Play();
            }
        }
    }
}
