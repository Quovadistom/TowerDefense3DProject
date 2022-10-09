using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemiesInRange : GenericRepository<BasicEnemy>
{
    public bool TryGetFirstEnemyInRange(out BasicEnemy selectedEnemy)
    {
        selectedEnemy = m_repoList.FirstOrDefault();
        return selectedEnemy != null;
    }    
    
    public bool TryGetLastEnemyInRange(out BasicEnemy selectedEnemy)
    {
        selectedEnemy = m_repoList.LastOrDefault();
        return selectedEnemy != null;
    }
}
