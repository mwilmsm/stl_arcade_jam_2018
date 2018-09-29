using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatusScript : MonoBehaviour
{


	public int SecretsListenedToo;
	public float GameTime;

	public GameObject GameOverPanel;
	public GameObject GoodEndingPanel;
	public GameObject BadEndingPanel;

	public GameObject Timer;

	public int MaxSecrets = 10;
	public float MaxGameTime = 240f;

	public String GameScene;

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
		
		GameOverPanel.SetActive(false);
		GoodEndingPanel.SetActive(false);
		BadEndingPanel.SetActive(false);
		
		Time.timeScale = 1f;
	}
	
	// Update is called once per frame
	void Update()
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

		if (GameOver)
		{
			GameOverScreen();
		}
		Timer.GetComponentInChildren<TextMeshPro>().SetText(GameTime.ToString());
		
		if (Input.GetKey("escape"))
		{
			QuitGame();
		}
	}

	private void GameOverScreen()
	{
		GameOverPanel.SetActive(true);
		Time.timeScale = 0;
		if (BadEnding)
		{
			//Players lost because Cat stole too many secrets
			BadEndingPanel.SetActive(true);
		}

		if (GoodEnding)
		{
			//Players Survived the entire game
			GoodEndingPanel.SetActive(true);
		}

		
	}

	public void RestartGame()
	{
		Time.timeScale = 1f;
		GameOver = false;
		SceneManager.LoadScene(GameScene);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
