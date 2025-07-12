using System;
using System.Collections.Generic;
using CustomAIBT.Utilities;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "KillPlayer", story: "[Self] KillPlayer: Dead [PlayerList] ATKRange: [DetectionRadius] [HeightTolerance]", category: "Action", id: "f9992355888ed3e1aca85554e107cc01")]
public partial class KillPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<List<GameObject>> PlayerList; // Active players
    [SerializeReference] public BlackboardVariable<float> DetectionRadius;
    [SerializeReference] public BlackboardVariable<float> HeightTolerance;
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
            if (candidate == Self.Value || PlayerList.Value.Contains(candidate))
                continue;

            ICanBeKilled killable = candidate.GetComponent<ICanBeKilled>();
            if (killable == null || !killable.CanBeKilled())
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
            /*
            // Instantly look at target
            Vector3 directionToTarget = (closestTarget.transform.position - Self.Value.transform.position).normalized;
            directionToTarget.y = 0; // Lock vertical rotation if needed
            if (directionToTarget != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                Self.Value.transform.rotation = lookRotation;
            }*/
            CustTransformUtils.FaceTarget(Self.Value.transform, closestTarget.transform);
            
            Debug.LogError("Kill You: " + closestTarget.name);
            closestTarget.GetComponent<ICanBeKilled>()?.OnKilled();
            Self.Value.GetComponent<CTAnimPlayer>().PlayAnimation(CTAnimPlayer.CharacterAnimation.Stab);
            PlayerList.Value.Add(closestTarget);
            return Status.Success;
        }

        return Status.Failure;
    }
}

