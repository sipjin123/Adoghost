using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace Unity.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "RegisterDetectedTarget", story: "RegisterDetectedTarget [Self] [PrintLogs] [Target] [IsGhost]", category: "Action",
        id: "576948b28dfa2f72bf019694540bd505")]
    public partial class RegisterDetectedTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        [SerializeReference] public BlackboardVariable<GameObject> Target;
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

                Target.Value = Ghost.TargetPlayer;
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

                Target.Value = CareTaker.TargetPlayer;
            }

            return Status.Success;
        }
    }
}