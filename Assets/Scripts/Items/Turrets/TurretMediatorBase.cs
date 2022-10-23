using System;
using UnityEngine;

public class RangeUpdate
{
    public RangeUpdate(float range, GameObject visual)
    {
        Range = range;
        Visual = visual;    
    }

    public float Range { get; private set; }
    public GameObject Visual { get; private set; }
}

public class TurretMediatorBase : MonoBehaviour
{
    public TurretUpgradeTreeBase UpgradeTreeAsset;
    public float Range = 4;

    public event Action<RangeUpdate> RangeUpdated;

    public void SetRange(RangeUpdate newRangeUpdate)
    {
        Range = newRangeUpdate.Range;
        RangeUpdated?.Invoke(newRangeUpdate);
    }
}