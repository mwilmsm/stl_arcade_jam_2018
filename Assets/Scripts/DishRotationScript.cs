using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishRotationScript : MonoBehaviour {

    public GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector2 direction = target.transform.position - this.transform.position;
        direction.Normalize();
        this.transform.right = direction;
    }
}
