using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using UnityEditor.Experimental.UIElements;

public class LineScript : MonoBehaviour {
 
	public GameObject gameObject1;          // Reference to the first GameObject
	public GameObject gameObject2;          // Reference to the second GameObject
 
	private LineRenderer line;                           // Line Renderer
    public float safeZoneRadius;                  // as a percentage - the safe zone is 0.5 +/- safeZoneRadius

	public float scaleInputRange = 66f; // scale number from [0 to 99] to [0 to 2Pi]
	public float scaleResult = .5f;

	public float distanceBetween;

	public int linePoints = 100;
	public int sinRatio = 1;
	public float yScale = 1;
	public float xScale = 1;
 
	// Use this for initialization
	void Start () {
		// Add a Line Renderer to the GameObject
		line = this.gameObject.AddComponent<LineRenderer>();
		// Set the width of the Line Renderer
		line.SetWidth(0.05F, 0.05F);
		// Set the number of vertex of the Line Renderer
		//line.SetVertexCount(2);
		
		FindDistanceBetween();
	}
     
	// Update is called once per frame
	void Update () {
		// Check if the GameObjects are not null
		if (gameObject1 != null && gameObject2 != null)
		{
            Vector2 start = gameObject1.transform.position;
            Vector2 end = gameObject2.transform.position;

            // Update position of the two vertex of the Line Renderer



			FindDistanceBetween();
			

			line.positionCount = linePoints;

			line.SetPosition(0, start);
			for (int i = 1; i < linePoints; i++)
			{
				float x = ComputeX(start, i);
				float y = ComputeY(x, start, end) * yScale;
				line.SetPosition(i, new Vector3(x, y, 0));
			}
			
			line.SetPosition(linePoints-1, end);

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

	private void FindDistanceBetween()
	{
		Vector2 playerPosition = gameObject1.transform.position;
		Vector2 allyPosition = gameObject2.transform.position;

		 distanceBetween = Vector2.Distance(playerPosition, allyPosition);
	}
	
	float ComputeFunction(float x)
	{
		return Mathf.Sin(x);
	}

	float ComputeM(Vector2 start, Vector2 stop)
	{
		return ((start.y - stop.y) / (start.x - stop.x));
	}
	
	float ComputeB(Vector2 start, Vector2 stop)
	{
		return (start.y - ComputeM(start, stop) * start.x);
	}

	float ComputeY(float x, Vector2 start, Vector2 stop)
	{
		return Mathf.Sin(sinRatio * x) + ComputeM(start, stop) * x + ComputeB(start, stop);
	}

	float ComputeX(Vector2 start, int currentPoint)
	{
		return (start.x + (xScale * ((distanceBetween / linePoints) * currentPoint)));
	}
}