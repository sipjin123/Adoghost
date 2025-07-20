using System;
using Unity.Behavior;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetHighestPrioTarget", story: "[Self] ObjectiveTag [TargetTag] Radius [Radius] PlayerTag [PlayerTag] Target [Target]", category: "Action", id: "afa987ee0b93bb1ace9360222af816de")]
public partial class SetHighestPrioTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<string> TargetTag;
    [SerializeReference] public BlackboardVariable<string> PlayerTag;
    [SerializeReference] public BlackboardVariable<float> Radius;
    public GameObject NearbyPlayer;
    protected override Status OnStart()
    {
        GameObject SelfRef = Self?.Value;
        Debug.Log("YES");
        return Status.Running;
    }
    
    protected override Status OnUpdate()
    {
        if (string.IsNullOrEmpty(TargetTag) || string.IsNullOrEmpty(PlayerTag))
            return Status.Failure;

        GameObject[] targets = GameObject.FindGameObjectsWithTag(TargetTag);
        GameObject[] players = GameObject.FindGameObjectsWithTag(PlayerTag);

        if (targets == null || targets.Length == 0 || players == null || players.Length == 0)
            return Status.Failure;

        GameObject bestTarget = null;
        int bestPriority = int.MinValue;
        float radiusSqr = Radius.Value * Radius.Value;
        GameObject cachedPlayer = null;
        foreach (GameObject t in targets)
        {
            if (t == null) continue;

            // Check if any player is near this target
            bool hasNearbyPlayer = false;
            foreach (GameObject p in players)
            {
                if (p == null) continue;
                float distSqr = (p.transform.position - t.transform.position).sqrMagnitude;
                if (distSqr <= radiusSqr)
                {
                    hasNearbyPlayer = true;
                    cachedPlayer = p;
                    break;
                }
            }

            if (!hasNearbyPlayer) continue;

            // Get the priority from a script or component
            var priorityComponent = t.GetComponent<ObjectivePriority>();
            if (priorityComponent == null) continue;

            if (priorityComponent.Priority > bestPriority)
            {
                bestPriority = priorityComponent.Priority;
                bestTarget = t;
                NearbyPlayer = cachedPlayer;
            }
        }

        if (bestTarget == null)
            return Status.Failure;

        Target.Value = bestTarget;
        
        DebugDraw.Sphere(bestTarget.transform.position, Radius, Color.cyan, 3);
        DebugDraw.Sphere(NearbyPlayer.transform.position, 2, Color.red, 3);
        
        Debug.Log("FoundTarget");
        return Status.Success;
    }
}

