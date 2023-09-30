using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave", order = 2)]
public class Wave : ScriptableObject
{
	public List<WaveEvent> waveEventList;
}
