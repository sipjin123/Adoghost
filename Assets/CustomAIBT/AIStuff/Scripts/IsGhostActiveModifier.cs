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

            CareTakerAI ai = self.GetComponent<CareTakerAI>();

            if (ai == null)
            {
                Debug.LogWarning("CheckIsGhostTime: CareTakerAI component not found.");
                return Status.Failure;
            }

            if (!ai.IsGhostTime)
            {
                Debug.Log("CheckIsGhostTime: IsGhostTime is false.");
                return Status.Failure;
            }

            if (PrintLogs)
            Debug.Log("CheckIsGhostTime: IsGhostTime is true.");
            return Status.Success;
        }
    }
}