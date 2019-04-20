using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Managers : MonoBehaviour {

    private static CS_Managers instance;

    public CS_GameManager gameManager;
    public CS_UIButtonManager InputManager;

    public static CS_Managers Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType<CS_Managers>();
            }

            return instance;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
