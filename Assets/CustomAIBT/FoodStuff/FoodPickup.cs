using System;
using TMPro;
using UnityEngine;

public class FoodPickup : MonoBehaviour
{
    public string foodName;
    public FoodStats stats;
    public TextMeshProUGUI MeshText;
    private void Start()
    {
        MeshText.text = foodName;
    }
}