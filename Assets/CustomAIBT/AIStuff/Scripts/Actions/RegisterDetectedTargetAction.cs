using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace Unity.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "RegisterDetectedTarget", story: "RegisterDetectedTarget [Self] [PrintLogs] [Target]", category: "Action",
        id: "576948b28dfa2f72bf019694540bd505")]
    public partial class RegisterDetectedTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        [SerializeReference] public BlackboardVariable<GameObject> Target;
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

            Target.Value = CareTaker.TargetPlayer;
            return Status.Success;
        }
    }
}