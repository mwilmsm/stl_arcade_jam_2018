using UnityEngine;
using System.Collections;
 
public class LineScript : MonoBehaviour {
 
	public GameObject gameObject1;          // Reference to the first GameObject
	public GameObject gameObject2;          // Reference to the second GameObject
 
	private LineRenderer line;                           // Line Renderer
    public float safeZoneRadius;                  // as a percentage - the safe zone is 0.5 +/- safeZoneRadius
 
	// Use this for initialization
	void Start () {
		// Add a Line Renderer to the GameObject
		line = this.gameObject.AddComponent<LineRenderer>();
		// Set the width of the Line Renderer
		line.SetWidth(0.05F, 0.05F);
		// Set the number of vertex fo the Line Renderer
		line.SetVertexCount(2);
	}
     
	// Update is called once per frame
	void Update () {
		// Check if the GameObjects are not null
		if (gameObject1 != null && gameObject2 != null)
		{
            Vector2 start = gameObject1.transform.position;
            Vector2 end = gameObject2.transform.position;

            // Update position of the two vertex of the Line Renderer
            line.SetPosition(0, start);
			line.SetPosition(1, end);

            this.CheckForCollisions(start, end);
		}
	}

    void CheckForCollisions(Vector2 start, Vector2 end)
    {
        // Check if enemy is hitting the line
        RaycastHit2D raycast = Physics2D.Linecast(start, end, 1 << LayerMask.NameToLayer("Enemy"));
        if (raycast.collider != null
            && (raycast.fraction > (0.5 + this.safeZoneRadius)
            || raycast.fraction < (0.5 - this.safeZoneRadius)))
        {
            Debug.Log("Hit the enemy! " + Time.time);
        }
        // else add points
    }
}