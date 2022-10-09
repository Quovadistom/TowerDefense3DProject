using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class WaveManager : MonoBehaviour
{
    private LevelService m_levelService;
    private EnemyService m_enemyService;
    private WaveSettings m_waveSettings;

    private void Awake()
    {
        OnNextWave();
    }

    [Inject]
    public void Construct(LevelService levelService, EnemyService enemyService, WaveSettings waveSettings)
    {
        m_levelService = levelService;
        m_enemyService = enemyService;
        m_waveSettings = waveSettings;
    }

    private async void OnNextWave()
    {
        m_levelService.StartLevel();
        m_enemyService.NewWaveStarted();

        foreach (Wave wave in m_waveSettings.Waves)
        {
            foreach (EnemyGroup enemyGroup in wave.EnemyGroups)
            {
                for (int i = 0; i < enemyGroup.EnemyAmount; i++)
                {
                    BasicEnemy basicEnemy = m_enemyService.CreateNewEnemy(enemyGroup.Enemy, transform.position);
                    await Task.Delay(enemyGroup.EnemyDelayMilliSeconds);
                }

                await Task.Delay(enemyGroup.GroupDelayMilliSeconds);
            }

            await Task.Delay(m_waveSettings.SecondsToNextWave * 1000);
        }
    }
}
