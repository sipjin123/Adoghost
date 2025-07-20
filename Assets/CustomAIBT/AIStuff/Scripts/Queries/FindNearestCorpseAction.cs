using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindNearestCorpse", story: "FindNearestCorpse [Self] [Target] [TagValue]", category: "Action", id: "26942e9800755a40b6a213d0c665022a")]
public partial class FindNearestCorpseAction : Action
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

        GameObject nearest = null;
        float minDistance = float.MaxValue;

        foreach (var obj in taggedObjects)
        {
            if (obj == null || obj == self) continue;

            ICanBeKilled killable = obj.GetComponent<ICanBeKilled>();
            if (killable == null)
                continue;
            
            if (killable.CanBeKilled())
                continue;
            
            if (!killable.IsValidCorpse())
                continue;
            
            float distance = Vector3.SqrMagnitude(obj.transform.position - self.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = obj;
            }
        }

        if (nearest == null)
            return Status.Failure;

        Target.Value = nearest;
        return Status.Success;
    }
}

