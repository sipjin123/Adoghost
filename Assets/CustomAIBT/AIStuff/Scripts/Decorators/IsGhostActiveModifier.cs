using System;
using Unity.Behavior;
using UnityEngine;
using Modifier = Unity.Behavior.Modifier;
using Unity.Properties;
namespace Unity.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "IsGhostActive", story: "IsGhostActive: [Self] [PrintLogs]", category: "Flow",
        id: "6e0ca2c4a5d107db73a87add832359c0")]
    public partial class IsGhostActiveModifier : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        [SerializeReference] public BlackboardVariable<bool> PrintLogs = new BlackboardVariable<bool>(false);

        protected override Status OnUpdate()
        {
            GameObject self = Self?.Value;

            if (self == null)
            {
                Debug.LogWarning("CheckIsGhostTime: Self GameObject is null.");
                return Status.Failure;
            }

            if (GhostManager.Instance == null)
            {
                Debug.LogWarning("CheckIsGhostTime: GhostManager is not available.");
                return Status.Failure;
            }

            if (!GhostManager.Instance.IsGhostTime)
            {
                if (PrintLogs)
                    Debug.Log("CheckIsGhostTime: GhostManager says it's NOT ghost time.");
                return Status.Failure;
            }

            if (PrintLogs)
                Debug.Log("CheckIsGhostTime: GhostManager says it's ghost time.");
    
            return Status.Success;
        }
    }
}