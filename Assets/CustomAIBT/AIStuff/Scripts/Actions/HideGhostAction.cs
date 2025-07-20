using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "HideGhost", story: "HideGhost", category: "Action [Self] [IsHiding]", id: "be7c6a4349cee3d26e65417e4e8f5804")]
public partial class HideGhostAction: Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<bool> IsHiding;

    protected override Status OnStart()
    {
        // Optional: return Running if you want to animate or delay it
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        GameObject self = Self?.Value;
        Transform target = Self?.Value.GetComponent<GhostAI>().HidingSpot;
        if (self == null || target == null)
        {
            Debug.LogWarning("TeleportGhostToHidingSpot: Missing Self or TargetTransform.");
            return Status.Failure;
        }

        self.transform.position = target.position;
        self.transform.rotation = target.rotation;

        if (IsHiding != null)
        {
            IsHiding.Value = true;
        }

        return Status.Success;
    }

    protected override void OnEnd()
    {
        // Optional cleanup or effect trigger
    }
}