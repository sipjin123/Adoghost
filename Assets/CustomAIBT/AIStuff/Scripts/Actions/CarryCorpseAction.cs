using System;
using System.Collections.Generic;
using CustomAIBT.Utilities;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CarryCorpse", story: "CarryCorpse [Self] [PlayerList] [DetectionRadius] [HeightTolerance] [HasCorpse]", category: "Action", id: "39d7db1d59ec1a94b12b9259c247d866")]
public partial class CarryCorpseAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<List<GameObject>> PlayerList; // Active players
    [SerializeReference] public BlackboardVariable<float> DetectionRadius;
    [SerializeReference] public BlackboardVariable<float> HeightTolerance;
    [SerializeReference] public BlackboardVariable<bool> HasCorpse;
    protected override Status OnUpdate()
    {
        if (Self?.Value == null || PlayerList?.Value == null || DetectionRadius == null)
            return Status.Failure;

        Vector3 origin = Self.Value.transform.position;
        float yTolerance = HeightTolerance; // Adjust this value as needed

        Collider[] hits = Physics.OverlapSphere(origin, DetectionRadius, ~0);
        GameObject closestTarget = null;
        float closestDistanceSqr = float.MaxValue;

        DebugDraw.Sphere(origin, DetectionRadius, Color.red, 3);
        Debug.LogError("Total Overlap: " + hits.Length);

        foreach (var hit in hits)
        {
            GameObject candidate = hit.gameObject;
            //|| PlayerList.Value.Contains(candidate)
            if (candidate == Self.Value)
                continue;

            ICanBeKilled killable = candidate.GetComponent<ICanBeKilled>();
            if (killable == null || killable.CanBeKilled())
                continue;

            // Y-axis check (prevent detecting through floors)
            float yDiff = Mathf.Abs(candidate.transform.position.y - origin.y);
            if (yDiff > yTolerance)
            {
                Debug.Log($"Rejected {candidate.name} due to Y difference: {yDiff}");
                continue;
            }

            float distSqr = (candidate.transform.position - origin).sqrMagnitude;
            if (distSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distSqr;
                closestTarget = candidate;
            }
        }

        if (closestTarget != null)
        {
            CustTransformUtils.FaceTarget(Self.Value.transform, closestTarget.transform);
            
            Debug.LogError("Carry Corpse: " + closestTarget.name);
            closestTarget.GetComponent<ICanBeKilled>()?.OnCarryCorpse(Self);
            Self.Value.GetComponent<CTAnimPlayer>().PlayAnimation(CTAnimPlayer.CharacterAnimation.Interact);
            HasCorpse.Value = true;
            //PlayerList.Value.Add(closestTarget);
            Self.Value.GetComponent<CareTakerAI>().CarriedPlayer = closestTarget;
            return Status.Success;
        }

        return Status.Failure;
    }
}

