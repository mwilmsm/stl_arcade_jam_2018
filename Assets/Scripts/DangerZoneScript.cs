using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneScript : MonoBehaviour
{

	public GameObject target;

	private bool enemyStealing;

	private float timer = 0f;
	private int stealTime = 1;

	private GameStatusScript GameStatusScript;

	private float silentTimer;
	private float maxSilentTime;
	private bool isSilent;

	// Use this for initialization
	void Start()
	{
		enemyStealing = false;
		timer = 0f;

		isSilent = false;
		silentTimer = 0f;

		GameStatusScript = GameObject.Find("GameStatus").GetComponent<GameStatusScript>();
	}

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;

		if (enemyStealing)
		{
			if (timer >= stealTime)
			{
				LosingSecrets();
				timer = 0f;
			}
		}
		else
		{
			timer = 0f;
		}

		if (isSilent)
		{
			silentTimer += Time.deltaTime;
			if (silentTimer > maxSilentTime)
			{
				isSilent = false;
				silentTimer = 0f;
				gameObject.GetComponent<PolygonCollider2D>().enabled = true;
			}
		}
		else
		{
			silentTimer = 0f;
		}

	}

	public void TrackDangerZoneMovement()
	{
		Vector2[] nodes = this.gameObject.GetComponent<PolygonCollider2D>().GetPath(0);
		nodes[3] = target.transform.position;
		this.gameObject.GetComponent<PolygonCollider2D>().SetPath(0, nodes);
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "enemy")
		{
			//Debug.Log("enemy entered zone");
			enemyStealing = true;
			LosingSecrets();
		}
	}

	public void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.name == "enemy")
		{
			//Debug.Log("                  enemy leaving trigger zone");
			enemyStealing = false;
			timer = 0f;
		}
	}

	public void LosingSecrets()
	{
		//Call Game status secrets
		GameStatusScript.SecretsStolen();
	}

	public void KeepThemSecrets(float timeToBeQuiet)
	{
		maxSilentTime = timeToBeQuiet;
		isSilent = true;
		gameObject.GetComponent<PolygonCollider2D>().enabled = false;
	}
}
