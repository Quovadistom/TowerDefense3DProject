using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveService
{
    private WaveSettings m_waveSettings;
    private SerializationService m_serializationService;
    private ModificationCollection m_modificationCollection;

    public event Action<Wave> StartWave;
    public event Action WaveComplete;
    public event Action<List<ModificationContainer>> ModificationsDrawn;

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

    public WaveService(WaveSettings waveSettings, SerializationService serializationService, ModificationCollection modificationCollection)
    {
        m_waveSettings = waveSettings;
        m_serializationService = serializationService;
        m_modificationCollection = modificationCollection;
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
