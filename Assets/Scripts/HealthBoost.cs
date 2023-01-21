using Unity.VisualScripting;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "HealthBoost", menuName = "ScriptableObjects/Boosts/GameBoosts/HealthBoost")]
public class HealthBoost : GameBoostBase
{
    [SerializeField] private int m_healthBoost;

    public override void ApplyBoost(ref GameBoostValues gameBoostValues)
    {
        gameBoostValues.Health += m_healthBoost;
    }
}