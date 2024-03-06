using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveService
{
    private LevelService m_levelService;
    private SerializationService m_serializationService;
    private ResourceCollection m_resourceCollection;
    private WavesCollection m_wavesCollection;

    public event Action<Wave> StartWave;
    public event Action WaveComplete;
    public event Action<List<Resource>> ModificationsDrawn;
    public bool IsLastWave => m_currentWaveIndex == m_wavesCollection.Waves.Count;

    private int m_currentWaveIndex = 0;

    private int m_aliveEnemies = 0;
    public int AliveEnemies
    {
        get => m_aliveEnemies;
        set
        {
            m_aliveEnemies = value;
            if (m_aliveEnemies == 0)
            {
                EndWave();
            }
        }
    }

    public WaveService(LevelService levelService, SerializationService serializationService, ResourceCollection modificationCollection)
    {
        m_levelService = levelService;
        m_serializationService = serializationService;
        m_resourceCollection = modificationCollection;
    }

    public void StartNextWave()
    {
        m_wavesCollection ??= m_levelService.MapInfo.WavesCollection;

        if (m_wavesCollection.Waves.Count < m_currentWaveIndex)
        {
            return;
        }

        Wave selectedWave = m_wavesCollection.Waves[m_currentWaveIndex];
        AliveEnemies = selectedWave.EnemyGroups.Sum(x => x.EnemyAmount);
        StartWave.Invoke(selectedWave);
        m_currentWaveIndex++;
    }

    public void EndWave()
    {
        WaveComplete.Invoke();

        if (m_currentWaveIndex % m_resourceCollection.Frequency == 0)
        {
            Debug.Log($"Modification Drawn for {m_currentWaveIndex}!");
            List<Resource> modificationList = new List<Resource>();
            for (int i = 0; i < m_resourceCollection.ResourceAmount; i++)
            {
                modificationList.Add(m_resourceCollection.GetRandomResourceWeighted(m_currentWaveIndex));
            }

            ModificationsDrawn?.Invoke(modificationList);
        }

        // m_serializationService.RequestSerialization();
    }
}
