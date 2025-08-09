using System;
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

    private FoodPickup HoveredFood;
    public float rayDistance = 5f; // How far the ray goes
    void DetectFood()
    {
        Camera cam = Camera.main; // Get the main camera

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // Try to get the FoodPickup component from the hit object
            FoodPickup food = hit.collider.GetComponent<FoodPickup>();
            HoveredFood = food;
            {
                //Debug.Log("Hit something, but it's not food");
            }
        }
        else
        {
            //Debug.Log("No object hit");
        }
    }

    void Update()
    {
        //HandleMovement();
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

        DetectFood();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //TryPickupNearestFood();
            
            
            if (HoveredFood != null)
            {
                Debug.Log("Pickup food: " + HoveredFood.foodName);
                // You can call a function on FoodPickup here, e.g.
                // food.PickUp();
                
                if (HoveredFood != null)
                {
                    var newItem = new FoodItem(HoveredFood.foodName, HoveredFood.stats);
                    collectedFoods.Add(newItem);
                    Destroy(HoveredFood.gameObject);
                    Debug.Log($"Picked up and destroyed: {newItem.name}");
                }
            }
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
                Debug.Log("Added food to crock pot nearby.");
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
        //Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, crockPotLayer);
        Collider[] hits = Physics.OverlapSphere(transform.position, 5, LayerMask.GetMask("Default"));
        float closestDist = float.MaxValue;
        CrockPot nearest = null;

        Debug.Log("Hit radius: " +hits.Length);
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

    private void OnGUI()
    {
        if (HoveredFood)
        {
            // Create centered style
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 24;
            style.normal.textColor = Color.white;

            // Draw label in middle of screen
            float width = Screen.width;
            float height = Screen.height;
            GUI.Label(new Rect(0, height / 2 - 12, width, 25), "Press E to Pick Up", style);
        }
    }
}
