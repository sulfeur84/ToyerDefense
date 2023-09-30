using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class TowerScript : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float rateOfFire;
    [SerializeField] private Transform flag;
    [SerializeField] private Transform spawnEnemy;

    private List<EnemiesScript> _enemies;

    private float rotCnt;

    // Start is called before the first frame update
    void Start()
    {
        rotCnt = rateOfFire;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.GamePaused) return;
        
        if(rotCnt <= 0)
        {
            if(_enemies.Count > 0)
            {
                EnemiesScript target = null;
                float targetDist = 10000;
                foreach (var enemy in _enemies)
                {
                    if(enemy._hasFlag)
                    {
                        target = enemy;
                        break;
                    }

                    float dist = Vector3.Distance(enemy.transform.position, flag.position);

                    if (dist <= targetDist)
                    {
                        target = enemy;
                        targetDist = dist;
                    }
                }
                
                AttackTarget(target);
            }
        }
    }

    private void AttackTarget(EnemiesScript target)
    {
        target.GetDamaged(damage);
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemiesScript enemy = collision.gameObject.GetComponent<EnemiesScript>();
        if (enemy != null)
        {
            _enemies.Add(enemy);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        EnemiesScript enemy = collision.gameObject.GetComponent<EnemiesScript>();
        if (enemy != null)
        {
            _enemies.Remove(enemy);
        }
    }
}
