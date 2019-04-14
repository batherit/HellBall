using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ObstacleSub : MonoBehaviour {

    public GameObject thomUp;
    public GameObject thomDown;

    // Use this for initialization
    void Start () {
        thomUp.transform.localPosition = new Vector3(Random.Range(-4.9f, 4.9f), 7.0f, 0.0f);
        thomDown.transform.localPosition = new Vector3(Random.Range(-4.9f, 4.9f), -7.0f, 0.0f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
