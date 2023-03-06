using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class TargetCloseEnemy : ITargetMethod
{
    public int Order => 2;

    public string Name { get { return "Closest"; } }

    public bool TryGetTarget(TurretEnemyHandler turretEnemyHandler, IReadOnlyList<BasicEnemy> enemies, out BasicEnemy enemy)
    {
        enemy = enemies.OrderBy(enemy => Vector3.Distance(enemy.EnemyMiddle.position, turretEnemyHandler.transform.position)).FirstOrDefault();
        return enemy != null;
    }
}
