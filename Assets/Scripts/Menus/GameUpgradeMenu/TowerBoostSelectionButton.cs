using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TowerBoostSelectionButton : MonoBehaviour
{
    [SerializeField] TowerBoostCollection m_towerUpgradeCollection;
    [SerializeField] Button m_button;
    [SerializeField] TMP_Text m_titleText;

    private TowerBoostService m_towerUpgradeService;
    private BoostAvailabilityService m_boostAvailabilityService;
    private Guid m_upgradeID;

    private int m_index = 0;

    [Inject]
    public void Construct(TowerBoostService towerUpgradeService, BoostAvailabilityService boostAvailabilityService)
    {
        m_towerUpgradeService = towerUpgradeService;
        m_boostAvailabilityService = boostAvailabilityService;
    }

    private void Awake()
    {
        m_button.onClick.AddListener(OnButtonClicked);
        m_button.interactable = false;
        m_index = transform.GetSiblingIndex();

        m_towerUpgradeService.TurretUpgradeChanged += OnTurretBoostChanged;
        m_towerUpgradeCollection.TowerSet += OnTowerSet;
    }

    private void OnEnable()
    {
        m_button.interactable = m_towerUpgradeCollection.LinkedTower != null;
        TowerBoostRow towerUpgradeRow = m_towerUpgradeService.TowerBoostRows.ToArray()[m_towerUpgradeCollection.Index];
        m_towerUpgradeService.TryGetTowerUpgradeInfo(towerUpgradeRow.UpgradeIDs[m_index], out BoostContainer boostContainer);
        OnTurretBoostChanged(towerUpgradeRow.TowerType, m_index, boostContainer);
    }

    private void OnDestroy()
    {
        m_towerUpgradeService.TurretUpgradeChanged -= OnTurretBoostChanged;
        m_towerUpgradeCollection.TowerSet -= OnTowerSet;
        m_button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        List<ButtonInfo> buttonInfos = new();

        IEnumerable<KeyValuePair<BoostContainer, int>> suitableBoosts = m_boostAvailabilityService.GetAvailableBoostList().Where(boost =>
        {
            return boost.Key.IsBoostSuitable(m_towerUpgradeCollection.LinkedTower);
        });

        foreach (var boost in suitableBoosts)
        {
            buttonInfos.Add(new ButtonInfo()
            {
                Title = boost.Key.Name.ToString(),
                Callback = () =>
                {
                    m_boostAvailabilityService.AddAvailableBoost(m_upgradeID);
                    boost.Key.TargetObjectID = m_towerUpgradeCollection.LinkedTower.ComponentID;
                    m_towerUpgradeService.UpdateTowerBoostCollection(m_towerUpgradeCollection.LinkedTower.ComponentID, m_index, boost.Key);
                    m_towerUpgradeCollection.UpgradeMenu.CloseItemMenu();
                }
            });
        }

        m_towerUpgradeCollection.UpgradeMenu.OpenItemMenu(buttonInfos);
    }

    private void OnTurretBoostChanged(Guid towerType, int index, BoostContainer upgrade)
    {
        if (m_towerUpgradeCollection.LinkedTower == null || upgrade == null)
        {
            return;
        }

        if (m_towerUpgradeCollection.LinkedTower.ComponentID == towerType && m_index == index)
        {
            m_upgradeID = upgrade.Name;
            m_titleText.text = upgrade.Name.ToString();
        }
    }

    private void OnTowerSet()
    {
        m_titleText.text = "Select Upgrade";
        m_upgradeID = Guid.Empty;
        m_button.interactable = true;
    }
}
