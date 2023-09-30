using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Values")]
    [Range(5, 60)] [SerializeField] private float waitBeforeFirstWave;
    [Range(.5f, 20)] [SerializeField] private float waitBetweenWaves;
    [Range(100, 2000)] [SerializeField] private int startCurrency;
    
    [Header("SceneElements")]
    [SerializeField] private Transform enemies;
    [SerializeField] private Transform towers;
    [SerializeField] private Transform enemySpawn;
    [SerializeField] private Transform nodesList;
    [SerializeField] private Transform flagTransform;
    [SerializeField] private TextMeshProUGUI currencyDisplay;
    [SerializeField] private Transform wavesCntDisplay;

    [Space]
    [SerializeField] private List<Wave> waveList;

    private int _currentWaveInd = 0;
    private int _currentWaveEventInd = 0;
    private float _waveEventCnt = 0;
    private bool waveEnded = false;
    private bool NoMoreWave = false;

    private int _currency;

    public static bool GamePaused = false;
    public static TowerData TowerTypeSelected = null;
    
    public static Action<bool> EndGameA;
    public static Action<Transform> TowerInstantiateA;
    public static Action<int> GainMoneyA;

    // Start is called before the first frame update
    void Start()
    {
        EndGameA = EndGame;
        TowerInstantiateA = TowerInstantiate;
        GainMoneyA = GainMoney;

        _currency = startCurrency;
        
        /*Wave currentWave = waveList[_currentWaveInd];
        WaveEvent currentEvent = currentWave.waveEventList[_currentWaveEventInd];
        */

        currencyDisplay.text = _currency.ToString();

        wavesCntDisplay.GetChild(1).GetComponent<TextMeshProUGUI>().text = "/ " + waveList.Count;
        
        _waveEventCnt = waitBeforeFirstWave;
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

        Wave currentWave = waveList[_currentWaveInd];

        WaveEvent currentEvent = currentWave.waveEventList[_currentWaveEventInd];

        if(currentEvent.enemyPrefab != null) SpawnEnemy(currentEvent.enemyPrefab);

        _currentWaveEventInd++;

        if(_currentWaveEventInd >= currentWave.waveEventList.Count)//wave end
        {
            _currentWaveEventInd = 0;
            _currentWaveInd++;
            
            wavesCntDisplay.GetChild(0).GetComponent<TextMeshProUGUI>().text = _currentWaveInd.ToString();

            if(_currentWaveInd >= waveList.Count)//No more Waves
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

    public void TowerSelectionButton(TowerData towerData)
    {
        if (_currency >= towerData.GetCost())
        {
            TowerTypeSelected = towerData;
            
            //todo : Hide tower selection buttons and display yne cancel button
        }
        else
        {
            //todo : display error
            Debug.Log("Not enough Money!");
        }
    }

    private void TowerInstantiate(Transform towerBase)
    {
        GameObject tower = Instantiate(TowerTypeSelected.GetTowerGo(), towerBase.position, towerBase.rotation);

        tower.GetComponent<TowerScript>().AssignFlag(flagTransform);
        _currency -= TowerTypeSelected.GetCost();
        currencyDisplay.text = _currency.ToString();

        TowerTypeSelected = null;
    }

    private void GainMoney(int value)
    {
        _currency += value;
        currencyDisplay.text = _currency.ToString();
    }

    private void ResetStaticValues()
    {
        TowerTypeSelected = null;
        GamePaused = false;
    }

    private void OnDestroy()
    {
        ResetStaticValues();
    }
}
