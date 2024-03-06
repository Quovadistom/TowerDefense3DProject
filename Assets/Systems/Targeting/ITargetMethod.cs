using System.Collections.Generic;

public interface ITargetMethod
{
    public string Name { get; }

    public bool TryGetTarget(TurretEnemyHandler turretEnemyHandler, IReadOnlyList<BasicEnemy> enemies, out BasicEnemy enemy);
}