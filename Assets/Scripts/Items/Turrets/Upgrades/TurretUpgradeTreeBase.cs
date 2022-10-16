using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUpgradeTreeBase : MonoBehaviour
{
    public TurretMediator TurretMediator { get; set; }

    internal void SetMediator(TurretMediator turretMediator)
    {
        TurretMediator = turretMediator;
    }
}
