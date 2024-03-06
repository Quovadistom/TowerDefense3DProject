using System;
using UnityEngine;

[Serializable]
public class TargetingModule : ModuleBase
{
    [HideInInspector] public ModuleDataTypeWithEvent<BasicEnemy> Target;
    public ModuleDataTypeWithEvent<float> TurnSpeed;
}