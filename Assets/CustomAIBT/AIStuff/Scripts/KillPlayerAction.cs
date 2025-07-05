using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "KillPlayer", story: "[Self] KillPlayer: [PlayerList] ATKRange: [DetectionRadius]", category: "Action", id: "f9992355888ed3e1aca85554e107cc01")]
public partial class KillPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<List<GameObject>> PlayerList; // Active players
    [SerializeReference] public BlackboardVariable<float> DetectionRadius;

    protected override Status OnUpdate()
    {
        if (Self?.Value == null || PlayerList?.Value == null || DetectionRadius == null)
            return Status.Failure;

        Vector3 origin = Self.Value.transform.position;

        Collider[] hits = Physics.OverlapSphere(origin, DetectionRadius, ~0);
        bool found = false;

        DebugDraw.Sphere(origin, DetectionRadius, Color.red, 3);
        
        Debug.LogError("Total Overlap: " + hits.Length);
        foreach (var hit in hits)
        {
            ICanBeKilled killable = hit.GetComponent<ICanBeKilled>();
            if (killable == null)
                continue;
            
            GameObject candidate = hit.gameObject;
            if (candidate == Self.Value || PlayerList.Value.Contains(candidate) || !killable.CanBeKilled())
                continue;

            Debug.LogError("Kill You: " + hit.gameObject.name);
            killable.OnKilled();
            
            // Mark as dead by removing from active list
            PlayerList.Value.Add(candidate);

            found = true;
            break; // only mark one per update, or remove this line for multi-detection
        }

        return found ? Status.Success : Status.Failure;
    }
}

