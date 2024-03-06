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
    public IReadOnlyList<ITargetMethod> TargetMethodList { get => m_targetMethodList; }

    public bool TryGetAssets(Guid turretType, out TowerAssets towerInfoComponent)
    {
        towerInfoComponent = TurretAssetsList.FirstOrDefault(assets => assets.ID == turretType);
        return towerInfoComponent != null;
    }
}

[Serializable]
public class TowerAssets
{
    [SerializeField] private SerializableGuid m_id;
    [SerializeField] private TowerModule m_towerPrefab;
    [SerializeField] private GameObject m_turretVisualPrefab;
    [SerializeField] private TowerTileVisual m_housingPrefab;

    [SerializeField] private bool m_isStartingTower;

    public Guid ID => m_id;

    public TowerModule TowerPrefab => m_towerPrefab;

    public TowerTileVisual HousingPrefab => m_housingPrefab;

    public bool IsStartingTower => m_isStartingTower;
}
