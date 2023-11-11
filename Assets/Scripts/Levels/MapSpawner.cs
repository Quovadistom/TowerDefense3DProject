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
        if (m_levelService.MapInfo != null)
        {
            Destroy(m_currentMap.gameObject);

            m_currentMap = Instantiate(m_levelService.MapInfo, transform, false);
        }

        m_levelService.MapInfo = m_currentMap;
    }
}
