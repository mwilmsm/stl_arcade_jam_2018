using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneScript : MonoBehaviour
{

	public GameObject target;
    public GameObject safeZone;

	private bool enemyStealing;
    private LineScript lineScript;

	private float timer = 0f;
	private int stealTime = 1;

	private GameStatusScript GameStatusScript;

	private float silentTimer;
	private float maxSilentTime;
	private bool isSilent;

    private bool isStunned;

	// Use this for initialization
	void Start()
	{
		enemyStealing = false;
		timer = 0f;

		isSilent = false;
		silentTimer = 0f;

        isStunned = false;

        EventManager.StartListening("STUN_ACTIVATED", OnStun);
        EventManager.StartListening("STUN_DEACTIVATED", EndStun);

        GameStatusScript = GameObject.Find("GameStatus").GetComponent<GameStatusScript>();
        lineScript = GameObject.Find("PlayerSoundWave").GetComponent<LineScript>();
    }

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;

		if (enemyStealing && !isSilent && !isStunned)
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

        TrackDangerZoneMovement();

	}

	public void TrackDangerZoneMovement()
	{
        Vector2 safeZonePos = safeZone.transform.position;
        Vector2 allyPos = target.transform.position;
        float height = lineScript.yScale * 0.5f;

		Vector2[] nodes = this.gameObject.GetComponent<PolygonCollider2D>().GetPath(0);

        float distance = lineScript.safeZoneRadius * 2f;
        Vector2 wideSide = (safeZonePos - allyPos) * distance;
        Vector2 normal = Vector2.Perpendicular(wideSide).normalized * height;

        nodes[0] = new Vector2(allyPos.x + wideSide.x + normal.x, allyPos.y + wideSide.y + normal.y);
        nodes[1] = new Vector2(allyPos.x + wideSide.x - normal.x, allyPos.y + wideSide.y - normal.y);
        nodes[2] = allyPos;

        this.gameObject.GetComponent<PolygonCollider2D>().SetPath(0, nodes);
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "enemy")
		{
			enemyStealing = true;
			LosingSecrets();
		}
	}

	public void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.name == "enemy")
		{
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

    public void OnStun()
    {
        this.isStunned = true;
	    gameObject.GetComponent<PolygonCollider2D>().enabled = false;
    }

    public void EndStun()
    {
        this.isStunned = false;
	    gameObject.GetComponent<PolygonCollider2D>().enabled = true;
    }
}
