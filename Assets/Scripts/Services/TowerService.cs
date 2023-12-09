using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class TurretInfo
{
    public Guid UniqueTowerID { get; set; }
    public Guid TurretTypeID { get; set; }
    public Vector3 Position { get; set; }
    public TowerModificationTreeData TowerModificationTree { get; set; }
    public List<Guid> ConnectedSupportTowers { get; set; }
    public string TargetMethodName { get; set; }
}

public class TowerService : ServiceSerializationHandler<TurretServiceDto>
{
    public event Action<TowerModule> TowerModuleAdded;
    public event Action<TowerModule> TowerModuleRemoved;

    private List<TowerModule> m_placedTurrets = new();
    private TurretCollection m_turretCollection;
    private TowerModule.Factory m_turretFactory;

    public TowerService(TurretCollection turretCollection,
        TowerModule.Factory turretFactory,
        SerializationService serializationService,
        DebugSettings debugSettings) : base(serializationService, debugSettings)
    {
        m_turretCollection = turretCollection;
        m_turretFactory = turretFactory;
    }

    public void AddTower(TowerModule towerModule)
    {
        m_placedTurrets.Add(towerModule);
        TowerModuleAdded?.Invoke(towerModule);
    }

    public void RemoveTower(TowerModule towerModule)
    {
        m_placedTurrets.Remove(towerModule);
        TowerModuleRemoved?.Invoke(towerModule);
    }

    public int GetPlacedTowerAmount(Guid id) => m_placedTurrets.Count(tower => tower.ID == id);

    protected override Guid Id => Guid.Parse("c35801bc-3e17-4ae9-9b46-b0b6dd990769");

    protected override void ConvertDto()
    {
        List<TurretInfo> placedTurrets = new();

        foreach (TowerModule placedTurret in m_placedTurrets)
        {
            TurretInfo turretInfo = new TurretInfo()
            {
                UniqueTowerID = placedTurret.TowerID,
                TurretTypeID = placedTurret.ID,
                Position = placedTurret.transform.position,
                TowerModificationTree = placedTurret.ModificationTreeData,
                ConnectedSupportTowers = placedTurret.ConnectedSupportTowers
            };

            placedTurret.TryFindAndActOnModule<TargetMethodModule>((component) => turretInfo.TargetMethodName = component.TargetMethod.Name);

            placedTurrets.Add(turretInfo);
        }

        Dto.PlacedTurrets = placedTurrets;
    }

    protected override void ConvertDtoBack(TurretServiceDto dto)
    {
        foreach (TurretInfo selectedTurret in dto.PlacedTurrets)
        {
            TowerAssets assets = m_turretCollection.TurretAssetsList.FirstOrDefault(assets => assets.ID == selectedTurret.TurretTypeID);

            if (assets != null)
            {
                TowerModule placedTurret = m_turretFactory.Create(assets.TowerPrefab, assets.ID);
                placedTurret.PlaceNewTower(selectedTurret.UniqueTowerID, selectedTurret.Position, selectedTurret.TowerModificationTree, selectedTurret.ConnectedSupportTowers);

                placedTurret.TryFindAndActOnModule<TargetMethodModule>((component) =>
                component.TargetMethod = m_turretCollection.TargetMethodList.First(x => x.Name == selectedTurret.TargetMethodName));
            }
            else
            {
                Debug.LogError($"The turret of type {selectedTurret.TurretTypeID} does not exist! Check if the GUID has changed.");
            }
        }
    }
}

public class TurretServiceDto
{
    public List<TurretInfo> PlacedTurrets;
}
