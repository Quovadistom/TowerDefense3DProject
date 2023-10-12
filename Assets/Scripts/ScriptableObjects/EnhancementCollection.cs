using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

public enum EnhancementType
{
    GameEnhancement,
    TowerEnhancement
}

[CreateAssetMenu(fileName = "UpgradesCollection", menuName = "ScriptableObjects/UpgradesCollection")]
public class EnhancementCollection : ScriptableObject
{
    [SerializeField] private List<EnhancementContainer> m_enhancementList;

    [Header("Rarity Rates")]
    [SerializeField] private int m_maxWaveForRarity;
    [SerializeField] private Vector2 m_rarityRangeCommon;
    [SerializeField] private Vector2 m_rarityRangeLegendary;

    [Header("Spawn Rates")]
    [SerializeField] private int m_frequency;
    [SerializeField] private int m_enhancementAmount;

    public int Frequency => m_frequency;
    public int EnhancementAmount => m_enhancementAmount;
    public IReadOnlyList<EnhancementContainer> EnhancementList => m_enhancementList;

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
            Rarity.Common => 0,
            Rarity.Rare => 1,
            Rarity.Epic => 2,
            Rarity.Legendary => 3,
            _ => 0
        };

        float startingValue = m_rarityRangeCommon.x - ((m_rarityRangeCommon.x - m_rarityRangeLegendary.x) / (enumCount - 1)) * rarityLevel;

        return Mathf.FloorToInt(startingValue + (commonRarityValue + rarityLevel * step) * wave);
    }

    public EnhancementContainer GetRandomEnhancementWeighted(int wave)
    {
        IEnumerable<Rarity> rarities = EnhancementList.Select(enhancement => enhancement.Rarity);

        int[] calculatedWeights = new int[rarities.Count()];

        for (int i = 0; i < rarities.Count(); i++)
        {
            int accumulatedWeight = i > 0 ? calculatedWeights[i - 1] : 0;
            calculatedWeights[i] = GetWeight(rarities.ElementAt(i), wave) + accumulatedWeight;
        }

        int randomWeight = UnityEngine.Random.Range(0, calculatedWeights[^1]);

        int randomIndex = Array.IndexOf(calculatedWeights, calculatedWeights.FirstOrDefault(x => x > randomWeight));
        return EnhancementList[randomIndex];
    }
}
