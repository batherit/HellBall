using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ResultPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
        CS_Managers.Instance.gameManager.ED_EndGame += EndGame;
        CS_Managers.Instance.gameManager.ED_ResetGame += ResetGame;
        gameObject.SetActive(false);
	}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CS_Managers.Instance.gameManager.ED_ResetGame();
        }
    }

    public void EndGame()
    {
        gameObject.SetActive(true);
    }

    public void ResetGame()
    {
        gameObject.SetActive(false);
    }
}
