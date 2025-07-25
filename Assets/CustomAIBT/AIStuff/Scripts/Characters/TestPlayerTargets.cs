using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class TestPlayerTargets : MonoBehaviour, ICanBeKilled
{
    public bool isInvincible = false;
    public int health = 100;

    public Material deadMaterial;
    public Material carryMaterial;
    public Material aliveMaterial;
    public bool corpseCarried;
    public bool isCorpseWithinCorpseZone;
    public Transform Limbo;
    public bool isInLimbo;
    private void Start()
    {
        aliveMaterial = GetComponent<Renderer>().material;
    }

    public bool CanBeKilled()
    {
        return !isInvincible && health > 0;
    }

    public void OnKilled()
    {
        Debug.Log($"{gameObject.name} has been killed!");
        GetComponent<Renderer>().material = deadMaterial;
        GetComponentInChildren<CTAnimPlayer>().PlayAnimation(CTAnimPlayer.CharacterAnimation.Death);
        health = 0;
        ToggleControl(false);
    }

    public void OnKilledAsGhost()
    {
        Debug.Log($"{gameObject.name} has been killed!");
        GetComponent<Renderer>().material = deadMaterial;
        GetComponentInChildren<CTAnimPlayer>().PlayAnimation(CTAnimPlayer.CharacterAnimation.Death);
        health = 0;   
        StartCoroutine(ReviveToLimbo());
        ToggleControl(false);
    }

    IEnumerator ReviveToLimbo()
    {
        yield return new WaitForSeconds(3f);
        Vector3 offset = new Vector3(
            Random.Range(-1f, 1f), // X offset
            0f,                    // Y offset (keep same height)
            Random.Range(-1f, 1f)  // Z offset
        );
        isInLimbo = true;
        transform.position = Limbo.position + offset;
        GetComponentInChildren<CTAnimPlayer>().PlayAnimation(CTAnimPlayer.CharacterAnimation.Land);
        ToggleControl(true);
    }

    private void ToggleControl(bool IsOn)
    {
        if (IsOn)
        {        
            GetComponentInChildren<ChefPlayer>().enabled = true;
        }
        else
        {
            GetComponentInChildren<ChefPlayer>().enabled = false;
        }
    }

    public void OnCarryCorpse(GameObject carrier)
    {
        float newScale = .25f;
        GetComponent<Renderer>().material = carryMaterial;
        transform.localScale = new Vector3(newScale, newScale, newScale);
        transform.SetParent(carrier.transform);
        corpseCarried = true;
    }

    public bool IsCorpseCarried()
    {
        return corpseCarried;
    }

    public bool IsValidCorpse()
    {
        return !isCorpseWithinCorpseZone && !isInLimbo;
    }

    public void OnDropCorpse()
    {
        GetComponentInChildren<CTAnimPlayer>().PlayAnimation(CTAnimPlayer.CharacterAnimation.Death);
        corpseCarried = false;
        float newScale = 1;
        GetComponent<Renderer>().material = health > 0 ? aliveMaterial : deadMaterial;
        transform.localScale = new Vector3(newScale, newScale, newScale);
        transform.SetParent(null);
        isCorpseWithinCorpseZone = true;

        if (CareTakerAI.Instance && CareTakerAI.Instance.CorpseZone)
        {
            float DistanceBetweenCorpseZone =
                Vector3.Distance(transform.position, CareTakerAI.Instance.CorpseZone.transform.position);
            Debug.Log("Distance is: " + DistanceBetweenCorpseZone);
            isCorpseWithinCorpseZone = DistanceBetweenCorpseZone < 3.0f;
        }
    }
}
