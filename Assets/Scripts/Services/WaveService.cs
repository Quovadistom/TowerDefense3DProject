using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveService
{
    private WaveSettings m_waveSettings;
    private SerializationService m_serializationService;
    private EnhancementCollection m_enhancementCollection;

    public event Action<Wave> StartWave;
    public event Action WaveComplete;
    public event Action<List<EnhancementContainer>> EnhancementsDrawn;

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

    public WaveService(WaveSettings waveSettings, SerializationService serializationService, EnhancementCollection enhancementCollection)
    {
        m_waveSettings = waveSettings;
        m_serializationService = serializationService;
        m_enhancementCollection = enhancementCollection;
    }

    public void StartNextWave()
    {
        if (m_waveSettings.Waves.Count < m_currentWaveIndex)
        {
            return;
        }

        Wave selectedWave = m_waveSettings.Waves[m_currentWaveIndex];
        AliveEnemies = selectedWave.EnemyGroups.Sum(x => x.EnemyAmount);
        StartWave.Invoke(selectedWave);
        m_currentWaveIndex++;
    }

    public void EndWave()
    {
        WaveComplete.Invoke();

        if (m_currentWaveIndex % m_enhancementCollection.Frequency == 0)
        {
            Debug.Log($"Enhancement Drawn for {m_currentWaveIndex}!");
            List<EnhancementContainer> enhancementList = new List<EnhancementContainer>();
            for (int i = 0; i < m_enhancementCollection.EnhancementAmount; i++)
            {
                enhancementList.Add(m_enhancementCollection.GetRandomEnhancementWeighted(m_currentWaveIndex));
            }

            EnhancementsDrawn?.Invoke(enhancementList);
        }

        // m_serializationService.RequestSerialization();
    }
}
