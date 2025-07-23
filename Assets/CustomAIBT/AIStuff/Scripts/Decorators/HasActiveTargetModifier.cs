using System;
using Unity.Behavior;
using UnityEngine;
using Modifier = Unity.Behavior.Modifier;
using Unity.Properties;

namespace Unity.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "HasActiveTarget", story: "IsGhostActive: [Self] [PrintLogs]", category: "Flow", id: "4d088ce66ef02d91ec26ecf4e9a5d68b")]
    public partial class HasActiveTargetModifier : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        [SerializeReference] public BlackboardVariable<bool> PrintLogs = new BlackboardVariable<bool>(false);

        protected override Status OnUpdate()
        {
            GameObject self = Self?.Value;

            if (self == null)
            {
                return Status.Failure;
            }

            var CareTaker = self.GetComponent<CareTakerAI>();
            if (CareTaker == null)
            {
                return Status.Failure;
            }
            
            if (CareTaker.TargetPlayer == null || !CareTaker.HasDetectedTarget)
            {
                return Status.Failure;
            }

            return Status.Success;
        }
    }
}