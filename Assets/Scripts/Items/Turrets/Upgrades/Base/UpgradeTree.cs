using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UpgradeTree : MonoBehaviour
{
    [SerializeField] private SelectedTurretMenu m_selectedTurretMenu;
    [SerializeField] private ScrollRect m_scrollRect;
    [SerializeField] private RectTransform m_viewPort;
    [SerializeField] private TMP_Text m_upgradeCountText;
    [SerializeField] private int m_availableUpgradeCount = 5;

    [SerializeField] private Transform m_treeParent;
    [SerializeField] private GameObject m_upgradeRow;

    private TowerInfoComponent m_activeTurrentInfo;
    private TowerUpgradeButton.Factory m_towerUpgradeButtonFactory;

    public event Action<int> AvailableUpgradeCountChanged;

    public int AvailableUpgradeCount
    {
        get => m_availableUpgradeCount;
        set
        {
            m_availableUpgradeCount = value;
            m_upgradeCountText.text = value.ToString();
            AvailableUpgradeCountChanged?.Invoke(value);
        }
    }

    [Inject]
    private void Construct(TowerUpgradeButton.Factory towerUpgradeButtonFactory)
    {
        m_towerUpgradeButtonFactory = towerUpgradeButtonFactory;
    }

    void Awake()
    {
        m_selectedTurretMenu.TurretDataChanged += OnTurretChanged;
        AvailableUpgradeCount = m_availableUpgradeCount;
    }

    private void OnDestroy()
    {
        m_selectedTurretMenu.TurretDataChanged -= OnTurretChanged;
    }

    private void OnTurretChanged(TowerInfoComponent selectedTurret)
    {
        if (selectedTurret != m_activeTurrentInfo)
        {
            foreach (Transform child in m_treeParent)
            {
                Destroy(child.gameObject);
            }
        }

        m_activeTurrentInfo = selectedTurret;

        if (selectedTurret != null)
        {
            CreateTurretUpgradeMenu(selectedTurret);
        }
    }

    private void CreateTurretUpgradeMenu(TowerInfoComponent selectedTurret)
    {
        TowerUpgradeTreeData towerUpgradeTreeData = selectedTurret.UpgradeTreeData;

        foreach (TowerUpgradeTreeRow towerUpgradeTreeStructure in towerUpgradeTreeData.Structure)
        {
            GameObject row = Instantiate(m_upgradeRow, m_treeParent, false);
            foreach (TowerUpgradeData upgrade in towerUpgradeTreeStructure.TowerUpgrades)
            {
                if (upgrade.IsBought)
                {
                    AvailableUpgradeCount--;
                }

                TowerUpgradeButton button = m_towerUpgradeButtonFactory.Create();
                button.transform.SetParent(row.transform, false);
                button.SetButtonInfo(towerUpgradeTreeData, upgrade, selectedTurret, this);
            }
        }
    }
}
