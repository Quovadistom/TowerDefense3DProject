using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTree : MonoBehaviour
{
    [SerializeField] SelectedTurretMenu m_selectedTurretMenu;
    [SerializeField] ScrollRect m_scrollRect;
    [SerializeField] RectTransform m_viewPort;
    [SerializeField] TMP_Text m_upgradeCountText;

    [SerializeField] private Transform m_treeParent;
    [SerializeField] private GameObject m_upgradeRow;
    [SerializeField] private ButtonHelper m_upgradeButton;

    private TurretUpgradeTreeBase m_activeTurretUpgradeTreeBase;
    private TowerInfoComponent m_activeTurrentInfo;

    private Dictionary<TowerInfoComponent, TurretUpgradeTreeBase> m_turretUpgradeTrees = new Dictionary<TowerInfoComponent, TurretUpgradeTreeBase>();

    private List<ButtonHelper> m_buttonHelperList;

    void Awake()
    {
        m_selectedTurretMenu.TurretDataChanged += OnTurretChanged;
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
                ButtonHelper button = Instantiate(m_upgradeButton, row.transform, false);
                button.SetButtonInfo(towerUpgradeTreeData, upgrade, selectedTurret);
            }
        }
    }

    private void OnUpgradeCountChanged(int count)
    {
        m_upgradeCountText.text = count.ToString();
        // Do something on count == 0?
    }
}
