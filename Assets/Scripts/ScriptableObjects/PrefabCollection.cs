using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabCollection", menuName = "ScriptableObjects/PrefabCollection")]
public class PrefabCollection : ScriptableObject
{
    public BasicEnemy EnemyPrefab;
    public Bullet BulletPrefab;
    public TurretEnemyHandler BasicTurret;
}
