using System;
using UnityEngine;
using System.Collections;
using AdoboHorror.Game.Network;
using UnityEngine.AI;
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
    public Renderer MeshRender;
    public Transform ParentTransformer;
    [SerializeField] private float smoothSpeed = 5f;
    private void Start()
    {
        aliveMaterial = MeshRender.material;
    }

    public bool CanBeKilled()
    {
        return !isInvincible && health > 0;
    }

    public void OnKilled()
    {
        Debug.Log($"{gameObject.name} has been killed!");
        MeshRender.material = deadMaterial;
        GetComponentInChildren<CTAnimPlayer>().PlayAnimation(CTAnimPlayer.CharacterAnimation.Death);
        health = 0;
        ToggleControl(false);
    }

    public void OnKilledAsGhost()
    {
        Debug.Log($"{gameObject.name} has been killed!");
        MeshRender.material = deadMaterial;
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

    private void Update()
    {
        if (ParentTransformer)
        {       transform.position = Vector3.Lerp(
                transform.position,
                ParentTransformer.transform.position,
                Time.deltaTime * smoothSpeed
            );
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            var NewLoc = new Vector3(1f, 1.3f, 10f);
            if (GetComponent<AvatarNetwork>())
            GetComponent<AvatarNetwork>().RequestTeleport(NewLoc);
        }
    }

    private void ToggleControl(bool IsOn)
    {
        ChefPlayer chefRef = GetComponentInChildren<ChefPlayer>();
        if (chefRef)
        {
            if (IsOn)
            {
                chefRef.enabled = true;
            }
            else
            {
                chefRef.enabled = false;
            }
        }
    }

    public void OnCarryCorpse(GameObject carrier)
    {
        float newScale = .25f;
        MeshRender.material = carryMaterial;
        transform.localScale = new Vector3(newScale, newScale, newScale);
        //transform.SetParent(carrier.transform);
        ParentTransformer = carrier.transform;
        corpseCarried = true;
        CapsuleCollider newCol = GetComponent<CapsuleCollider>();
        NavMeshAgent NavAgent = GetComponent<NavMeshAgent>();
        if (newCol)
            newCol.enabled = false;
        if (NavAgent)
            NavAgent.enabled = false;
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
        NavMeshAgent NavAgent = GetComponent<NavMeshAgent>();
        CapsuleCollider newCol = GetComponent<CapsuleCollider>();
        if (NavAgent)
            NavAgent.enabled = true;
        if (newCol)
            newCol.enabled = true;
        
        ParentTransformer = null;
        GetComponentInChildren<CTAnimPlayer>().PlayAnimation(CTAnimPlayer.CharacterAnimation.Death);
        corpseCarried = false;
        float newScale = 1;
        MeshRender.material = health > 0 ? aliveMaterial : deadMaterial;
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
