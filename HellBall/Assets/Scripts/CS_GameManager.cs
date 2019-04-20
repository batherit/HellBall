using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_GameManager : MonoBehaviour {
    public CS_PlayerController player;
    public CS_ElapsedTime elapsedTime;
    bool gameHasStarted;

    public delegate void DELEGATE_StartGame();
    public DELEGATE_StartGame ED_StartGame;

    public delegate void DELEGATE_EndGame();
    public DELEGATE_EndGame ED_EndGame;

    public delegate void DELEGATE_ResetGame();
    public DELEGATE_ResetGame ED_ResetGame;

    private void Start()
    {
        gameHasStarted = false;
        player.ED_Dead += EndGame;
    }

    public void StartGame()
    {
        if (gameHasStarted == false)
        {
            gameHasStarted = true;
            elapsedTime.Zero();
            elapsedTime.On();

            ED_StartGame();
        }
    }

    public void EndGame ()
    {
        if(gameHasStarted == true)
        {
            gameHasStarted = false;
            elapsedTime.Off();

            ED_EndGame();
        }
    }

    public void ResetGame ()
    {
        ED_ResetGame();
    }
}
