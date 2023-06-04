using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveService
{
    private WaveSettings m_waveSettings;
    private SerializationService m_serializationService;
    private BoostCollection m_boostCollection;

    public event Action<Wave> StartWave;
    public event Action WaveComplete;
    public event Action<List<BoostContainer>> BoostsDrawn;

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

    public WaveService(WaveSettings waveSettings, SerializationService serializationService, BoostCollection boostCollection)
    {
        m_waveSettings = waveSettings;
        m_serializationService = serializationService;
        m_boostCollection = boostCollection;
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

        if (m_currentWaveIndex % m_boostCollection.Frequency == 0)
        {
            Debug.Log($"Boost Drawn for {m_currentWaveIndex}!");
            List<BoostContainer> boostList = new List<BoostContainer>();
            for (int i = 0; i < m_boostCollection.BoostAmount; i++)
            {
                boostList.Add(m_boostCollection.GetRandomBoostWeighted(m_currentWaveIndex));
            }

            BoostsDrawn?.Invoke(boostList);
        }

        // m_serializationService.RequestSerialization();
    }
}
