using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZoneScript : MonoBehaviour {

    public GameObject leftCharacter;
    public GameObject rightCharacter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.Stretch(leftCharacter.transform.position, rightCharacter.transform.position);
	}

    public void Stretch(Vector2 _initialPosition, Vector2 _finalPosition)
    {
        Vector2 centerPos = (_initialPosition + _finalPosition) / 2f;
        this.gameObject.transform.position = centerPos;

        Vector2 direction = _finalPosition - _initialPosition;
        direction.Normalize();
        this.gameObject.transform.right = direction;

        Vector2 scale = new Vector2(1, 1);
        //scale.x = Vector2.Distance(_initialPosition, _finalPosition);
        this.gameObject.transform.localScale = scale;
    }
}
