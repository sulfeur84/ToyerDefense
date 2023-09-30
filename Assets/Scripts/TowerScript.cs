using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class TowerScript : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private int damage;
    [SerializeField] private float rateOfFire;
    
    [Header("SceneObjects")]
    [SerializeField] private Transform flag;
    [SerializeField] private Transform flashPoint;
    
    [Header("Prefabs")]
    [SerializeField] private GameObject muzzleFlash;
    

    private List<EnemiesScript> _enemies;

    private float rotCnt;

    // Start is called before the first frame update
    void Start()
    {
        rotCnt = rateOfFire;
        _enemies = new List<EnemiesScript>();
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
                    if (enemy.isDead)
                    {
                        _enemies.Remove(enemy);
                        continue;
                    }
                    if(enemy.hasFlag)
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
                
                Vector3 targetT = new Vector3(target.transform.position.x, 0, target.transform.position.z);
                Vector3 selfT = new Vector3(transform.position.x, 0, transform.position.z);
                
                Vector3 targetDirection = targetT - selfT;
                
                Vector3 dir = Vector3.RotateTowards(selfT, targetDirection, 180, 0);
                transform.rotation = Quaternion.LookRotation(dir);
                
                bool enemyDead = AttackTarget(target);

                if (enemyDead) _enemies.Remove(target);
                rotCnt = rateOfFire;
            }
        }
        else rotCnt -= Time.deltaTime;
    }

    private bool AttackTarget(EnemiesScript target)
    {
        Instantiate(muzzleFlash, flashPoint.position, flashPoint.rotation);
        return target.GetDamaged(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemiesScript enemy = other.gameObject.GetComponent<EnemiesScript>();
        if (enemy != null)
        {
            _enemies.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemiesScript enemy = other.gameObject.GetComponent<EnemiesScript>();
        if (enemy != null)
        {
            _enemies.Remove(enemy);
        }
    }

    public void AssignFlag(Transform flag)
    {
        this.flag = flag;
    }
}
