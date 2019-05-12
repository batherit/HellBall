using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_CheckPoint : MonoBehaviour {
    [HideInInspector] public CS_MapGenerator mapGenerator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            CS_Managers.Instance.audioManager.soundCheck.Play();
            mapGenerator.IncreaseCheckedPointsNum();
            DestroyObject(gameObject);
        }
    }
}
