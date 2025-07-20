using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AreThereAnyCorpse", story: "AreThereAnyCorpse [Self] [Target] [TagValue]", category: "Flow",
    id: "761206b501eb71cd528820355d7cd0df")]
public partial class AreThereAnyCorpseModifier : Action
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

        foreach (var obj in taggedObjects)
        {
            if (obj == null || obj == self) continue;

            ICanBeKilled killable = obj.GetComponent<ICanBeKilled>();
            if (killable == null)
                continue;

            if (killable.CanBeKilled())
                continue;

            if (killable.IsValidCorpse())
            {
                return Status.Success;
            }
        }

        return Status.Failure;
    }
}
