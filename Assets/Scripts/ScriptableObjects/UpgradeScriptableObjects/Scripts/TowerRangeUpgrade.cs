using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerRangeUpgrade", menuName = "ScriptableObjects/Upgrades/TowerRangeUpgrade")]
public class TowerRangeUpgrade : Upgrade<RangeComponent>
{
    [SerializeField] private float m_increasePercentage;

    public override Action<RangeComponent> ComponentAction
    {
        get
        {
            return (component) =>
            {
                component.Range = component.Range.AddPercentage(m_increasePercentage);
            };
        }
    }
}