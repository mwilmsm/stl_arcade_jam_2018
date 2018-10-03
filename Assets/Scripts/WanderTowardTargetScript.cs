using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderTowardTargetScript : MonoBehaviour {

    private Rigidbody2D rb2d;
    public GameObject target;
    public float wanderSpeed;
    private float wanderDirectionTimer;
    public float wanderDirectionUpdateTime;

    private Animator animator;

    private bool stunned = false;

    // Use this for initialization
    void Start () {
        this.rb2d = gameObject.GetComponent<Rigidbody2D>();
        this.animator = gameObject.GetComponent<Animator>();
        EventManager.StartListening("ENEMY_STUNNED", OnStunned);
        EventManager.StartListening("ENEMY_UNSTUNNED", EndStunned);
        Wander();
    }

    // Update is called once per frame
    void Update() {
        if (Time.time > this.wanderDirectionTimer + this.wanderDirectionUpdateTime)
        {
            Wander();
        }
    }

    private void LateUpdate()
    {
        if (stunned)
        {
            this.rb2d.velocity = new Vector2(0, 0);
        }
    }

    void Wander()
    {
        Vector2 direction = this.target.GetComponent<Transform>().position - this.transform.position;
        direction.Normalize();
        this.rb2d.velocity += direction * wanderSpeed;
        this.wanderDirectionTimer = Time.time;
    }


    void OnStunned()
    {
        stunned = true;
        animator.SetTrigger("startleHit");
    }

    void EndStunned()
    {
        stunned = false;
    }
}
