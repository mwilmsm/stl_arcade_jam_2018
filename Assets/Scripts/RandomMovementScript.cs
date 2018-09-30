using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovementScript : MonoBehaviour
{

    public float walkSpeed = 10.0f;
    
    private Rigidbody2D rb2d;
    private float randomDirectionTimer;
    public float timeUntilDirectionChange = 1.0f;
    private Animator animator;

    // Use this for initialization
    void Start()
    {
        this.rb2d = gameObject.GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        this.MoveRandomly();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > this.randomDirectionTimer + this.timeUntilDirectionChange)
        {
            this.MoveRandomly();
        }
    }

    void MoveRandomly()
    {
        Vector2 velocity = new Vector2(0, 0);
        while (velocity == new Vector2(0, 0))
        {
            velocity = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
        }
        this.rb2d.velocity = velocity;
        this.animator.SetFloat("speed", velocity.magnitude);
        this.randomDirectionTimer = Time.time;
    }
}
