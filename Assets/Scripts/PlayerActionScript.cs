using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionScript : MonoBehaviour
{

    public string player;

    public float stunCooldown;
    public float stunDuration;
    private float stunTimer;
    private bool stunActivated = false;
    private bool enemyStunned = false;
    private bool stunAvailable = false;

    public float silenceCooldown = 10f;
    public float silenceDuration;
    private float silenceTimer;
    private bool silenceActivated = false;
    private bool silenceAvailable = false;

    private AllyMovementScript AllyMovementScript;
    private bool player2Active;

    private GameStatusScript GameStatusScript;

    private Animator animator;


    private void Start()
    {
        player2Active = false;
        AllyMovementScript = GameObject.Find("Ally1").GetComponentInChildren<AllyMovementScript>();
        GameStatusScript = GameObject.Find("GameStatus").GetComponent<GameStatusScript>();
        this.animator = this.gameObject.GetComponent<Animator>();

        silenceAvailable = true;
        silenceTimer = 0f;
        stunAvailable = true;
        stunTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        silenceTimer += Time.deltaTime;
        stunTimer += Time.deltaTime;
        if (!stunActivated)
        {
            if (Input.GetButtonDown(player + "Button2") && (stunTimer >= stunCooldown))
            {
                PressStunButton();
            }
        }
        else if (stunTimer > stunDuration)
        {
           Unstun();
        }

        if (!stunAvailable && stunTimer >= stunCooldown)
        {
            StunRefresh();
        }

        if (enemyStunned && stunTimer > stunDuration)
        {
            EventManager.TriggerEvent("ENEMY_UNSTUNNED");
        }

        if (!silenceActivated)
        {
            if (Input.GetButtonDown(player + "Button1") && (silenceTimer >= silenceCooldown))
            {
                if (player == "Player1")
                {
                    PressQuietButton();
                }
                else if (player == "Player2" && player2Active)
                {
                    PressQuietButton();
                }
            }

        }else if (silenceTimer > silenceDuration)
        {
            Unsilence();
        }

        if (!silenceAvailable && silenceTimer >= silenceCooldown)
        {
            SilenceRefresh();
        }


        //Check if player two is trying to join
        if (!player2Active)
        {
            if (Input.GetButtonDown("Player2Button2"))
            {
                AllyMovementScript.Player2Joined();
                player2Active = true;
                EventManager.TriggerEvent("PLAYER2_JOIN");
            }
        }
    }
    
    private void PressStunButton()
    {
        stunActivated = true;
        stunAvailable = false;
        this.animator.SetTrigger("pushedStunButton");
        if (GameStatusScript.enemyInDangerZone)
        {
            EventManager.TriggerEvent("ENEMY_STUNNED");
            this.enemyStunned = true;
        }

        EventManager.TriggerEvent("STUN_ACTIVATED");
        PlaySound("StunningSound");

        stunTimer = 0f;
    }

    private void Unstun()
    {
        stunActivated = false;
        EventManager.TriggerEvent("STUN_DEACTIVATED");
    }

    private void StunRefresh()
    {
        EventManager.TriggerEvent("STUN_REFRESH");
        stunAvailable = true;
    }

    private void PressQuietButton()
    {
        //do thing
        this.animator.SetTrigger("pushedQuietButton");

        EventManager.TriggerEvent("SILENT_ACTIVATED");

        silenceActivated = true;
        silenceAvailable = false;
        silenceTimer = 0f;
    }

    private void Unsilence()
    {
        EventManager.TriggerEvent("SILENT_DEACTIVATED");
        silenceActivated = false;
    }

    private void SilenceRefresh()
    {
        EventManager.TriggerEvent("SILENT_REFRESH");
        silenceAvailable = true;
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
