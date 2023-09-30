using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "ScriptableObjects/TowerData", order = 3)]
public class TowerData : ScriptableObject
{
    [SerializeField] private int cost;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private GameObject towerPreview;

    public int GetCost()
    {
        return cost;
    }

    public GameObject GetTowerGo()
    {
        return towerPrefab;
    }

    public GameObject GetTowerPreviewGo()
    {
        return towerPreview;
    }
}
