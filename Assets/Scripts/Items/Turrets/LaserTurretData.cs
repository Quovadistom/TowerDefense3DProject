using System;
using UnityEngine;
using Zenject;

[Serializable]
public class LaserTurretData : TurretAndAttackData<LaserSpawnPoints>
{
    public float LaserLength = 5;

    private LayerSettings m_layerSettings;

    [Inject]
    public void Construct(LayerSettings layerSettings)
    {
        m_layerSettings = layerSettings;
    }

    private void Awake()
    {
        FiringMethod = new LaserFiringMethod(m_layerSettings, this);
    }
}
