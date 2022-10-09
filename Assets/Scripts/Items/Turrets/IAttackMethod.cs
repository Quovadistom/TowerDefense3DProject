using System.Collections.Generic;
using UnityEngine;

public interface IAttackMethod
{

    public void Shoot(BulletService bulletService, IReadOnlyList<Transform> bulletSpawnPointsList, BasicEnemy target);
}
