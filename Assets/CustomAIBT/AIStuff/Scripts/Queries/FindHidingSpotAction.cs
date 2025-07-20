using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindHidingSpot", story: "FindHidingSpot: [Self] [TargetZone]", category: "Action", id: "9b0ee2abd114a6fd2192d9c4a7ccd28f")]
public partial class FindHidingSpotAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> TargetZone;

    protected override Status OnUpdate()
    {
        GameObject self = Self?.Value;
        
        if (self == null || self.GetComponent<CareTakerAI>().HidingSpot == null)
            return Status.Failure;

        TargetZone.Value = self.GetComponent<CareTakerAI>().HidingSpot;
        
        DebugDraw.Sphere( self.GetComponent<CareTakerAI>().HidingSpot.transform.position, 1, Color.red, 3);
        return Status.Success;
    }
}

