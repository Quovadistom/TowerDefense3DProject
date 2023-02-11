using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveService
{
    private WaveSettings m_waveSettings;
    private SerializationService m_serializationService;

    public event Action<Wave> StartWave;
    public event Action WaveComplete;

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

    public void EndWave()
    {
        WaveComplete.Invoke();
        // m_serializationService.RequestSerialization();
    }

    public WaveService(WaveSettings waveSettings, SerializationService serializationService)
    {
        m_waveSettings = waveSettings;
        m_serializationService = serializationService;
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
}
