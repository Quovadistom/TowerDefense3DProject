using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretCollection", menuName = "ScriptableObjects/TurretCollection")]
public class TurretCollection : ScriptableObject
{
    [SerializeField] private List<TowerInfoComponent> m_turretList;

    public IReadOnlyList<TowerInfoComponent> TurretList { get => m_turretList; }

    public bool TryGetTowerPrefab(TowerType turretType, out TowerInfoComponent towerInfoComponent)
    {
        towerInfoComponent = TurretList.FirstOrDefault(tower => tower.TurretType == turretType);
        return towerInfoComponent != null;
    }
}
