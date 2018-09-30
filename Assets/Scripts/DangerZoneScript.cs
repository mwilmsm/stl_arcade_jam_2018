using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneScript : MonoBehaviour
{

	public GameObject target;

	private bool enemyStealing;
    private bool stunned;
    private bool quiet;

	private float timer = 0f;
	private int stealTime = 1;

	private GameStatusScript GameStatusScript;

	// Use this for initialization
	void Start()
	{
		enemyStealing = false;
		timer = 0f;

		GameStatusScript = GameObject.Find("GameStatus").GetComponent<GameStatusScript>();

        EventManager.StartListening("STUN_ACTIVATED", OnStunned);
        EventManager.StartListening("STUN_DEACTIVATED", EndStunned);
        EventManager.StartListening("QUIET_ACTIVATED", OnQuiet);
        EventManager.StartListening("QUIET_DEACTIVATED", EndQuiet);
    }

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;

		if (enemyStealing && !stunned && !quiet)
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

    public void OnStunned()
    {
        this.stunned = false;
    }

    public void EndStunned()
    {
        this.stunned = true;
    }

    public void OnQuiet()
    {
        this.quiet = true;
    }

    public void EndQuiet()
    {
        this.quiet = false;
    }
}
