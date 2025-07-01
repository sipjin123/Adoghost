using System;
using System.Collections.Generic;
using UnityEngine;

public class CrockPot : MonoBehaviour
{
    [Header("Crock Pot Settings")]
    public int maxCapacity = 5;
    
    public float heatLevel = 5f;
    public bool isOn = false;

    [Range(1f, 10f)]
    public float minHeat = 1f;
    public float maxHeat = 10f;
    public float heatStep = 1f;
    public Renderer potRenderer; 
    [SerializeField]
    public List<FoodItem> ingredients = new List<FoodItem>();

    /// <summary>
    /// Tries to add a FoodItem from the player's list to the crock pot.
    /// Removes it from the player's list if successful.
    /// </summary>
    
    private void Start()
    {
        if (wobbleTarget != null)
            initialWobbleRotation = wobbleTarget.localRotation;
    }

    public bool AddIngredient(FoodItem item, List<FoodItem> playerList)
    {
        if (ingredients.Count >= maxCapacity)
        {
            Debug.Log("Crock pot is full!");
            return false;
        }

        if (playerList.Contains(item))
        {
            ingredients.Add(item);
            playerList.Remove(item);
            Debug.Log($"Added {item.name} to the crock pot.");
            return true;
        }
        else
        {
            Debug.Log("Item not found in player's list.");
            return false;
        }
    }

    public void Clear()
    {
        ingredients.Clear();
    }

    public List<FoodItem> GetCurrentIngredients()
    {
        return new List<FoodItem>(ingredients);
    }
    
    public void FinishCooking()
    {
        isOn = false;
        heatLevel = 1;
        
        if (ingredients.Count == 0)
        {
            Debug.Log("Crock pot is empty.");
            return;
        }

        // Extract stats from each FoodItem
        List<FoodStats> statsList = new List<FoodStats>();
        foreach (var item in ingredients)
        {
            if (item != null && item.stats != null)
                statsList.Add(item.stats);
        }

        if (statsList.Count == 0)
        {
            Debug.Log("No valid food stats found.");
            return;
        }

        FoodStats result = FoodStatsUtility.CombineStats(statsList);

        if (result != null)
        {
            Debug.Log($"Cooked Result → Quality: {result.quality:F2}, Aroma: {result.aroma}, Saltiness: {result.saltiness}, Sweetness: {result.sweetness}, Spiciness: {result.spiciness}, Hunger: {result.hungerSatisfaction}");
        }

        ingredients.Clear();
    }
    
    public void TogglePower()
    {
        isOn = !isOn;
        Debug.Log($"Crock pot is now {(isOn ? "ON" : "OFF")}");
        
        Color targetColor = isOn ? Color.red : Color.black;
        potRenderer.material.color = targetColor;
    }

    public void IncreaseHeat()
    {
        if (!isOn)
        {
            Debug.Log("Crock pot is off. Can't adjust heat.");
            return;
        }

        heatLevel = Mathf.Min(heatLevel + heatStep, maxHeat);
        Debug.Log($"Heat increased to {heatLevel}");
    }

    public void DecreaseHeat()
    {
        if (!isOn)
        {
            Debug.Log("Crock pot is off. Can't adjust heat.");
            return;
        }

        heatLevel = Mathf.Max(heatLevel - heatStep, minHeat);
        Debug.Log($"Heat decreased to {heatLevel}");
    }
    void Update()
    {
        HandleCooking();
        HandleWobble();
    }
    
    float AdjustQuality(float baseQuality, float cookState)
    {
        float offset = Mathf.Abs(cookState - 100f); // Distance from perfect
        float penalty = Mathf.Clamp01(offset / 50f); // Max penalty at 150% or 50 under
        float adjustedQuality = baseQuality * (1f - penalty);
        return Mathf.Clamp(adjustedQuality, 0f, baseQuality);
    }
    
    void HandleCooking()
    {
        if (!isOn || ingredients.Count == 0)
            return;

        foreach (var item in ingredients)
        {
            if (item == null || item.stats == null)
                continue;

            float baseRate = 100f / 60f; // base cook rate per second
            float delta = baseRate * item.stats.CookingSpeed * (heatLevel/10) * Time.deltaTime;

            item.stats.CookStateValue += delta;
            item.stats.quality = AdjustQuality(100f, item.stats.CookStateValue);
        }
    }

