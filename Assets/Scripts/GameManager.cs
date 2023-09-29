using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
        if(result)
        {
            //win
        }
        else
        {
            //defeat
        }
    }
}
