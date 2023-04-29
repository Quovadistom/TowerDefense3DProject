using System.Collections.Generic;
using System.Linq;

public class TargetLastEnemy : ITargetMethod
{
    public string Name { get { return "Last"; } }

    public bool TryGetTarget(TurretEnemyHandler turretEnemyHandler, IReadOnlyList<BasicEnemy> enemies, out BasicEnemy enemy)
    {
        enemy = enemies.LastOrDefault();
        return enemy != null;
    }
}
