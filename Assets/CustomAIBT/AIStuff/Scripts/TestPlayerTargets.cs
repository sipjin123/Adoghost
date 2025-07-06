using UnityEngine;

public class TestPlayerTargets : MonoBehaviour, ICanBeKilled
{
    public bool isInvincible = false;
    public int health = 100;

    public Material deadMaterial;
    public bool CanBeKilled()
    {
        return !isInvincible && health > 0;
    }

    public void OnKilled()
    {
        Debug.Log($"{gameObject.name} has been killed!");
        GetComponent<Renderer>().material = deadMaterial;
        health = 0;
        float newScale = .25f;
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}
