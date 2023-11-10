using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveSettings", menuName = "ScriptableObjects/WaveSettings")]
public class WavesCollection : ScriptableObject
{
    public List<Wave> Waves;

#if UNITY_EDITOR
    private void OnValidate()
    {
        for (int i = 0; i < Waves.Count; i++)
        {
            Wave wave = Waves[i];

            wave.SlotName = $"Wave {i + 1}";
            for (int j = 0; j < wave.EnemyGroups.Count; j++)
            {
                EnemyGroup enemyGroup = wave.EnemyGroups[j];
                string enemyType = enemyGroup.Enemy == null ? "Undefined" : enemyGroup.Enemy.GetType().ToString();
                enemyGroup.SlotName = $"Enemy Group {j + 1} ({enemyType})";
            }
        }
    }
#endif
}

[Serializable]
public class Wave
{
    [HideInInspector] public string SlotName;
    public List<EnemyGroup> EnemyGroups;
}

[Serializable]
public class EnemyGroup
{
    [HideInInspector] public string SlotName;
    public BasicEnemy Enemy;
    public int EnemyAmount;
    public float EnemyDelay;
    public float GroupDelay;
}
