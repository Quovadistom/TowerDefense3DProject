using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TowerSelectionButton : MonoBehaviour
{
    [SerializeField] Button m_button;
    [SerializeField] TowerBoostCollection m_towerUpgradeCollection;

    private TurretCollection m_turretCollection;
    private TowerBoostService m_towerUpgradeService;
    private MenuService m_itemMenuService;

    [Inject]
    public void Construct(TurretCollection turretCollection, TowerBoostService towerUpgradeService, MenuService itemMenuService)
    {
        m_turretCollection = turretCollection;
        m_towerUpgradeService = towerUpgradeService;
        m_itemMenuService = itemMenuService;
    }

    private void Awake()
    {
        m_button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        m_button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        List<ButtonInfo> buttonInfos = new List<ButtonInfo>();

        IEnumerable<string> nonAvailableTowerTypes = m_towerUpgradeService.TowerBoostRows.Select(x => x.TowerType);
        IEnumerable<TowerInfoComponent> nonAvailableTowers = m_turretCollection.TurretList.Where(x => nonAvailableTowerTypes.Contains(x.TowerTypeID));

        foreach (TowerInfoComponent infoComponent in m_turretCollection.TurretList.Except(nonAvailableTowers))
        {
            buttonInfos.Add(new ButtonInfo()
            {
                Title = infoComponent.gameObject.name,
                Callback = () => m_towerUpgradeService.UpdateTowerUpgradeCollection(m_towerUpgradeCollection.Index, infoComponent.TowerTypeID)
            });
        }

        m_itemMenuService.RequestItemMenu(buttonInfos);
    }
}
