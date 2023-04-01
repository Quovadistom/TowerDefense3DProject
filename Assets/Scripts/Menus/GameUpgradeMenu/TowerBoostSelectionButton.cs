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
    private MenuService m_itemMenuService;
    private string m_upgradeID;

    private int m_index = 0;

    [Inject]
    public void Construct(TowerBoostService towerUpgradeService, BoostAvailabilityService boostAvailabilityService, MenuService itemMenuService)
    {
        m_towerUpgradeService = towerUpgradeService;
        m_boostAvailabilityService = boostAvailabilityService;
        m_itemMenuService = itemMenuService;
    }

    private void Awake()
    {
        m_button.onClick.AddListener(OnButtonClicked);
        m_button.interactable = false;
        m_index = transform.GetSiblingIndex();

        m_button.interactable = m_towerUpgradeCollection.LinkedTower != null;
        TowerBoostRow towerUpgradeRow = m_towerUpgradeService.TowerBoostRows.ToArray()[m_towerUpgradeCollection.Index];
        OnTurretBoostChanged(towerUpgradeRow.TowerType, m_index, towerUpgradeRow.UpgradeIDs[m_index]);

        m_towerUpgradeService.TurretUpgradeChanged += OnTurretBoostChanged;
        m_towerUpgradeCollection.TowerSet += OnTowerSet;
    }

    private void OnDestroy()
    {
        m_towerUpgradeService.TurretUpgradeChanged -= OnTurretBoostChanged;
        m_towerUpgradeCollection.TowerSet -= OnTowerSet;
        m_button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        List<ButtonInfo> buttonInfos = new List<ButtonInfo>();

        ITowerComponent[] towerComponents = m_towerUpgradeCollection.LinkedTower.GetComponents<ITowerComponent>();

        foreach (KeyValuePair<string, int> boost in m_boostAvailabilityService.AvailableBoosts)
        {
            if (m_boostAvailabilityService.TryGetTowerBoostInformation(boost.Key, out TowerUpgradeBase boostInfo))
            {
                if (towerComponents.FirstOrDefault(component => component.GetType() == boostInfo.TowerComponentType) != null)
                {
                    for (int i = 0; i < boost.Value; i++)
                    {
                        buttonInfos.Add(new ButtonInfo()
                        {
                            Title = boostInfo.Name,
                            Callback = () => m_towerUpgradeService.UpdateTowerBoostCollection(m_towerUpgradeCollection.LinkedTower.TurretType, m_index, boostInfo.ID)
                        });
                    }
                }
            }
        }

        m_itemMenuService.RequestItemMenu(buttonInfos);
    }

    private void OnTurretBoostChanged(TowerType towerType, int index, string upgradeID)
    {
        if (m_towerUpgradeCollection.LinkedTower == null || string.IsNullOrEmpty(upgradeID))
        {
            return;
        }

        if (m_towerUpgradeCollection.LinkedTower.TurretType == towerType && m_index == index)
        {
            m_upgradeID = upgradeID;
            m_titleText.text = upgradeID;
        }
    }

    private void OnTowerSet()
    {
        m_titleText.text = "Select Upgrade";
        m_upgradeID = string.Empty;
        m_button.interactable = true;
    }
}
