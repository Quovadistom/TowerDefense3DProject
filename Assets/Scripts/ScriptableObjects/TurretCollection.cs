using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretCollection", menuName = "ScriptableObjects/TurretCollection")]
public class TurretCollection : ScriptableObject
{
    [SerializeField] private List<TowerAssets> m_turretList;

    // Unity does not serialize interfaces or abstract classes, so add this manually
    private ITargetMethod[] m_targetMethodList = new ITargetMethod[3]
    {
        new TargetFirstEnemy(),
        new TargetCloseEnemy(),
        new TargetLastEnemy()
    };

    public IReadOnlyList<TowerAssets> TurretAssetsList { get => m_turretList; }
    public IReadOnlyList<TowerInfoComponent> TurretList { get => m_turretList.Select(towerAssets => towerAssets.TowerPrefab).ToList(); }
    public IReadOnlyList<ITargetMethod> TargetMethodList { get => m_targetMethodList; }

    public bool TryGetTowerPrefab(Guid turretType, out TowerInfoComponent towerInfoComponent)
    {
        towerInfoComponent = TurretList.FirstOrDefault(tower => tower.ComponentID == turretType);
        return towerInfoComponent != null;
    }
}

[Serializable]
public class TowerAssets
{
    public TowerInfoComponent TowerPrefab;
    public GameObject TurretVisualPrefab;
    public TownTileVisual HousingPrefab;

    [SerializeField] private bool m_isStartingTower;
    public bool IsStartingTower => m_isStartingTower;
}
