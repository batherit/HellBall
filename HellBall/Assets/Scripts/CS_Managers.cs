using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Managers : MonoBehaviour {

    private static CS_Managers instance;

    public CS_GameManager gameManager;
    public CS_InputManager InputManager;
    public CS_AudioManager audioManager;

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
}
