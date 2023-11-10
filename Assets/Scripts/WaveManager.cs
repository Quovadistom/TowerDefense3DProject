using UnityEngine;
using Zenject;

public class WaveManager : MonoBehaviour
{
    private EnemyService m_enemyService;
    private LevelService m_levelService;
    private WaveService m_waveService;
    private Wave m_currentWave;
    private EnemyGroup m_currentEnemyGroup;
    private int m_enemyGroupCount = 0;
    private int m_spawnedEnemyCount = 0;
    private bool m_waveActive = false;
    private float m_elapsedTime;

    [Inject]
    public void Construct(EnemyService enemyService, LevelService levelService, WaveService waveService)
    {
        m_enemyService = enemyService;
        m_levelService = levelService;
        m_waveService = waveService;
    }

    private void Awake()
    {
        m_waveService.StartWave += OnNextWaveStarted;
        m_waveService.WaveComplete += OnWaveComplete;
    }

    private void OnDestroy()
    {
        m_waveService.StartWave -= OnNextWaveStarted;
        m_waveService.WaveComplete -= OnWaveComplete;
    }

    private void Update()
    {
        if (!m_waveActive || m_currentWave == null)
        {
            return;
        }

        m_elapsedTime += Time.deltaTime;

        if (m_currentEnemyGroup != null && m_elapsedTime > m_currentEnemyGroup.EnemyDelay && m_spawnedEnemyCount < m_currentEnemyGroup.EnemyAmount)
        {
            for (int i = 0; i < m_levelService.Map.WaypointsCollection.Count; i++)
            {
                WaypointList waypoints = m_levelService.Map.WaypointsCollection[i];
                m_enemyService.CreateNewEnemy(m_currentEnemyGroup.Enemy, waypoints);
            }

            m_elapsedTime = 0;
            m_spawnedEnemyCount++;
        }
        else if (m_currentEnemyGroup == null ||
            (m_elapsedTime > m_currentEnemyGroup.GroupDelay &&
            m_enemyGroupCount < m_currentWave.EnemyGroups.Count &&
            m_spawnedEnemyCount == m_currentEnemyGroup.EnemyAmount))
        {
            m_currentEnemyGroup = m_currentWave.EnemyGroups[m_enemyGroupCount];
            m_spawnedEnemyCount = 0;
            m_elapsedTime = 0;
            m_enemyGroupCount++;
        }
    }

    private void OnNextWaveStarted(Wave wave)
    {
        m_waveActive = true;
        m_currentWave = wave;
    }

    private void OnWaveComplete()
    {
        m_waveActive = false;
        m_enemyGroupCount = 0;
    }
}
