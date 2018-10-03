using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZoneScript : MonoBehaviour {

    public GameObject leftCharacter;
    public GameObject rightCharacter;
    
    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = (leftCharacter.transform.position + rightCharacter.transform.position) / 2f;
    }
}
