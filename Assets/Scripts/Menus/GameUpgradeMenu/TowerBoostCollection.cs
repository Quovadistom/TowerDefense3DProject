using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TowerBoostCollection : MonoBehaviour
{
    [SerializeField] private TMP_Text m_itemTitle;
    [SerializeField] private Image m_itemIcon;

    public int Index { get; private set; } = 0;
    private TowerBoostService m_towerUpgradeService;
    private TurretCollection m_towerCollection;

    public event Action TowerSet;

    public TowerInfoComponent LinkedTower { get; private set; }

    [Inject]
    public void Construct(TowerBoostService towerUpgradeService, TurretCollection towerCollection)
    {
        m_towerUpgradeService = towerUpgradeService;
        m_towerCollection = towerCollection;
    }

    public void Awake()
    {
        Index = transform.GetSiblingIndex();
        TowerBoostRow towerUpgradeRow = m_towerUpgradeService.TowerBoostRows.ToArray()[Index];
        OnTurretTypeChanged(Index, towerUpgradeRow.TowerType);

        m_towerUpgradeService.TurretTypeChanged += OnTurretTypeChanged;
    }

    private void OnDestroy()
    {
        m_towerUpgradeService.TurretTypeChanged -= OnTurretTypeChanged;
    }

    private void OnTurretTypeChanged(int index, string type)
    {
        if (Index == index && m_towerCollection.TryGetTowerPrefab(type, out TowerInfoComponent towerInfoComponent))
        {
            SetTower(towerInfoComponent);
        }
    }

    public void SetTower(TowerInfoComponent tower)
    {
        LinkedTower = tower;
        m_itemTitle.text = tower.gameObject.name;
        TowerSet?.Invoke();
    }
}