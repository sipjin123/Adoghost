using System.Collections.Generic;
using UnityEngine;

public class ChefPlayer : MonoBehaviour
{
       [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Food Pickup")]
    public float pickupRadius = 5f;
    public LayerMask foodLayer;

    private CharacterController controller;

    [SerializeField]
    public List<FoodItem> collectedFoods = new List<FoodItem>();
    
    [Header("Crock Pot Settings")]
    public float detectionRadius = 3f;
    public LayerMask crockPotLayer;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Ensure a valid LayerMask
        if (foodLayer.value == 0)
        {
            foodLayer = LayerMask.GetMask("Default");
            Debug.Log("No food layer set. Defaulting to 'Default' layer.");
        }
    }

    void Update()
    {
        HandleMovement();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CrockPot nearestPot = FindNearestCrockPot();
            if (nearestPot != null)
                nearestPot.TogglePower();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CrockPot nearestPot = FindNearestCrockPot();
            if (nearestPot != null)
                nearestPot.IncreaseHeat();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CrockPot nearestPot = FindNearestCrockPot();
            if (nearestPot != null)
                nearestPot.DecreaseHeat();
        }

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryPickupNearestFood();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            CrockPot nearestPot = FindNearestCrockPot();
            if (nearestPot != null)
            {
                nearestPot.FinishCooking();
            }
            else
            {
                Debug.Log("No crock pot nearby to cook from.");
            }
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            CrockPot nearestPot = FindNearestCrockPot();
            if (nearestPot != null)
            {
                TryAddToPot(nearestPot);
            }
            else
            {
                Debug.Log("No crock pot nearby.");
            }
        }
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.right * h + transform.forward * v;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void TryPickupNearestFood()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRadius, foodLayer);
        if (hits.Length == 0) return;

        FoodPickup nearest = null;
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            FoodPickup fp = hit.GetComponent<FoodPickup>();
            if (fp != null)
            {
                float dist = Vector3.Distance(transform.position, fp.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = fp;
                }
            }
        }

        if (nearest != null)
        {
            var newItem = new FoodItem(nearest.foodName, nearest.stats);
            collectedFoods.Add(newItem);
            Destroy(nearest.gameObject);
            Debug.Log($"Picked up and destroyed: {newItem.name}");
        }
    }

    CrockPot FindNearestCrockPot()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, crockPotLayer);
        float closestDist = float.MaxValue;
        CrockPot nearest = null;

        foreach (var hit in hits)
        {
            CrockPot pot = hit.GetComponent<CrockPot>();
            if (pot != null)
            {
                float dist = Vector3.Distance(transform.position, pot.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    nearest = pot;
                }
            }
        }

        return nearest;
    }

    void TryAddToPot(CrockPot pot)
    {
        if (pot == null || collectedFoods == null || collectedFoods.Count == 0)
        {
            Debug.Log("No pot or no food to add.");
            return;
        }

        int itemsAdded = 0;

        // Loop using index to safely modify the original list
        for (int i = collectedFoods.Count - 1; i >= 0; i--)
        {
            FoodItem food = collectedFoods[i];

            bool added = pot.AddIngredient(food, collectedFoods); // This handles both adding and removing
            if (added)
            {
                itemsAdded++;
            }

            // Stop if pot reached max capacity
            if (pot.GetCurrentIngredients().Count >= pot.maxCapacity)
                break;
        }

        Debug.Log($"Transferred {itemsAdded} item(s) to the crock pot.");
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
