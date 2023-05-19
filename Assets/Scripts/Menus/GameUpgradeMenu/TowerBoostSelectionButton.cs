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
    private string m_upgradeID;

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
        m_towerUpgradeService.TryGetTowerUpgradeInfo(towerUpgradeRow.UpgradeIDs[m_index], out TowerUpgradeBase towerUpgradeBase);
        OnTurretBoostChanged(towerUpgradeRow.TowerType, m_index, towerUpgradeBase);
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
                            Callback = () =>
                            {
                                m_towerUpgradeService.UpdateTowerBoostCollection(m_towerUpgradeCollection.LinkedTower.TowerTypeID, m_index, boostInfo);
                                m_towerUpgradeCollection.UpgradeMenu.CloseItemMenu();
                            }
                        });
                    }
                }
            }
        }

        m_towerUpgradeCollection.UpgradeMenu.OpenItemMenu(buttonInfos);
    }

    private void OnTurretBoostChanged(string towerType, int index, TowerUpgradeBase upgrade)
    {
        if (m_towerUpgradeCollection.LinkedTower == null || upgrade == null)
        {
            return;
        }

        if (m_towerUpgradeCollection.LinkedTower.TowerTypeID == towerType && m_index == index)
        {
            m_upgradeID = upgrade.ID;
            m_titleText.text = upgrade.Name;
        }
    }

    private void OnTowerSet()
    {
        m_titleText.text = "Select Upgrade";
        m_upgradeID = string.Empty;
        m_button.interactable = true;
    }
}
