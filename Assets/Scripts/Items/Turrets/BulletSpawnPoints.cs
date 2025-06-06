using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnPoints : MonoBehaviour
{
    [SerializeField] private List<Transform> m_spawnPoints;
    public IReadOnlyList<Transform> SpawnPoints { get { return m_spawnPoints; } }  
}
