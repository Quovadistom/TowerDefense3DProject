using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveService
{
    private WaveSettings m_waveSettings;

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
                WaveComplete.Invoke();
            }
        }
    }

    public WaveService(WaveSettings waveSettings)
    {
        m_waveSettings = waveSettings;
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
