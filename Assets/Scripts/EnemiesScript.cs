using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class EnemiesScript : MonoBehaviour
{

    [Range(.1f, 7f)] [SerializeField] private float speed;
    [SerializeField] private Transform nodesList;
    [SerializeField] private Transform flagTransform;

    private Transform _targetNode;
    private int _cNodeInd = 1;

    private bool _endTouched = false;

    private bool _hasFlag = false;

    private float _flagSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _targetNode = nodesList.GetChild(_cNodeInd);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _hasFlag ?
            Vector3.MoveTowards(transform.position, _targetNode.position, _flagSpeed*Time.deltaTime)
            : Vector3.MoveTowards(transform.position, _targetNode.position, speed*Time.deltaTime);

        //todo : défaite si les ennemies retourche leur spawn avec l'objectif

        Vector3 selPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 flagPos = new Vector3(flagTransform.position.x, 0, flagTransform.position.z);
        
        //if touche l'objectif alors le récupere et revient dans les targets nodes
        if (Vector3.Distance(selPos, flagPos) < 0.05f && !_hasFlag)
        {
            _hasFlag = true;
            FlagScript.AddEnemiesA(this);
            _cNodeInd--;
            _targetNode = nodesList.GetChild(_cNodeInd);
        }
        
        if(Vector3.Distance(transform.position, _targetNode.position) < 0.001f)
        {
            if (nodesList.childCount <= _cNodeInd+1 || _endTouched || _hasFlag)
            {
                _cNodeInd--;
                _targetNode = nodesList.GetChild(_cNodeInd);
                _endTouched = true;
            }
            else
            {
                _cNodeInd++;
                _targetNode = nodesList.GetChild(_cNodeInd);
            }
        }
    }

    public void ChangeFlagSpeed(float flagSpeed)
    {
        _flagSpeed = flagSpeed;
    }
    
    private void Death()
    {
        FlagScript.RemoveEnemiesA(this);
        
        Destroy(gameObject);
    }
}