    void OnGUI()
    {
        
        // === CONTROL INSTRUCTIONS (Top-Right) ===
        string controlsText =
            "Controls:\n" +
            "[Space] - Pick up food\n" +
            "[X] - End Cooking\n" +
            "[E] - Add to CrockPot\n" +
            "[1] - Turn on Crockpot\n" +
            "[2] - Increase Heat\n" +
            "[3] - Decrease Heat";

        float controlBoxWidth = 350f;
        float controlBoxHeight = 120f;
        float margin = 10f;

        Rect controlRect = new Rect(
            Screen.width - controlBoxWidth - margin,
            margin,
            controlBoxWidth,
            controlBoxHeight
        );

        GUI.color = new Color(1f, 1f, 1f, 0.85f); // slightly transparent white
        GUI.Box(controlRect, controlsText);
        GUI.color = Color.white;
        
        float barWidth = 300f;
        float barHeight = 20f;
        float startX = 10f;
        float startY = 10f;
        float spacing = 30f;
        GUIStyle barStyle = GUI.skin.box;

        // === CROCKPOT STATUS ===
        string statusText = isOn ? "CrockPot: ON" : "CrockPot: OFF";
        GUI.color = isOn ? Color.green : Color.red;
        GUI.Box(new Rect(startX, startY, barWidth, barHeight), statusText);
        GUI.color = Color.white;

        startY += spacing;

        // === HEAT LEVEL BAR ===
        float heatPercent = Mathf.Clamp01(heatLevel / 10f);
        GUI.Box(new Rect(startX, startY, barWidth, barHeight), "");

        Rect heatRect = new Rect(startX, startY, barWidth * heatPercent, barHeight);
        GUI.color = Color.yellow;
        GUI.Box(heatRect, $"Heat: {heatLevel:F1}/10");
        GUI.color = Color.white;

        startY += spacing * 2;

        // === INGREDIENT COOKING PROGRESS ===
        if (ingredients == null || ingredients.Count == 0) return;

        for (int i = 0; i < ingredients.Count; i++)
        {
            FoodItem item = ingredients[i];
            if (item == null || item.stats == null) continue;

            float cookPercent = Mathf.Clamp01(item.stats.CookStateValue / 100f);
            GUI.Box(new Rect(startX, startY + i * spacing, barWidth, barHeight), $"{item.stats.FoodName} - Cook: {item.stats.CookStateValue:F0}% - Quality: {item.stats.quality:F0}%");

            Rect fillRect = new Rect(startX, startY + i * spacing, barWidth * cookPercent, barHeight);
            GUI.color = Color.Lerp(Color.red, Color.green, cookPercent);
            GUI.Box(fillRect, "");
            GUI.color = Color.white;
        }
        
        // === FINAL COMBINED RESULT PREVIEW ===
        if (ingredients.Count > 0)
        {
            List<FoodStats> statsList = new List<FoodStats>();
            foreach (var item in ingredients)
            {
                if (item != null && item.stats != null)
                    statsList.Add(item.stats);
            }

            FoodStats result = FoodStatsUtility.CombineStats(statsList);
            if (result != null)
            {
                startY += ingredients.Count * spacing + spacing;

                string line1 = $"Result → Quality: {result.quality:F2}, Aroma: {result.aroma}, Saltiness: {result.saltiness}";
                string line2 = $"Sweetness: {result.sweetness}, Spiciness: {result.spiciness}, Hunger: {result.hungerSatisfaction}";

                GUI.color = Color.cyan;

                GUI.Box(new Rect(startX, startY, 400f, barHeight), line1);
                GUI.Box(new Rect(startX, startY + barHeight, 400f, barHeight), line2);

                GUI.color = Color.white;
            }
        }
    }
    
    void HandleWobble()
    {
        if (isOn && ingredients.Count > 0 && wobbleTarget != null)
        {
            float wobbleAngle = Mathf.Sin(Time.time * wobbleFrequency) * wobbleAmplitude;
            Quaternion wobble = Quaternion.Euler(0, 0, wobbleAngle);
            wobbleTarget.localRotation = initialWobbleRotation * wobble;
        }
        else if (wobbleTarget != null)
        {
            // Reset to original rotation
            wobbleTarget.localRotation = Quaternion.Slerp(wobbleTarget.localRotation, initialWobbleRotation, Time.deltaTime * 5f);
        }
    }
    
    [Header("Visual Effects")]
    public Transform wobbleTarget; // Assign the mesh or object to wobble
    public float wobbleAmplitude = 2f; // Degrees
    public float wobbleFrequency = 5f; // Speed
    private Quaternion initialWobbleRotation;
}