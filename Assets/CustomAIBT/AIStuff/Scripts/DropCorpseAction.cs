using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DropCorpse", story: "DropCorpse [Self] [Target] [HasCorpse] [TagValue]", category: "Action", id: "dbc0e4e9f252462ed219c4e3ed74acfb")]
public partial class DropCorpseAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<bool> HasCorpse;
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
            
            float distance = Vector3.SqrMagnitude(obj.transform.position - self.transform.position);
            if (distance < minDistance && killable.IsCorpseCarried())
            {
                minDistance = distance;
                nearest = obj;
            }
        }

        if (nearest == null)
            return Status.Failure;

        Target.Value = nearest;
        nearest.GetComponent<ICanBeKilled>().OnDropCorpse();
        HasCorpse.Value = false;
        return Status.Success;
    }
}

