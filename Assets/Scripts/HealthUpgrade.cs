using UnityEngine;

[CreateAssetMenu(fileName = "HealthBoost", menuName = "ScriptableObjects/Boosts/GameBoosts/HealthBoost")]
public class HealthUpgrade : GameUpgradeBase
{
    [SerializeField] private int m_healthBoost;

    public override GameUpgradeValues ApplyUpgrade(GameUpgradeValues gameBoostValues)
    {
        gameBoostValues.Health += m_healthBoost;
        return gameBoostValues;
    }
}