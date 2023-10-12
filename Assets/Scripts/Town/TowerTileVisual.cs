using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class TowerTileVisual : MonoBehaviour
{
    [SerializeField] private Transform[] m_updateLocations = new Transform[4];
    private EnhancementCollection m_enhancementCollection;

    public Guid ID;
    private TownHousingService m_townHousingService;

    [Inject]
    private void Construct(Guid m_id, TownHousingService townHousingService, EnhancementCollection enhancementCollection)
    {
        ID = m_id;
        m_townHousingService = townHousingService;
        m_enhancementCollection = enhancementCollection;
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

            EnhancementContainer enhancementContainer = m_enhancementCollection.EnhancementList.FirstOrDefault(enhancement => enhancement.ID == housingData.ActiveUpgrades[location]);

            if (enhancementContainer != null)
            {
                Instantiate(enhancementContainer.Visual, m_updateLocations[location], false);
            }
        }
    }

    public void SetTileUpgrades(Guid[] updates)
    {
        for (int i = 0; i < updates.Length; i++)
        {
            if (updates[i] != Guid.Empty)
            {
                GameObject visual = m_enhancementCollection.EnhancementList.FirstOrDefault(enhancement => enhancement.ID == updates[i] && enhancement.EnhancementType == EnhancementType.TowerEnhancement)?.Visual;
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
