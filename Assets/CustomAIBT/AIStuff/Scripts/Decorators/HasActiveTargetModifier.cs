using System;
using Unity.Behavior;
using UnityEngine;
using Modifier = Unity.Behavior.Modifier;
using Unity.Properties;

namespace Unity.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "HasActiveTarget", story: "IsGhostActive: [Self] [PrintLogs] [IsGhost]", category: "Flow", id: "4d088ce66ef02d91ec26ecf4e9a5d68b")]
    public partial class HasActiveTargetModifier : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        [SerializeReference] public BlackboardVariable<bool> PrintLogs = new BlackboardVariable<bool>(false);
        [SerializeReference] public BlackboardVariable<bool> IsGhost;
        protected override Status OnUpdate()
        {
            GameObject self = Self?.Value;

            if (self == null)
            {
                return Status.Failure;
            }

            if (IsGhost)
            {
                var Ghost = self.GetComponent<GhostAI>();
                if (Ghost == null)
                {
                    return Status.Failure;
                }
            
                if (Ghost.TargetPlayer == null || !Ghost.HasDetectedTarget)
                {
                    return Status.Failure;
                }
            }
            else
            {

                var CareTaker = self.GetComponent<CareTakerAI>();
                if (CareTaker == null)
                {
                    return Status.Failure;
                }
            
                if (CareTaker.TargetPlayer == null || !CareTaker.HasDetectedTarget)
                {
                    return Status.Failure;
                }
            }

            return Status.Success;
        }
    }
}