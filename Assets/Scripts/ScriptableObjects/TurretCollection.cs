using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretCollection", menuName = "ScriptableObjects/TurretCollection")]
public class TurretCollection : ScriptableObject
{
    [SerializeField] private List<TurretInfoComponent> m_turretList;

    public IReadOnlyList<TurretInfoComponent> TurretList { get => m_turretList; }
}
