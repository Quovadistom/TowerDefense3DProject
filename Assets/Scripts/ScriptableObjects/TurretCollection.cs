using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretCollection", menuName = "ScriptableObjects/TurretCollection")]
public class TurretCollection : ScriptableObject
{
    [SerializeField] private List<TowerInfoComponent> m_turretList;

    // Unity does not serialize interfaces or abstract classes, so add this manually
    private ITargetMethod[] m_targetMethodList = new ITargetMethod[3]
    {
        new TargetFirstEnemy(),
        new TargetCloseEnemy(),
        new TargetLastEnemy()
    };

    public IReadOnlyList<TowerInfoComponent> TurretList { get => m_turretList; }
    public IReadOnlyList<ITargetMethod> TargetMethodList { get => m_targetMethodList; }

    public bool TryGetTowerPrefab(string turretType, out TowerInfoComponent towerInfoComponent)
    {
        towerInfoComponent = TurretList.FirstOrDefault(tower => tower.ComponentID == turretType);
        return towerInfoComponent != null;
    }
}
