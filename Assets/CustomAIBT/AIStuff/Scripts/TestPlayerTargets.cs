using System;
using UnityEngine;

public class TestPlayerTargets : MonoBehaviour, ICanBeKilled
{
    public bool isInvincible = false;
    public int health = 100;

    public Material deadMaterial;
    public Material carryMaterial;
    public Material aliveMaterial;
    public bool corpseCarried;
    public bool isCorpseWithinCorpseZone;
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
        return !isCorpseWithinCorpseZone;
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
    }
}
