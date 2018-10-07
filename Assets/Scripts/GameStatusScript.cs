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

    public bool enemyInDangerZone;

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

	public GameObject ReadySilenceButton;
	public GameObject CooldownSilenceButton;
	public GameObject ReadyP1StunButton;
	public GameObject ReadyP2StunButton;
	public GameObject CooldownP1StunButton;
	public GameObject CooldownP2StunButton;

	private bool player2;

	public bool Tutorial; 
	public GameObject tutorialScreen; 
	public GameObject[] tutorials; 
	private int currentTutorial = 0;
	
	
	// Use this for initialization
	void Start ()
	{
		GameTime = 0f;

		GameOver = false;
		GoodEnding = false;
		BadEnding = false;
        enemyInDangerZone = false;
		
		GameOverPanel.SetActive(false);
		GoodEndingPanel.SetActive(false);
		BadEndingPanel.SetActive(false);


		TutorialScreens();
		Time.timeScale = 0f;
		
		secretBar = CatTracker.GetComponentInChildren<Slider>();

		secretBar.maxValue = MaxSecrets;
		
		PlaySound("Gamebegin");
		PlaySound("Talking");
		
		EventManager.StartListening("STUN_ACTIVATED", OnStun);
		EventManager.StartListening("STUN_REFRESH", EndStunCooldown);
		
		EventManager.StartListening("PLAYER2_JOIN", JoinPlayer2);
		
		EventManager.StartListening("SILENT_ACTIVATED", OnSilent);
		EventManager.StartListening("SILENT_REFRESH", EndSlientCooldown);

		player2 = false;
	}
	
	// Update is called once per frame
	void Update()
	{
		GameTime += Time.deltaTime;

		//Update Game Timer

		if (GameTime > MaxGameTime && !GameOver)
		{
			GameOver = true;
			GoodEnding = true;
			GameOverScreen();
		}

		if (SecretsListened >= MaxSecrets && !GameOver)
		{
			GameOver = true;
			BadEnding = true;
			
			GameOverScreen();
		}

		if (Tutorial)
		{
			if (Input.GetButtonDown("Player1Button1") || Input.GetButtonDown("Player1Button2"))
			{
				NextTutorial();
			}else if (Input.GetButtonDown("Player1Button1") && Input.GetButtonDown("Player1Button2"))
			{
				StartGame();
			}
		}
		
		if (GameOver)
		{
			if (Input.GetButtonDown("Player1Button2"))
			{
				RestartGame();
			}
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
			PlaySound("FailureEnd");
			//Players lost because Cat stole too many secrets
			BadEndingPanel.SetActive(true);
		}

		if (GoodEnding)
		{
			PlaySound("Game End");
			//Players Survived the entire game
			GoodEndingPanel.SetActive(true);
		}

		
	}
	
	private void TutorialScreens()
	{
		Tutorial = true;
		tutorialScreen.SetActive(true);
		tutorials[0].SetActive(true);

		
	}

	private void NextTutorial()
	{
		tutorials[currentTutorial].SetActive(false);
		if (currentTutorial < tutorials.Length -1)
		{
			currentTutorial++;
			tutorials[currentTutorial].SetActive(true);
		}
		else
		{
			StartGame();
		}

	}

	private void PreviousTutorial()
	{
		
	}

	private void StartGame()
	{
		tutorialScreen.SetActive(false);
		Tutorial = false;
		Time.timeScale = 1f;	
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
        enemyInDangerZone = true;
		SecretsStolen(1);
	}

	public void SecretsStolen(int secrets)
	{
		SecretsListened += secrets;
		PlaySound("EnemyHears");
	}
    
//	public void KeepThemSecrets()
//	{
////		playerLineScript.SilenceTheLine(timetoBeQuiet);
////		AllyLineScript.SilenceTheLine(timetoBeQuiet);
//		
//		leftDangerZoneScript.KeepThemSecrets(timetoBeQuiet);
//		rightDangerZoneScript.KeepThemSecrets(timetoBeQuiet);
//	}
	
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
	
	public virtual void StopSound(string soundName)
	{
		AudioSource[] sounds = GetComponents<AudioSource>();

		for (int i = 0; i < sounds.Length; i++)
		{
			if (sounds[i].clip.name.Contains(soundName)) 
			{
				sounds[i].Stop();
			}
		}
	}

	private void JoinPlayer2()
	{
		player2 = true;
	}

	private void OnStun()
	{
		if (player2)
		{
			ReadyP1StunButton.SetActive(false);
			ReadyP2StunButton.SetActive(false);
			CooldownP2StunButton.SetActive(true);
		}
		else
		{
			ReadyP1StunButton.SetActive(false);
			CooldownP1StunButton.SetActive(true);
		}
	}

	private void EndStunCooldown()
	{
		if (player2)
		{
			ReadyP2StunButton.SetActive(true);
			CooldownP1StunButton.SetActive(false);
			CooldownP2StunButton.SetActive(false);
		}
		else
		{
			ReadyP1StunButton.SetActive(true);
			CooldownP1StunButton.SetActive(false);
			CooldownP2StunButton.SetActive(false);
		}
	}
	
	private void OnSilent()
	{
		ReadySilenceButton.SetActive(false);
		CooldownSilenceButton.SetActive(true);
		StopSound("Talking");
	}

	private void EndSlientCooldown()
	{
		ReadySilenceButton.SetActive(true);
		CooldownSilenceButton.SetActive(false);
		PlaySound("Talking");
	}
}
