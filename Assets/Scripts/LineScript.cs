using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Reflection;


public class LineScript : MonoBehaviour {
 
	public GameObject gameObject1;          // Reference to the first GameObject
	public GameObject gameObject2;          // Reference to the second GameObject
 
	private LineRenderer line;                           // Line Renderer
    public float defaultSafeZoneRadius = 0.25f;
    private float stunnedSafeZoneRadius = 0.1f;
    public float safeZoneRadius;                  // as a percentage - the safe zone is 0.5 +/- safeZoneRadius

	public float scaleInputRange = 66f; // scale number from [0 to 99] to [0 to 2Pi]
	public float scaleResult = .5f;

	public float distanceBetween;

	public int linePoints = 100;
	public float sinFreq = 1;
	public float yScale = 1;
	public float xScale = 1;
	private float wiggle;
	
	public bool isAlly;

	private bool beQuiet;


	// Use this for initialization
	void Start () {
		// Add a Line Renderer to the GameObject
		line = this.gameObject.GetComponent<LineRenderer>();
		// Set the width of the Line Renderer
		line.SetWidth(0.05F, 0.05F);
        // Set the number of vertex of the Line Renderer
        //line.SetVertexCount(2);

        this.safeZoneRadius = this.defaultSafeZoneRadius;
        	
		
		wiggle = 0f;

		
		FindDistanceBetween();

		beQuiet = false;
		
		EventManager.StartListening("STUN_ACTIVATED", OnStun);
		EventManager.StartListening("STUN_DEACTIVATED", EndStun);
		
		EventManager.StartListening("SILENT_ACTIVATED", SilenceTheLine);
		EventManager.StartListening("SILENT_DEACTIVATED", StartTalking);
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

			wiggle += Time.deltaTime;

			float maxWiggle = (Mathf.PI * 2); 
			if (wiggle > maxWiggle)
			{
				wiggle = 0;
			}
			
			line.positionCount = linePoints;

			line.SetPosition(0, start);
			
			int safeZoneRange = Mathf.RoundToInt(linePoints * safeZoneRadius);
			
			for (int i = 1; i < linePoints; i++)
			{
				float x = ComputeX(start, end, i);
				
				
				if (i > safeZoneRange && i < (linePoints - safeZoneRange))
				{
					SetSinFreq(true, x);
				}
				else
				{
					SetSinFreq(false, x);
				}
				
				float y = ComputeY(x, start, end) * yScale;

				line.SetPosition(i, new Vector3(x, y, 0));
			}
			
			line.SetPosition(linePoints-1, end);
		}
	}

	private void FindDistanceBetween()
	{
		Vector2 playerPosition = gameObject1.transform.position;
		Vector2 allyPosition = gameObject2.transform.position;

		 distanceBetween =  Vector2.Distance(playerPosition, allyPosition);
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
		float y = 0f;
		if (beQuiet)
		{
			y = ComputeLine(x, start,stop);
		}
		else
		{
			y = ComputeSin(x) + ComputeLine(x, start,stop);
		}

		return y;
	}

	float ComputeLine(float x, Vector2 start, Vector2 stop)
	{
		return ComputeM(start, stop) * x + ComputeB(start, stop);
	}

	float ComputeSin(float x)
	{

		float sinValue;
		if (isAlly)
		{
			sinValue = Mathf.Sin(x - wiggle) * sinFreq;
		}
		else
		{
			sinValue = Mathf.Sin(x + wiggle) * sinFreq;
		}
		
		

		return sinValue;
	}
	void SetSinFreq(bool safeZone, float x)
	{
		sinFreq = 1;

		if (safeZone)
		{
			sinFreq = Mathf.Sin(15 * x);
		}

	}

	float ComputeX(Vector2 start, Vector2 stop, int currentPoint)
	{
		float nextPoint;
		float nextDistance = ((distanceBetween / linePoints) * currentPoint);
		
		if (nextDistance > distanceBetween)
		{
			nextDistance = distanceBetween;
		}

		if (isAlly)
		{
			nextPoint = start.x - (xScale * nextDistance);
			
			if (nextPoint < (stop.x * xScale))
			{
				nextPoint = stop.x;
			}
		}
		else
		{
			nextPoint = start.x + (xScale * nextDistance);
			
			if (nextPoint > (stop.x * xScale))
			{
				nextPoint = stop.x;
			}
		}

		return nextPoint;
	}

	public void SilenceTheLine()
	{
		beQuiet = true;  
    }

	public void StartTalking()
	{
		beQuiet = false;
		this.safeZoneRadius = this.defaultSafeZoneRadius;
	}

	private void OnStun()
	{
		this.safeZoneRadius = this.stunnedSafeZoneRadius;
	}

	private void EndStun()
	{
		this.safeZoneRadius = defaultSafeZoneRadius;
	}
}