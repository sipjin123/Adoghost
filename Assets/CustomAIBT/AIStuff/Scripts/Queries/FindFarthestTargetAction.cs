using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindFarthestTarget", story: "FindFarthestTarget [TagValue]", category: "Action", id: "511d48349278e05551b11107b0136203")]
public partial class FindFarthestTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<string> TagValue;

    protected override Status OnUpdate()
    {
        GameObject self = Self?.Value;
        if (self == null || string.IsNullOrEmpty(TagValue))
            return Status.Failure;

        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(TagValue);
        if (taggedObjects == null || taggedObjects.Length == 0)
            return Status.Failure;

        GameObject farthest = null;
        float maxDistance = float.MinValue;

        foreach (var obj in taggedObjects)
        {
            if (obj == null || obj == self) continue;

            float distance = Vector3.SqrMagnitude(obj.transform.position - self.transform.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthest = obj;
            }
        }

        if (farthest == null)
            return Status.Failure;

        Target.Value = farthest;
        return Status.Success;
    }
}