using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Wave", menuName = "New Wave")]
public class Wave : ScriptableObject {
    public float WaveTime;
    public float SpawnCD;
    public int SpawnAmount;
    public Enemy[] Enemies = new Enemy[0];
}