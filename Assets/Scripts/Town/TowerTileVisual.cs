using System;
using UnityEngine;
using Zenject;

public class TowerTileVisual : MonoBehaviour
{
    [SerializeField] private Transform[] m_updateLocations = new Transform[4];

    public Guid ID;
    private TownHousingService m_townHousingService;

    [Inject]
    private void Construct(Guid m_id, TownHousingService townHousingService)
    {
        ID = m_id;
        m_townHousingService = townHousingService;
    }

    private void Awake()
    {
        m_townHousingService.ServiceRead += ReadService;
        m_townHousingService.TileUpgradeApplied += OnTileUpgradeApplied;

        ReadService();
    }

    private void OnDestroy()
    {
        m_townHousingService.ServiceRead -= ReadService;
        m_townHousingService.TileUpgradeApplied -= OnTileUpgradeApplied;
    }

    private void ReadService()
    {
        HousingData housingData = m_townHousingService.GetHousingData(ID);
        SetTileUpgrades(housingData);
    }

    public void SetTileUpgrades(HousingData updates)
    {
        for (int i = 0; i < updates.ActiveUpgrades.Length; i++)
        {
            OnTileUpgradeApplied(updates, i);
        }
    }

    private void OnTileUpgradeApplied(HousingData housingData, int location)
    {
        if (housingData.TowerTypeGuid == ID)
        {
            m_updateLocations[location].ClearChildren();

            if (housingData.ActiveUpgrades[location] != null)
            {
                Instantiate(housingData.ActiveUpgrades[location].Visual, m_updateLocations[location], false);
            }
        }
    }

    public class Factory : PlaceholderFactory<TowerTileVisual, Guid, TowerTileVisual>
    {
    }
}
