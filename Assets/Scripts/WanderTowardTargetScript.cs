using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderTowardTargetScript : MonoBehaviour {

    private Rigidbody2D rb2d;
    public GameObject target;
    public float wanderSpeed;
    private float wanderDirectionTimer;
    public float wanderDirectionUpdateTime;

    // Use this for initialization
    void Start () {
        this.rb2d = gameObject.GetComponent<Rigidbody2D>();
        Wander();
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > this.wanderDirectionTimer + this.wanderDirectionUpdateTime)
        {
            Wander();
        }
    }

    void Wander()
    {
        Vector2 direction = this.target.GetComponent<Transform>().position - this.transform.position;
        direction.Normalize();
        this.rb2d.velocity += direction * wanderSpeed;
        this.wanderDirectionTimer = Time.time;
    }
}
