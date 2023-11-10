using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaypointList
{
    [SerializeField] private List<Transform> m_waypointList;

    public IReadOnlyList<Transform> Waypoints => m_waypointList;
}

public class Map : MonoBehaviour
{
    [SerializeField] private List<WaypointList> m_waypointsCollection;
    [SerializeField] private WavesCollection m_wavesCollection;

    public IReadOnlyList<WaypointList> WaypointsCollection => m_waypointsCollection;
    public WavesCollection WavesCollection => m_wavesCollection;
}
