using System;
using UnityEngine;

[Serializable]
public class TargetingComponent : ComponentBase
{
    [HideInInspector] public ComponentDataTypeWithEvent<BasicEnemy> Target;
    public ComponentDataTypeWithEvent<float> TurnSpeed;
}