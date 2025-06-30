using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RandomChance", story: "RandomizeChance [RandomChance]", category: "Action", id: "d45f0cc863428db9505f812705cce500")]
public partial class RandomChanceAction : Action
{
    [Range(0,10)]
    [SerializeReference] public BlackboardVariable<float> RandomChance;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var NewVal = Random.Range(0, 10);
        if (NewVal < RandomChance)
        {
            return Status.Success;
        }
        return Status.Failure;
    }

    protected override void OnEnd()
    {
    }
}

