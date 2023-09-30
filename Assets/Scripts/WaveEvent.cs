using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveEvent", menuName = "ScriptableObjects/WaveEvent", order = 1)]
public class WaveEvent : ScriptableObject
{
	[Range(.1f, 10)] public float WaitBefore = 1f;

	public GameObject enemyPrefab;
}
