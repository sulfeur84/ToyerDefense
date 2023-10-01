using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagScript : MonoBehaviour
{
    [Range(.1f, 7)] [SerializeField] private float speedPEnemies;
    [SerializeField] private Transform nodesList;
    [SerializeField] private Transform startTransform;
    
    private Transform _targetNode;
    private int _cNodeInd;
    private bool _isEnded = false;

    private List<EnemiesScript> _enemiesInRange;

    public static Action<EnemiesScript> AddEnemiesA;
    public static Action<EnemiesScript> RemoveEnemiesA;

    // Start is called before the first frame update
    void Start()
    {
        AddEnemiesA = AddEnemies;
        RemoveEnemiesA = RemoveEnemies;
        
        _cNodeInd = nodesList.childCount - 2;
        
        _targetNode = nodesList.GetChild(_cNodeInd);

        _enemiesInRange = new List<EnemiesScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_isEnded || GameManager.GamePaused) return;

        float speedTotal = 0;

        foreach (EnemiesScript enemy in _enemiesInRange)
        {
            speedTotal += enemy.GetSpeed() * speedPEnemies;
        }
        
        //float speed = speedPEnemies*_enemiesInRange.Count;
        
        float speed = speedTotal;
        
        transform.position = Vector3.MoveTowards(transform.position, _targetNode.position,  speed*Time.deltaTime);

        Vector3 targetPos = new Vector3(_targetNode.position.x, 0, _targetNode.position.z);
        Vector3 selfPos = new Vector3(transform.position.x, 0, transform.position.z);
        
        if(Vector3.Distance(selfPos, targetPos) < 0.001f)
        {
            if (0 <= _cNodeInd-1)
            {
                _cNodeInd--;
                _targetNode = nodesList.GetChild(_cNodeInd);
            }
            else //end player loose
            {
                _isEnded = true;
                GameManager.EndGameA(false);
            }
        }
    }

    private void AddEnemies(EnemiesScript enemiesScript)
    {
        _enemiesInRange.Add(enemiesScript);

        float speedTotal = 0;

        foreach (EnemiesScript enemy in _enemiesInRange)
        {
            speedTotal += enemy.GetSpeed() * speedPEnemies;
        }
        
        UpdateSpeed(speedTotal);
    }

    private void RemoveEnemies(EnemiesScript enemiesScript)
    {
        _enemiesInRange.Remove(enemiesScript);

        float speedTotal = 0;

        foreach (EnemiesScript enemy in _enemiesInRange)
        {
            speedTotal += enemy.GetSpeed() * speedPEnemies;
        }
        
        UpdateSpeed(speedTotal);
    }

    private void UpdateSpeed(float speed)
    {
        foreach (EnemiesScript enemies in _enemiesInRange)
        {
            enemies.ChangeFlagSpeed(speed);
        }
    }
}
