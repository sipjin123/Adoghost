using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsCurrTargetNearby", story: "IsCurrTargetNearby [Self] [Target] [Radius]", category: "Action", id: "46415e95eba7482ecb543b2bced55d26")]
public partial class IsCurrTargetNearbyAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Radius;
    
    protected override Status OnStart()
    {
        var selfRef = Self?.Value;
        var targetRef = Target?.Value;
        float radius = Radius?.Value ?? 0f;

        if (selfRef == null || targetRef == null || radius <= 0f)
            return Status.Failure;

        Vector3 selfPos = selfRef.transform.position;
        Vector3 targetPos = targetRef.transform.position;

        float distance = Vector3.Distance(selfPos, targetPos);
        float yTolerance = 2f; // Optional: Y-axis constraint

        if (Mathf.Abs(targetPos.y - selfPos.y) > yTolerance)
        {
            Debug.Log($"Rejected target due to Y difference: {Mathf.Abs(targetPos.y - selfPos.y)}");
            return Status.Failure;
        }

        Debug.Log($"Distance to Target: {distance}");

        if (distance <= radius)
        {
            Debug.Log($"Target is within range: {distance} <= {radius}");
            return Status.Success;
        }

        return Status.Failure;
    }
}

