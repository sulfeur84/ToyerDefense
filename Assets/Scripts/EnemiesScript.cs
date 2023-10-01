using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class EnemiesScript : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private int maxHp;
    [SerializeField] private int moneyGain;
    [Range(.1f, 7f)] [SerializeField] private float speed;
    
    [Header("SceneObjects")]
    [SerializeField] private Transform _nodesList;
    [SerializeField] private Transform _flagTransform;
    
    private int _hp;
    private Transform _targetNode;
    private int _cNodeInd = 1;
    private bool _endTouched = false;
    private bool _isEnded = false;
    
    [HideInInspector] public bool hasFlag = false;
    [HideInInspector] public bool isDead = false;

    private float _flagSpeed;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _targetNode = _nodesList.GetChild(_cNodeInd);
        _hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GamePaused || _isEnded) return;
        
        transform.position = hasFlag ?
            Vector3.MoveTowards(transform.position, _targetNode.position, _flagSpeed*Time.deltaTime)
            : Vector3.MoveTowards(transform.position, _targetNode.position, speed*Time.deltaTime);

        Vector3 targetDirection = _targetNode.position - transform.position;
        Vector3 dir = Vector3.RotateTowards(transform.position, targetDirection, 100, 0);
        transform.rotation = Quaternion.LookRotation(dir);

        //todo : défaite si les ennemies retourche leur spawn avec l'objectif

        Vector3 selPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 flagPos = new Vector3(_flagTransform.position.x, 0, _flagTransform.position.z);
        
        //if touche l'objectif alors le récupere et revient dans les targets nodes
        if (Vector3.Distance(selPos, flagPos) < 0.05f && !hasFlag)
        {
            hasFlag = true;
            FlagScript.AddEnemiesA(this);
            _cNodeInd--;
            _targetNode = _nodesList.GetChild(_cNodeInd);
        }
        
        if(Vector3.Distance(transform.position, _targetNode.position) < 0.001f)
        {
            if (_nodesList.childCount <= _cNodeInd+1 || _endTouched || hasFlag)
            {
                _cNodeInd--;
                if (_cNodeInd < 0)
                {
                    _isEnded = true;
                }
                else
                {
                    _targetNode = _nodesList.GetChild(_cNodeInd);
                    _endTouched = true;
                }
            }
            else
            {
                _cNodeInd++;
                _targetNode = _nodesList.GetChild(_cNodeInd);
            }
        }
    }

    public void ChangeFlagSpeed(float flagSpeed)
    {
        _flagSpeed = flagSpeed;
    }

    public bool GetDamaged(int damage)
    {
        _hp -= damage;
        //Debug.Log(gameObject.name + " has lost " + damage + " and has " + _hp + " left.");
        if (_hp <= 0)
        {
            Death();
            return true;
        }

        return false;
    }
    
    private void Death()
    {
        FlagScript.RemoveEnemiesA(this);
        GameManager.GainMoneyA(moneyGain);

        isDead = true;
        DestroySelf();
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void AssignPath(Transform nodesList, Transform flagTransform)
    {
        this._flagTransform = flagTransform;
        this._nodesList = nodesList;
    }
}
