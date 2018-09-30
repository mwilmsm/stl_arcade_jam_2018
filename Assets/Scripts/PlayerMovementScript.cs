using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovementScript : MonoBehaviour {
    
    public float walkSpeed = 10.0f;

    private Collider2D col2d;
    private Rigidbody2D rb2d;
    private Animator animator;

    private bool player2Active;
    private AllyMovementScript AllyMovementScript;

    // Use this for initialization
    void Start()
    {
        this.col2d = gameObject.GetComponent<Collider2D>();
        this.rb2d = gameObject.GetComponent<Rigidbody2D>();

        player2Active = false;
        AllyMovementScript = GameObject.Find("Ally1").GetComponentInChildren<AllyMovementScript>();
        this.animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = new Vector2(0, 0);

        if (Input.GetAxis("Vertical") > 0 && !Input.GetKey(KeyCode.DownArrow))
        {
            velocity.Set(velocity.x, walkSpeed);
        }
        if (Input.GetAxis("Vertical") < 0 && !Input.GetKey(KeyCode.UpArrow))
        {
            velocity.Set(velocity.x, -(walkSpeed));
        }

        if (Input.GetAxis("Horizontal") < 0 && !Input.GetKey(KeyCode.RightArrow))
        {
            velocity.Set(-(walkSpeed), velocity.y);
        }
        if (Input.GetAxis("Horizontal") > 0 && !Input.GetKey(KeyCode.LeftArrow))
        {
            velocity.Set(walkSpeed, velocity.y);
        }

        this.animator.SetFloat("speed", velocity.magnitude);
        this.rb2d.velocity = velocity;

       
    }
    
}
