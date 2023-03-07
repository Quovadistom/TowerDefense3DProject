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

    private TurretUpgradeTreeBase m_activeTurretUpgradeTreeBase;
    private TowerInfoComponent m_activeTurrentInfo;

    private Dictionary<TowerInfoComponent, TurretUpgradeTreeBase> m_turretUpgradeTrees = new Dictionary<TowerInfoComponent, TurretUpgradeTreeBase>();

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
        TowerUpgradeComponent towerUpgradeComponent;

        if (m_activeTurretUpgradeTreeBase != null)
        {
            m_activeTurretUpgradeTreeBase.gameObject.SetActive(false);
        }
        if (m_activeTurrentInfo != null)
        {
            if (m_activeTurretUpgradeTreeBase.TryGetComponent(out towerUpgradeComponent))
            {
                towerUpgradeComponent.AvailableUpgradeCountChanged -= OnUpgradeCountChanged;
            }
        }

        m_activeTurrentInfo = selectedTurret;

        if (selectedTurret == null)
        {
            return;
        }

        if (m_turretUpgradeTrees.TryGetValue(selectedTurret, out TurretUpgradeTreeBase upgradeTree))
        {
            m_activeTurretUpgradeTreeBase = upgradeTree;
        }
        else
        {
            selectedTurret.UpgradeTreeAsset.transform.SetParent(m_viewPort, false);
            m_activeTurretUpgradeTreeBase = selectedTurret.UpgradeTreeAsset;
            m_turretUpgradeTrees.Add(selectedTurret, m_activeTurretUpgradeTreeBase);
        }

        if (selectedTurret.TryGetComponent(out towerUpgradeComponent))
        {
            OnUpgradeCountChanged(towerUpgradeComponent.TowerAvailableUpgradeCount);
            towerUpgradeComponent.AvailableUpgradeCountChanged += OnUpgradeCountChanged;
        }

        m_scrollRect.StopMovement();
        m_scrollRect.content = (RectTransform)m_activeTurretUpgradeTreeBase.transform;

        if (m_activeTurretUpgradeTreeBase.TryGetComponent(out Canvas canvas))
        {
            Destroy(canvas);
        }

        m_activeTurretUpgradeTreeBase.gameObject.SetActive(true);
    }

    private void OnUpgradeCountChanged(int count)
    {
        m_upgradeCountText.text = count.ToString();
        // Do something on count == 0?
    }
}
