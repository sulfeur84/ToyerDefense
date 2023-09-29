using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ennemiesScript : MonoBehaviour
{

    [Range(.1f, 5f)] [SerializeField] private float step;
    [SerializeField] private Transform nodesList;

    private Transform _targetNode;
    private int _cNodeInd = 1;

    private bool _endTouched = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _targetNode = nodesList.GetChild(_cNodeInd);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetNode.position, step*Time.deltaTime);

        //todo : if touche l'objectif alors le récupere (en enfant) et revient dans les targets nodes
        //todo : défaite si les ennemies retourche leur spawn avec l'objectif

        if(Vector3.Distance(transform.position, _targetNode.position) < 0.001f)
        {
            if (nodesList.childCount <= _cNodeInd+1 || _endTouched)
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
}
