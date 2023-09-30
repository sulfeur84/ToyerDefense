using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Values")]
    [Range(.5f, 20)] [SerializeField] private float waitBetweenWaves;
    
    [Header("SceneElements")]
    [SerializeField] private Transform enemies;
    [SerializeField] private Transform towers;
    [SerializeField] private Transform enemySpawn;
    [SerializeField] private Transform nodesList;
    [SerializeField] private Transform flagTransform;

    [Space]
    [SerializeField] private List<Wave> WaveList;

    private int _currentWaveInd = 0;
    private int _currentWaveEventInd = 0;
    private float _waveEventCnt = 0;
    private bool waveEnded = false;
    private bool NoMoreWave = false;

    public static bool GamePaused = false;
    
    public static Action<bool> EndGameA;

    // Start is called before the first frame update
    void Start()
    {
        EndGameA = EndGame;
        
        Wave currentWave = WaveList[_currentWaveInd];
        WaveEvent currentEvent = currentWave.waveEventList[_currentWaveEventInd];

        _waveEventCnt = currentEvent.WaitBefore;
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePaused) return;
        
        if (NoMoreWave)
        {
            if(enemies.childCount <= 0) EndGame(true);
            return;
        }

        if(waveEnded && enemies.childCount > 0) return;
        
        if(_waveEventCnt >= 0)
        {
            _waveEventCnt -= Time.deltaTime;
            return;
        }
        waveEnded = false;

        Wave currentWave = WaveList[_currentWaveInd];

        WaveEvent currentEvent = currentWave.waveEventList[_currentWaveEventInd];

        if(currentEvent.enemyPrefab != null) SpawnEnemy(currentEvent.enemyPrefab);

        _currentWaveEventInd++;

        if(_currentWaveEventInd >= currentWave.waveEventList.Count)//wave end
        {
            _currentWaveEventInd = 0;
            _currentWaveInd++;

            if(_currentWaveInd >= WaveList.Count)//No more Waves
            {
                NoMoreWave = true;
            }
            else
            {
                Debug.Log("End Wave");
                waveEnded = true;
                _waveEventCnt = waitBetweenWaves;
            }
        }
        else
        {
            Debug.Log("End Event");
            _waveEventCnt = currentEvent.WaitBefore;
        }
    }

    private void SpawnEnemy(GameObject toSpawnPrefab)
    {
        GameObject spawnedEnemy = Instantiate(toSpawnPrefab, enemySpawn.position, Quaternion.identity, enemies);
        
        spawnedEnemy.GetComponent<EnemiesScript>().AssignPath(nodesList, flagTransform);
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
