using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindNextRandWaypoint", story: "FindNextRandWaypoint: [TagValue] Self: [Self] TargetWP: [Target]", 
    category: "Action", id: "e487f7e43f14ab11c817ea121430e3a9")]
public partial class FindNextRandWaypointAction : Action
{   
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<string> TagValue;

    // Internal pool of available targets
    [NonSerialized] private List<GameObject> remainingTargets = new();
    [NonSerialized] private List<GameObject> allTargets = new();
    protected override Status OnUpdate()
    {
        GameObject self = Self?.Value;
        if (self == null || string.IsNullOrEmpty(TagValue?.Value))
            return Status.Failure;

        // Refresh allTargets only if it's never been initialized or the scene changed significantly
        if (allTargets.Count == 0)
        {
            GameObject[] found = GameObject.FindGameObjectsWithTag(TagValue.Value);
            foreach (var obj in found)
            {
                if (obj != null && obj != self)
                    allTargets.Add(obj);
            }

            if (allTargets.Count == 0)
                return Status.Failure;
        }

        // Refill remainingTargets when exhausted
        if (remainingTargets.Count == 0)
        {
            remainingTargets = new List<GameObject>(allTargets);
        }

        // Randomly select and remove a target
        int index = UnityEngine.Random.Range(0, remainingTargets.Count);
        GameObject selected = remainingTargets[index];
        remainingTargets.RemoveAt(index);

        Target.Value = selected;
        Debug.LogError("New target is: " + Target.Value.gameObject.name);
        return Status.Success;
    }
}