using System.Threading.Tasks;
using System.Timers;
using UnityEngine;
using Zenject;

public class WaveManager : MonoBehaviour
{
    private EnemyService m_enemyService;
    private WaveService m_waveService;

    [Inject]
    public void Construct(EnemyService enemyService, WaveService waveService)
    {
        m_enemyService = enemyService;
        m_waveService = waveService;
    }

    private void Awake()
    {
        m_waveService.StartWave += OnNextWaveStarted;
    }

    private void OnDestroy()
    {
        m_waveService.StartWave -= OnNextWaveStarted;
    }

    private async void OnNextWaveStarted(Wave wave)
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
    }
}
