using System.Collections.Generic;
using UnityEngine;

public static class FoodStatsUtility
{
    public static FoodStats CombineStats(List<FoodStats> allStats)
    {
        if (allStats == null || allStats.Count == 0)
            return null;

        float totalQuality = 0f;

        float aroma = 0;
        float saltiness = 0;
        float sweetness = 0;
        float spiciness = 0;
        float hungerSatisfaction = 0;

        foreach (var fs in allStats)
        {
            float weight = Mathf.Clamp01(fs.quality / 100f); // 100 means full effect
            totalQuality += fs.quality;

            aroma += fs.aroma * weight;
            saltiness += fs.saltiness * weight;
            sweetness += fs.sweetness * weight;
            spiciness += fs.spiciness * weight;
            hungerSatisfaction += fs.hungerSatisfaction * weight;
        }

        return new FoodStats
        {
            quality = totalQuality / allStats.Count,
            aroma = Mathf.RoundToInt(aroma),
            saltiness = Mathf.RoundToInt(saltiness),
            sweetness = Mathf.RoundToInt(sweetness),
            spiciness = Mathf.RoundToInt(spiciness),
            hungerSatisfaction = Mathf.RoundToInt(hungerSatisfaction)
        };
    }
}