using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject alarmGo;

    [Space]
    [SerializeField] private List<Wave> waveList;

    private int _currentWaveInd = 0;
    private int _currentWaveEventInd = 0;
    private float _waveEventCnt = 0;
    private bool waveEnded = true;
    private bool NoMoreWave = false;

    private int _currency;

    public static bool GamePaused = false;
    public static TowerData TowerTypeSelected = null;
    
    public static Action<bool> EndGameA;
    public static Action<Transform> TowerInstantiateA;
    public static Action<int> GainMoneyA;
    public static Action<bool> TriggerAlarmA;

    private void Awake()
    {
        TriggerAlarmA = TriggerAlarm;
        EndGameA = EndGame;
        TowerInstantiateA = TowerInstantiate;
        GainMoneyA = GainMoney;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale *= 1.5f;

        _currency = startCurrency;

        currencyDisplay.text = _currency.ToString();

        wavesCntDisplay.GetChild(2).GetComponent<TextMeshProUGUI>().text = "/ " + waveList.Count;
        wavesCntDisplay.GetChild(1).GetComponent<TextMeshProUGUI>().text = (_currentWaveInd + 1).ToString();
        
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

        if (waveEnded)
        {
            waveEnded = false;
            SoundManager.PlaySfxA("NextRound");
        }

        Wave currentWave = waveList[_currentWaveInd];

        WaveEvent currentEvent = currentWave.waveEventList[_currentWaveEventInd];

        if(currentEvent.enemyPrefab != null) SpawnEnemy(currentEvent.enemyPrefab);

        _currentWaveEventInd++;

        if(_currentWaveEventInd >= currentWave.waveEventList.Count)//wave end
        {
            _currentWaveEventInd = 0;
            _currentWaveInd++;
            

            if(_currentWaveInd >= waveList.Count)//No more Waves
            {
                NoMoreWave = true;
            }
            else
            {
                wavesCntDisplay.GetChild(1).GetComponent<TextMeshProUGUI>().text = (_currentWaveInd + 1).ToString();
                Debug.Log("End Wave");
                waveEnded = true;
                _waveEventCnt = waitBetweenWaves;
            }
        }
        else
        {
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
            SceneManager.LoadScene("Victory");
        }
        else
        {
            //defeat
            SceneManager.LoadScene("Defeat");
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
            
            UiManager.HideShopCardA();
        }
        else
        {
            //todo : display error
            Debug.Log("Not enough Money!");
        }
    }

    public void CancelTowerSelection()
    {
        UiManager.DisplayShopCardA();
        TowerTypeSelected = null;
    }

    private void TowerInstantiate(Transform towerBase)
    {
        GameObject tower = Instantiate(TowerTypeSelected.GetTowerGo(), towerBase.position, towerBase.rotation, towers);

        tower.GetComponent<TowerScript>().AssignFlag(flagTransform);
        _currency -= TowerTypeSelected.GetCost();
        currencyDisplay.text = _currency.ToString();

        UiManager.DisplayShopCardA();
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

    private void TriggerAlarm(bool b)
    {
        alarmGo.SetActive(b);
    }
}
