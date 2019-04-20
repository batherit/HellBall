using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ReadyPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
        CS_Managers.Instance.gameManager.ED_StartGame += StartGame;
        CS_Managers.Instance.gameManager.ED_ResetGame += ResetGame;
	}

    void StartGame()
    {
        gameObject.SetActive(false);
    }

    void ResetGame()
    {
        gameObject.SetActive(true);
    }
}
