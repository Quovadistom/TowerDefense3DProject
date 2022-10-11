using System.Collections.Generic;
using UnityEngine;

public interface IAttackMethod
{

    public void Shoot(BasicEnemy target);
    public void TargetLost();
}
