using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetCloseEnemy : ITargetMethod
{
    public string Name { get { return "Closest"; } }

    public bool TryGetTarget(TurretEnemyHandler turretEnemyHandler, IReadOnlyList<BasicEnemy> enemies, out BasicEnemy enemy)
    {
        enemy = enemies.OrderBy(enemy => Vector3.Distance(enemy.EnemyMiddle.position, turretEnemyHandler.transform.position)).FirstOrDefault();
        return enemy != null;
    }
}
