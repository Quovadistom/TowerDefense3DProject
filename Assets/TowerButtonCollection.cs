using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TowerButtonCollection : MonoBehaviour
{
    private TurretCollection m_turretCollection;
    private SpawnTowerButton.Factory m_buttonFactory;

    [Inject]
    public void Construct(TurretCollection turretCollection, SpawnTowerButton.Factory factory)
    {
        m_turretCollection = turretCollection;
        m_buttonFactory = factory;
    }

    private void Awake()
    {
        foreach(TurretInfoComponent turretInfoComponent in m_turretCollection.TurretList)
        {
            SpawnTowerButton button = m_buttonFactory.Create();
            button.transform.SetParent(transform, false);
            button.TurretToSpawn = turretInfoComponent;
        }
    }
}
