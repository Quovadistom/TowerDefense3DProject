using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesCollection", menuName = "ScriptableObjects/UpgradesCollection")]
public class BoostCollection : ScriptableObject
{
    [SerializeField] private List<TowerBoostBase> m_towerBoostList;
    [SerializeField] private List<GameBoostBase> m_gameBoostList;

    public IReadOnlyList<TowerBoostBase> TowerBoostList { get => m_towerBoostList; }
    public IReadOnlyList<GameBoostBase> GameBoostList { get => m_gameBoostList; }
}
