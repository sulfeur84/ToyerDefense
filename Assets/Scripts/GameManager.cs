using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform enemies;
    [SerializeField] private Transform towers;
    
    public static bool GamePaused = false;
    
    public static Action<bool> EndGameA;

    // Start is called before the first frame update
    void Start()
    {
        EndGameA = EndGame;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EndGame(bool result)//false for player loose and true for player win
    {
        Debug.Log("End Game : "+result);
        PauseGame();
        if(result)
        {
            //win
        }
        else
        {
            //defeat
        }
    }

    private void PauseGame()
    {
        GamePaused = true;
    }

    private void UnpauseGame()
    {
        GamePaused = false;
    }
}
