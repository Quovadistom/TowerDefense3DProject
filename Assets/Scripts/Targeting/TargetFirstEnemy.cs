using System.Collections.Generic;
using System.Linq;

public class TargetFirstEnemy : ITargetMethod
{
    public int Order => 0;

    public string Name { get { return "First"; } }

    public bool TryGetTarget(TurretEnemyHandler turretEnemyHandler, IReadOnlyList<BasicEnemy> enemies, out BasicEnemy enemy)
    {
        enemy = enemies.FirstOrDefault();
        return enemy != null;
    }
}
