using System;
using UnityEngine;

public class Resource : ScriptableObject, IEquatable<Resource>
{
    [SerializeField] private string m_name;
    [SerializeField] private SerializableGuid m_id;
    [SerializeField] private Rarity m_rarity = Rarity.Common;
    [SerializeField] private Sprite m_visual;
    [SerializeField] private int m_startingAmount = 0;

    public string Name => m_name;
    public Guid ID => m_id;
    public Rarity Rarity => m_rarity;
    public Sprite Visual => m_visual;
    public int StartingAmount => m_startingAmount;

    public bool Equals(Resource otherResource) => ID == otherResource.ID;
}
