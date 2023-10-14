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
        m_townHousingService.TileModificationApplied += OnTileModificationApplied;

        ReadService();
    }

    private void OnDestroy()
    {
        m_townHousingService.ServiceRead -= ReadService;
        m_townHousingService.TileModificationApplied -= OnTileModificationApplied;
    }

    private void ReadService()
    {
        HousingData housingData = m_townHousingService.GetHousingData(ID);
        SetTileModifications(housingData);
    }

    public void SetTileModifications(HousingData updates)
    {
        for (int i = 0; i < updates.ActiveModifications.Length; i++)
        {
            OnTileModificationApplied(updates, i);
        }
    }

    private void OnTileModificationApplied(HousingData housingData, int location)
    {
        if (housingData.TowerTypeGuid == ID)
        {
            m_updateLocations[location].ClearChildren();

            if (housingData.ActiveModifications[location] != null)
            {
                Instantiate(housingData.ActiveModifications[location].Visual, m_updateLocations[location], false);
            }
        }
    }

    public class Factory : PlaceholderFactory<TowerTileVisual, Guid, TowerTileVisual>
    {
    }
}
