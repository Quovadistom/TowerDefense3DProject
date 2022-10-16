using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUpgradeTreeBase : MonoBehaviour
{
    public TurretMediator m_turretMediator;

    internal void SetMediator(TurretMediator turretMediator)
    {
        m_turretMediator = turretMediator;
    }
}
