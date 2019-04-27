using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_GameManager : MonoBehaviour {
    [HideInInspector] public CS_PlayerController player;
    [HideInInspector] public CS_ElapsedTime elapsedTime;

    public delegate void DELEGATE_StartGame();
    public DELEGATE_StartGame ED_StartGame;

    public delegate void DELEGATE_EndGame();
    public DELEGATE_EndGame ED_EndGame;

    public delegate void DELEGATE_ResetGame();
    public DELEGATE_ResetGame ED_ResetGame;

    private void Start()
    {
        player = FindObjectOfType<CS_PlayerController>();
        elapsedTime = FindObjectOfType<CS_ElapsedTime>();
        player.ED_Dead += EndGame;
    }

    public void StartGame()
    {
        elapsedTime.Zero();
        elapsedTime.On();

        ED_StartGame();
    }

    public void EndGame ()
    {
        elapsedTime.Off();

        ED_EndGame();
    }

    public void ResetGame ()
    {
        elapsedTime.Zero();

        ED_ResetGame();
    }
}
