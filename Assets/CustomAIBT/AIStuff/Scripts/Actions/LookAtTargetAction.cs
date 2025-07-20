using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LookAtTarget", story: "LookAt [Self] [Target] [RotationSpeed] [AngleThreshold]", category: "Action", id: "ff1ec8e3cd5eb05701afbfee8f701892")]
public partial class LookAtTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> RotationSpeed = new(5f);
    [SerializeReference] public BlackboardVariable<float> AngleThreshold = new(2f);

    protected override Status OnUpdate()
    {
        if (Self?.Value == null || Target?.Value == null)
            return Status.Failure;

        Transform selfTransform = Self.Value.transform;
        Transform targetTransform = Target.Value.transform;

        Vector3 direction = (targetTransform.position - selfTransform.position).normalized;
        direction.y = 0f; // Keep rotation on horizontal plane

        if (direction.sqrMagnitude < 0.001f)
            return Status.Failure;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        selfTransform.rotation = Quaternion.Slerp(
            selfTransform.rotation,
            targetRotation,
            Time.deltaTime * RotationSpeed.Value
        );

        // Check if rotation is close enough
        float angle = Quaternion.Angle(selfTransform.rotation, targetRotation);
        if (angle <= AngleThreshold.Value)
        {
            return Status.Success;
        }

        return Status.Running;
    }
}

