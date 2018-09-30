using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStatusScript : MonoBehaviour
{


	public int SecretsListened;
	public float GameTime;

	public GameObject GameOverPanel;
	public GameObject GoodEndingPanel;
	public GameObject BadEndingPanel;

	public GameObject Timer;
	public GameObject CatTracker;

	public int MaxSecrets = 10;
	public float MaxGameTime = 240f;

	public String GameScene;

	private bool GameOver;
	private bool GoodEnding;
	private bool BadEnding;
	
	private Color safe =  new Color(0f,1f,0f,1f);
	private Color lowSecret =  new Color(1f,1f,0f,1f);
	private Color midSecret =  new Color(1f,.5f,0f,1f);
	private Color highSecret =  new Color(1f,0f,0f,1f);
	
	
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

		if (SecretsListened >= MaxSecrets)
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

		UpdateSecretsBar();
	}

	private void UpdateSecretsBar()
	{
		Slider secretBar = CatTracker.GetComponentInChildren<Slider>();
		secretBar.value = SecretsListened;

		Image fill = secretBar.GetComponentInChildren<Image>();
		
		if (secretBar.value > (secretBar.maxValue * .25))
		{
			fill.color = safe;
		}else if (secretBar.value > (secretBar.maxValue * .5))
		{
			fill.color = lowSecret;
		}else if (secretBar.value > (secretBar.maxValue * .75))
		{
			fill.color = midSecret;
		}else if (secretBar.value >= (secretBar.maxValue))
		{
			fill.color = highSecret;
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
