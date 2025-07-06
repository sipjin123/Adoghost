using System;
using Unity.Behavior;
using UnityEngine;
using Modifier = Unity.Behavior.Modifier;
using Unity.Properties;

namespace Unity.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "IfPlayerNearby", story: "IfPlayerNearby [Self] [Radius] [PlayerTag]", category: "Flow",
        id: "f5b0bd7366cc38a32e52749ca39509ab")]
    public partial class IfPlayerNearbyModifier : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        [SerializeReference] public BlackboardVariable<string> PlayerTag;
        [SerializeReference] public BlackboardVariable<float> Radius;

        protected override Status OnStart()
        {
            var selfRef = Self?.Value;
            var tag = PlayerTag?.Value;

            if (selfRef == null || string.IsNullOrEmpty(tag))
                return Status.Failure;

            Vector3 pos = selfRef.transform.position;
            Collider[] hits = Physics.OverlapSphere(pos, Radius.Value);

            foreach (var hit in hits)
            {
                if (hit.CompareTag(tag))
                {
                    ICanBeKilled killable = hit.GetComponent<ICanBeKilled>();
                    if (killable == null)
                        continue;
                    if (!killable.CanBeKilled())
                        continue;
                    
                    Debug.LogError("Has Found Player Nearby: " + hit.gameObject.name);
                    return Status.Success;
                }
            }

            return Status.Failure;
        }
    }
}