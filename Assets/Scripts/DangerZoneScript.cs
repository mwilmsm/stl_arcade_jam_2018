using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneScript : MonoBehaviour
{

	public GameObject target;
    public GameObject safeZone;
    
    private LineScript lineScript;

	private float timer = 0f;
	private int stealTime = 1;

	private GameStatusScript GameStatusScript;


	private bool isSilent;

    private bool isStunned;

	// Use this for initialization
	void Start()
	{
		timer = 0f;

        EventManager.StartListening("ENEMY_STUNNED", OnStun);
        EventManager.StartListening("ENEMY_UNSTUNNED", EndStun);
		
		EventManager.StartListening("SILENT_ACTIVATED", KeepThemSecrets);
		EventManager.StartListening("SILENT_DEACTIVATED", StartTalking);

        GameStatusScript = GameObject.Find("GameStatus").GetComponent<GameStatusScript>();
        lineScript = GameObject.Find("PlayerSoundWave").GetComponent<LineScript>();
    }

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;

		if (GameStatusScript.enemyInDangerZone && !isSilent && !isStunned)
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


        TrackDangerZoneMovement();

	}

	public void TrackDangerZoneMovement()
	{
        Vector2 safeZonePos = safeZone.transform.position;
        Vector2 allyPos = target.transform.position;
                
        Vector2 wideSide = (safeZonePos - allyPos) * lineScript.safeZoneRadius * 2f;
        Vector2 normal = Vector2.Perpendicular(wideSide).normalized * lineScript.yScale * 0.5f;

        // I have no clue why the - 1.5f is necessary, but all the danger zone boxes end up too high up if it isn't here. -mw
        Vector2[] nodes = new Vector2[3];
        nodes[0] = new Vector2(allyPos.x + wideSide.x + normal.x, allyPos.y + wideSide.y + normal.y);
        nodes[1] = new Vector2(allyPos.x + wideSide.x - normal.x, allyPos.y + wideSide.y - normal.y);
        nodes[2] = new Vector2(allyPos.x, allyPos.y);
        this.gameObject.GetComponent<PolygonCollider2D>().SetPath(0, nodes);
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "enemy")
		{
			if (!isSilent || !isStunned)
			{
				GameStatusScript.enemyInDangerZone = true;
				LosingSecrets();
			}
		}
	}

	public void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.name == "enemy")
		{
            GameStatusScript.enemyInDangerZone = false;
			timer = 0f;
		}
	}

	public void LosingSecrets()
	{
		//Call Game status secrets
		GameStatusScript.SecretsStolen();
	}

	public void KeepThemSecrets()
	{
		isSilent = true;
		gameObject.GetComponent<PolygonCollider2D>().enabled = false;
		GameStatusScript.enemyInDangerZone = false;
		timer = 0f;
	}
	
	public void StartTalking()
	{
		isSilent = false;
		gameObject.GetComponent<PolygonCollider2D>().enabled = true;
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
