using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_DataBase : MonoBehaviour {

    private static CS_DataBase instance;

    private Hashtable guns = new Hashtable();

    private void Awake()
    {
        guns.Add("Gun", new S_GunInfo(E_ShotMode.Independent, 100.0f, 0.2f, 0.25f, 0.0f, 6, 0.8f));
        guns.Add("Rifle", new S_GunInfo(E_ShotMode.Independent, 100.0f, 0.2f, 0.1f, 0.0f, 30, 1.0f));
    }

    public static CS_DataBase Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType<CS_DataBase>();
            }

            return instance;
        }
    }

    public S_GunInfo GetGunInfo(string gunName)
    {
        return guns[gunName] as S_GunInfo;
    }
}
