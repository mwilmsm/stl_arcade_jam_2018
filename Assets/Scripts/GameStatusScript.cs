using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStatusScript : MonoBehaviour
{

//Should really be called Game Manager but at this point I don't want to break stuff that is mapping to the name
	
	public int SecretsListened;
	public float GameTime;

	public GameObject GameOverPanel;
	public GameObject GoodEndingPanel;
	public GameObject BadEndingPanel;

	public GameObject Timer;
	public GameObject CatTracker;

	public int MaxSecrets = 25;
	public float MaxGameTime = 240f;

	public String GameScene;

	private bool GameOver;
	private bool GoodEnding;
	private bool BadEnding;
	private Slider secretBar;
	
	private Color safe =  new Color(0f,1f,0f,1f);
	private Color lowSecret =  new Color(1f,1f,0f,1f);
	private Color midSecret =  new Color(1f,.5f,0f,1f);
	private Color highSecret =  new Color(1f,0f,0f,1f);

	private LineScript playerLineScript;
	private AllyLineScript AllyLineScript;

	private DangerZoneScript leftDangerZoneScript;
	private DangerZoneScript rightDangerZoneScript;
	
	public float timetoBeQuiet = 3f;
	public float stunTime = 3f;
	
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
		
		secretBar = CatTracker.GetComponentInChildren<Slider>();

		secretBar.maxValue = MaxSecrets;

		playerLineScript = GameObject.Find("PlayerSoundWave").GetComponent<LineScript>();
		AllyLineScript = GameObject.Find("AllySoundWave").GetComponent<AllyLineScript>();

		leftDangerZoneScript = GameObject.Find("DangerZoneLeft").GetComponent<DangerZoneScript>();
		rightDangerZoneScript = GameObject.Find("DangerZoneRight").GetComponent<DangerZoneScript>();
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
		Timer.GetComponentInChildren<TextMeshPro>().SetText(GameTime.ToString("F2"));
		
		if (Input.GetKey("escape"))
		{
			QuitGame();
		}

		UpdateSecretsBar();
	}

	private void UpdateSecretsBar()
	{
		
		secretBar.value = SecretsListened;

		Image fill = secretBar.GetComponentInChildren<Image>();
		
		if (secretBar.value >= (secretBar.maxValue * .9))
		{
			fill.color = highSecret;
		}
		else if (secretBar.value > (secretBar.maxValue * .75))
		{
			fill.color = midSecret;
		}else if (secretBar.value > (secretBar.maxValue * .5))
		{
			fill.color = lowSecret;
		}else if (secretBar.value > (secretBar.maxValue * .25))
		{
			fill.color = safe;
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

	public void SecretsStolen()
	{
		SecretsStolen(1);
	}

	public void SecretsStolen(int secrets)
	{
		SecretsListened += secrets;
	}
    
	public void KeepThemSecrets()
	{
		playerLineScript.SilenceTheLine(timetoBeQuiet);
		AllyLineScript.SilenceTheLine(timetoBeQuiet);
		
		leftDangerZoneScript.KeepThemSecrets(timetoBeQuiet);
		rightDangerZoneScript.KeepThemSecrets(timetoBeQuiet);
	}
}
