using UnityEngine;
using Zenject;

public class MapSpawner : MonoBehaviour
{
    [SerializeField] private Map m_currentMap;

    private LevelService m_levelService;

    [Inject]
    private void Construct(LevelService levelService)
    {
        m_levelService = levelService;
    }

    private void Awake()
    {
        if (m_levelService.Map != null)
        {
            Destroy(m_currentMap.gameObject);

            m_currentMap = Instantiate(m_levelService.Map, transform, false);
        }
        m_levelService.Map = m_currentMap;
    }
}
