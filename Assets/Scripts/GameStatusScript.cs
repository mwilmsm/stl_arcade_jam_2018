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

	public GameObject ReadySilenceButton;
	public GameObject CooldownSilenceButton;
	public GameObject ReadyP1StunButton;
	public GameObject ReadyP2StunButton;
	public GameObject CooldownP1StunButton;
	public GameObject CooldownP2StunButton;

	private bool player2;
	
	
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
		
		PlaySound("Gamebegin");
		PlaySound("Talking");
		
		EventManager.StartListening("STUN_ACTIVATED", OnStun);
		EventManager.StartListening("STUN_DEACTIVATED", EndStun);
		
		EventManager.StartListening("PLAYER2_JOIN", JoinPlayer2);
		
		EventManager.StartListening("SILENT_ACTIVATED", OnSilent);
		EventManager.StartListening("SILENT_DEACTIVATED", EndSlient);

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
		PlaySound("EnemyHears");
	}
    
	public void KeepThemSecrets()
	{
		playerLineScript.SilenceTheLine(timetoBeQuiet);
		AllyLineScript.SilenceTheLine(timetoBeQuiet);
		
		leftDangerZoneScript.KeepThemSecrets(timetoBeQuiet);
		rightDangerZoneScript.KeepThemSecrets(timetoBeQuiet);
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

	private void EndStun()
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

	private void EndSlient()
	{
		ReadySilenceButton.SetActive(true);
		CooldownSilenceButton.SetActive(false);
		PlaySound("Talking");
	}
}
