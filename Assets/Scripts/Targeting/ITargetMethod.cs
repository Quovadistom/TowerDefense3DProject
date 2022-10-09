using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetMethod
{
    string Name { get; }

    public bool TryGetTarget(IReadOnlyList<BasicEnemy> enemies, out BasicEnemy enemy);
}