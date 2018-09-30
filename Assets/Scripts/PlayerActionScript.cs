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

    public float silenceCooldown = 10f;
    private float silenceTimer;
    private bool silenceActivated = false;

    private AllyMovementScript AllyMovementScript;
    private bool player2Active;

    private GameStatusScript GameStatusScript;

    private Animator animator;


    private void Start()
    {
        stunTimer = -1 * stunCooldown;
        player2Active = false;
        AllyMovementScript = GameObject.Find("Ally1").GetComponentInChildren<AllyMovementScript>();
        GameStatusScript = GameObject.Find("GameStatus").GetComponent<GameStatusScript>();
        this.animator = this.gameObject.GetComponent<Animator>();

        silenceTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        silenceTimer += Time.deltaTime;
        if (!stunActivated)
        {
            if (Input.GetButtonDown(player + "Button2") && (Time.time > stunTimer + stunCooldown))
            {
                stunActivated = true;
                stunTimer = Time.time;
                this.animator.SetTrigger("pushedStunButton");
                EventManager.TriggerEvent("STUN_ACTIVATED");
                StunTheCat();
            }
        }
        else if (Time.time > stunTimer + stunDuration)
        {
            stunActivated = false;
            EventManager.TriggerEvent("STUN_DEACTIVATED");
        }

        if (Input.GetButtonDown(player + "Button1") && !silenceActivated)
        {
            if (player == "Player1")
            {
                PressQuietButton();
            }else if (player == "Player2" && player2Active)
            {
                PressQuietButton();
            }

        }

        if (silenceActivated && silenceTimer >= silenceCooldown)
        {
            silenceActivated = false;
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

    private void PressQuietButton()
    {
        //do thing
        this.animator.SetTrigger("pushedQuietButton");

        EventManager.TriggerEvent("SILENT_ACTIVATED");
        GameStatusScript.KeepThemSecrets();

        silenceActivated = true;
        silenceTimer = 0f;
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
