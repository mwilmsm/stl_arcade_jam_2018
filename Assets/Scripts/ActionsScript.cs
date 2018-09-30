using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsScript : MonoBehaviour {

	private AllyMovementScript AllyMovementScript;
	private bool player2Active;

	private GameStatusScript GameStatusScript;
	
	// Use this for initialization
	void Start () {
		
		player2Active = false;
		AllyMovementScript = GameObject.Find("Ally1").GetComponentInChildren<AllyMovementScript>();
		GameStatusScript = GameObject.Find("GameStatus").GetComponent<GameStatusScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetButtonDown("Player1Button1") || Input.GetButtonDown("Player2Button1"))
		{
			//do thing
			GameStatusScript.KeepThemSecrets();
		}
		if (Input.GetButtonDown("Player1Button2") || Input.GetButtonDown("Player2Button2"))
		{
			//do other thing
		}

        
		//Check if player two is trying to join
		if (!player2Active)
		{
			if (Input.GetButtonDown("Player2Button1"))
			{
				Player2Joined();
			}
			if (Input.GetButtonDown("Player2Button2"))
			{
				//do other thing
				Player2Joined();
			}
		}
	}
	
	public void Player2Joined()
	{
		AllyMovementScript.Player2Joined();
		player2Active = true;
	}
}
