using UnityEngine;

[System.Serializable]
public class FoodStats
{
    public string FoodName;

    public float CookStateValue = 0;
    
    [Range(0f, 5)]
    public float CookingSpeed = 1;
    
    [Range(0f, 100f)]
    public float quality = 100;

    [Range(0, 10)]
    public int aroma;

    [Range(0, 10)]
    public int saltiness;

    [Range(0, 10)]
    public int sweetness;

    [Range(0, 10)]
    public int spiciness;

    [Range(0, 10)]
    public int hungerSatisfaction;
    
    public FoodStats Clone()
    {
        return new FoodStats
        {
            CookStateValue = this.CookStateValue,
            FoodName = this.FoodName,
            CookingSpeed = this.CookingSpeed,
            quality = this.quality,
            aroma = this.aroma,
            saltiness = this.saltiness,
            sweetness = this.sweetness,
            spiciness = this.spiciness,
            hungerSatisfaction = this.hungerSatisfaction
        };
    }
}