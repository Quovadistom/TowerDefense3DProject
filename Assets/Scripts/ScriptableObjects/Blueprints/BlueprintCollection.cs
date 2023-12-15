using System.Collections.Generic;
using UnityEngine;

public enum BlueprintType
{
    Game,
    Tower
}

public class BlueprintCollection : ScriptableObject
{
    [SerializeField] private List<Blueprint> m_blueprints;

    public List<Blueprint> Blueprints => m_blueprints;
}