using System.Collections.Generic;
using System.Linq;

public class TargetFirstEnemy : ITargetMethod
{
    public string Name { get { return "First"; } }

    public bool TryGetTarget(TurretEnemyHandler turretEnemyHandler, IReadOnlyList<BasicEnemy> enemies, out BasicEnemy enemy)
    {
        enemy = enemies.OrderByDescending(x => x.DistanceTraveled).FirstOrDefault();

        return enemy != null;
    }
}
