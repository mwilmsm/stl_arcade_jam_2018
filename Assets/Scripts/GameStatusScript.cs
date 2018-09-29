using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatusScript : MonoBehaviour
{


	public int SecretsListenedToo;
	public float GameTime;

	public int MaxSecrets = 10;
	public float MaxGameTime = 240;

	private bool GameOver;
	private bool GoodEnding;
	private bool BadEnding;
	
	
	// Use this for initialization
	void Start ()
	{
		GameTime = 0f;

		GameOver = false;
		GoodEnding = false;
		BadEnding = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		while (!GameOver)
		{
			GameTime += Time.deltaTime;

			//Update Game Timer

			if (GameTime > MaxGameTime)
			{
				GameOver = true;
				GoodEnding = true;
			}

			if (SecretsListenedToo >= MaxSecrets)
			{
				GameOver = true;
				BadEnding = true;
			}
		}
		GameOverScreen();
	}

	private void GameOverScreen()
	{
		if (BadEnding)
		{
			//Players lost because Cat stole too many secrets
		}

		if (GoodEnding)
		{
			//Players Survived the entire game
		}

		
	}
}
