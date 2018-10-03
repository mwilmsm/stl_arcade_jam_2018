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
    private float dangerZoneHeight = 1.0f;

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

	}

	public void TrackDangerZoneMovement()
	{
        Vector2 safeZoneEdge = lineScript.transform.position;
        Vector2 allyEdge = target.transform.position;

		Vector2[] nodes = this.gameObject.GetComponent<PolygonCollider2D>().GetPath(0);

        float radius = lineScript.safeZoneRadius;
        Vector2 wideSide = new Vector2(
            ((safeZoneEdge.x - allyEdge.x) * lineScript.safeZoneRadius),
            ((safeZoneEdge.y - allyEdge.y) * lineScript.safeZoneRadius));
        Vector2 normal = Vector2.ClampMagnitude(Vector2.Perpendicular(safeZoneEdge - allyEdge), this.dangerZoneHeight);

        nodes[0] = new Vector2(wideSide.x + (normal.x * 0.5f), wideSide.y + (normal.y * 0.5f));
        nodes[1] = new Vector2(wideSide.x - (normal.x * 0.5f), wideSide.y - (normal.y * 0.5f));
        nodes[2] = allyEdge;

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
