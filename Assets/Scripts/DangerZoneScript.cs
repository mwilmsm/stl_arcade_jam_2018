using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneScript : MonoBehaviour {

    public GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
            Debug.Log("enemy entered zone");
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "enemy")
        {
            Debug.Log("                  enemy leaving trigger zone");
        }
    }
}
