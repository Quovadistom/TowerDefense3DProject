using System;
using UnityEngine;
using Zenject;

[Serializable]
public class LaserTurretMediator : BarrelTurretMediator
{
    public LaserSpawnPoints LaserSpawnPoints;
    public float LaserLength = 5;

    private LayerSettings m_layerSettings;

    [Inject]
    public void Construct(LayerSettings layerSettings)
    {
        m_layerSettings = layerSettings;
    }

    protected override void Awake()
    {
        base.Awake();
        FiringMethod = new LaserFiringMethod(m_layerSettings, this);
    }
}
