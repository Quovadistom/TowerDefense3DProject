using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Rarity
{
    COMMON,
    RARE,
    EPIC,
    LEGENDARY
}

[Serializable]
public class BoostContainer<T>
{
    public T Boost;
    public Rarity Rarity = Rarity.COMMON;
}

[CreateAssetMenu(fileName = "UpgradesCollection", menuName = "ScriptableObjects/UpgradesCollection")]
public class BoostCollection : ScriptableObject
{
    [SerializeField] private List<BoostContainer<TowerUpgradeBase>> m_towerBoostList;
    [SerializeField] private List<BoostContainer<GameUpgradeBase>> m_gameBoostList;

    [Header("Rarity Rates")]
    [SerializeField] private int m_maxWaveForRarity;
    [SerializeField] private Vector2 m_rarityRangeCommon;
    [SerializeField] private Vector2 m_rarityRangeLegendary;

    [Header("Spawn Rates")]
    [SerializeField] private int m_frequency;
    [SerializeField] private int m_boostAmount;

    public int Frequency => m_frequency;
    public int BoostAmount => m_boostAmount;

    public IReadOnlyList<BoostContainer<TowerUpgradeBase>> TowerBoostList { get => m_towerBoostList; }
    public IReadOnlyList<BoostContainer<GameUpgradeBase>> GameBoostList { get => m_gameBoostList; }

    private int GetWeight(Rarity rarity, int wave)
    {
        // If above a certain wave, keep the same chances
        if (wave > m_maxWaveForRarity)
        {
            wave = m_maxWaveForRarity;
        }

        float commonRarityValue = (m_rarityRangeCommon.y - m_rarityRangeCommon.x) / (m_maxWaveForRarity - 0);
        float epicRarityValue = (m_rarityRangeLegendary.y - m_rarityRangeLegendary.x) / (m_maxWaveForRarity - 0);
        int enumCount = Enum.GetNames(typeof(Rarity)).Length;
        float step = (epicRarityValue - commonRarityValue) / (enumCount - 1);

        int rarityLevel = rarity switch
        {
            Rarity.COMMON => 0,
            Rarity.RARE => 1,
            Rarity.EPIC => 2,
            Rarity.LEGENDARY => 3,
            _ => 0
        };

        float startingValue = m_rarityRangeCommon.x - ((m_rarityRangeCommon.x - m_rarityRangeLegendary.x) / (enumCount - 1)) * rarityLevel;

        return Mathf.FloorToInt(startingValue + (commonRarityValue + rarityLevel * step) * wave);
    }

    public UpgradeBase GetRandomBoostWeighted(int wave)
    {
        Rarity[] rarities = m_towerBoostList.Select(boost => boost.Rarity).Concat(m_gameBoostList.Select(boost => boost.Rarity)).ToArray();

        int[] calculatedWeights = new int[rarities.Length];

        for (int i = 0; i < rarities.Length; i++)
        {
            int accumulatedWeight = i > 0 ? calculatedWeights[i - 1] : 0;
            calculatedWeights[i] = GetWeight(rarities[i], wave) + accumulatedWeight;
        }

        int randomWeight = UnityEngine.Random.Range(0, calculatedWeights[calculatedWeights.Length - 1]);

        int randomIndex = Array.IndexOf(calculatedWeights, calculatedWeights.FirstOrDefault(x => x > randomWeight));
        return m_towerBoostList.Select(boost => (UpgradeBase)boost.Boost).Concat(m_gameBoostList.Select(boost => (UpgradeBase)boost.Boost)).ToList()[randomIndex];
    }
}
