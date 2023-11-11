using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveService
{
    private LevelService m_levelService;
    private SerializationService m_serializationService;
    private ModificationCollection m_modificationCollection;
    private WavesCollection m_wavesCollection;

    public event Action<Wave> StartWave;
    public event Action WaveComplete;
    public event Action<List<ModificationContainer>> ModificationsDrawn;
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

    public WaveService(LevelService levelService, SerializationService serializationService, ModificationCollection modificationCollection)
    {
        m_levelService = levelService;
        m_serializationService = serializationService;
        m_modificationCollection = modificationCollection;

        m_wavesCollection = m_levelService.MapInfo.WavesCollection;
    }

    public void StartNextWave()
    {
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

        if (m_currentWaveIndex % m_modificationCollection.Frequency == 0)
        {
            Debug.Log($"Modification Drawn for {m_currentWaveIndex}!");
            List<ModificationContainer> modificationList = new List<ModificationContainer>();
            for (int i = 0; i < m_modificationCollection.ModificationAmount; i++)
            {
                modificationList.Add(m_modificationCollection.GetRandomModificationWeighted(m_currentWaveIndex));
            }

            ModificationsDrawn?.Invoke(modificationList);
        }

        // m_serializationService.RequestSerialization();
    }
}
