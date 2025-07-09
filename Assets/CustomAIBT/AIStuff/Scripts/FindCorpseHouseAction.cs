using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindCorpseHouse", story: "FindCorpseHouse [Self] [TargetZone]", category: "Action", id: "2ad21251a0faf697b818380ef17b9c68")]
public partial class FindCorpseHouseAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> TargetZone;

    protected override Status OnUpdate()
    {
        GameObject self = Self?.Value;
        
        if (self == null || self.GetComponent<CareTakerAI>().CorpseZone == null)
            return Status.Failure;

        TargetZone.Value = self.GetComponent<CareTakerAI>().CorpseZone;
        Debug.Log("Found the House");
        
        DebugDraw.Sphere( self.GetComponent<CareTakerAI>().CorpseZone.transform.position, 1, Color.red, 3);
        return Status.Success;
    }
}

