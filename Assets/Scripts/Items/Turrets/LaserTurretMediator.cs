using System;
using UnityEngine;
using Zenject;

[Serializable]
public class LaserTurretMediator : MonoBehaviour
{
    public LaserSpawnPoints LaserSpawnPoints;
    public float LaserLength = 5;

    private LayerSettings m_layerSettings;

    [Inject]
    public void Construct(LayerSettings layerSettings)
    {
        m_layerSettings = layerSettings;
    }

    protected void Start()
    {
        //base.Start();
        //CurrentAttackMethod = new LaserFiringMethod(m_layerSettings, this);
    }
}
