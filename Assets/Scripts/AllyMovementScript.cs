using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMovementScript : MonoBehaviour {
    
	public float walkSpeed = 10.0f;

	private Collider2D col2d;
	private Rigidbody2D rb2d;


	// Use this for initialization
	void Start()
	{
		this.col2d = gameObject.GetComponent<Collider2D>();
		this.rb2d = gameObject.GetComponent<Rigidbody2D>();

		
	}

	// Update is called once per frame
	void Update()
	{
		Vector2 velocity = new Vector2(0, 0);

		if (Input.GetAxis("Vertical2") > 0 && !Input.GetKey(KeyCode.DownArrow))
		{
			velocity.Set(velocity.x, walkSpeed);
		}
		if (Input.GetAxis("Vertical2") < 0 && !Input.GetKey(KeyCode.UpArrow))
		{
			velocity.Set(velocity.x, -(walkSpeed));
		}

		if (Input.GetAxis("Horizontal2") < 0 && !Input.GetKey(KeyCode.RightArrow))
		{
			velocity.Set(-(walkSpeed), velocity.y);
		}
		if (Input.GetAxis("Horizontal2") > 0 && !Input.GetKey(KeyCode.LeftArrow))
		{
			velocity.Set(walkSpeed, velocity.y);
		}

		this.rb2d.velocity = velocity;

		
	}

	public void Player2Joined()
	{
		this.enabled = true;
	}
	
	
}
