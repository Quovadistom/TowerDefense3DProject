using System.Collections.Generic;

public interface ITargetMethod
{
    public int Order { get; }
    public string Name { get; }

    public bool TryGetTarget(TurretEnemyHandler turretEnemyHandler, IReadOnlyList<BasicEnemy> enemies, out BasicEnemy enemy);
}