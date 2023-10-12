using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class TowerTileVisual : MonoBehaviour
{
    [SerializeField] private Transform[] m_updateLocations = new Transform[4];
    private BoostCollection m_boostCollection;

    public Guid ID;
    private TownHousingService m_townHousingService;

    [Inject]
    private void Construct(Guid m_id, TownHousingService townHousingService, BoostCollection boostCollection)
    {
        ID = m_id;
        m_townHousingService = townHousingService;
        m_boostCollection = boostCollection;
    }

    private void Awake()
    {
        m_townHousingService.TileUpgradeApplied += OnTileUpgradeApplied;

        HousingData housingData = m_townHousingService.GetHousingData(ID);
        SetTileUpgrades(housingData.ActiveUpgrades);
    }

    private void OnDestroy()
    {
        m_townHousingService.TileUpgradeApplied -= OnTileUpgradeApplied;
    }

    private void OnTileUpgradeApplied(HousingData housingData, int location)
    {
        if (housingData.TowerTypeGuid == ID)
        {
            m_updateLocations[location].ClearChildren();

            BoostContainer boostContainer = m_boostCollection.BoostList.FirstOrDefault(boost => boost.ID == housingData.ActiveUpgrades[location]);

            if (boostContainer != null)
            {
                Instantiate(boostContainer.Visual, m_updateLocations[location], false);
            }
        }
    }

    public void SetTileUpgrades(Guid[] updates)
    {
        for (int i = 0; i < updates.Length; i++)
        {
            if (updates[i] != Guid.Empty)
            {
                GameObject visual = m_boostCollection.BoostList.FirstOrDefault(boost => boost.ID == updates[i] && boost.BoostType == BoostType.TowerBoost)?.Visual;
                if (visual != null)
                {
                    Instantiate(visual, m_updateLocations[i]);
                }
            }
        }
    }

    public class Factory : PlaceholderFactory<TowerTileVisual, Guid, TowerTileVisual>
    {
    }
}
