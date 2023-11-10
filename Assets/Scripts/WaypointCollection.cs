using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WaypointCollection : MonoBehaviour
{
    [SerializeField]
    private List<Transform> m_waypoints = new List<Transform>();
    private LevelService m_levelService;

    public IReadOnlyList<Transform> Waypoints
    {
        get { return m_waypoints; }
    }

    [Inject]
    public void Construct(LevelService levelService)
    {
        m_levelService = levelService;
    }

    private void Awake()
    {
    }
}
