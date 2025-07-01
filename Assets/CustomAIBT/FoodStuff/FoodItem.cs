using System;
using UnityEngine;

[Serializable]
public class FoodItem
{
    public string name;
    public FoodStats stats;
    
    public FoodItem(string name, FoodStats stats)
    {
        this.name = name;
        this.stats = stats.Clone();
    }
}