using System.Collections.Generic;
using System.Linq;
[System.Serializable]
public class TargetLastEnemy : ITargetMethod
{
    public string Name { get { return "Last"; } }

    public bool TryGetTarget(IReadOnlyList<BasicEnemy> enemies, out BasicEnemy enemy)
    {
        enemy = enemies.LastOrDefault();
        return enemy != null;
    }
}
