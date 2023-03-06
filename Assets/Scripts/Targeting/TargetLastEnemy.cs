using System.Collections.Generic;
using System.Linq;
[System.Serializable]
public class TargetLastEnemy : ITargetMethod
{
    public int Order => 1;
    public string Name { get { return "Last"; } }

    public bool TryGetTarget(TurretEnemyHandler turretEnemyHandler, IReadOnlyList<BasicEnemy> enemies, out BasicEnemy enemy)
    {
        enemy = enemies.LastOrDefault();
        return enemy != null;
    }
}
