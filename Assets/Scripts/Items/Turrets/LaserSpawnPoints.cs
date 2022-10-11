using System.Collections.Generic;
using UnityEngine;

public class LaserSpawnPoints : MonoBehaviour
{
    [SerializeField] private List<LineRenderer> m_spawnPoints;
    public IReadOnlyList<LineRenderer> SpawnPoints { get { return m_spawnPoints; } }
}