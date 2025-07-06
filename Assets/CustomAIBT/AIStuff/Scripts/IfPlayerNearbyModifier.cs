using System;
using Unity.Behavior;
using UnityEngine;
using Modifier = Unity.Behavior.Modifier;
using Unity.Properties;

namespace Unity.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "IfPlayerNearby", story: "IfPlayerNearby [Self] [Radius] [PlayerTag] [TargetRef] [HeightTolerance]", category: "Flow",
        id: "f5b0bd7366cc38a32e52749ca39509ab")]
    public partial class IfPlayerNearbyModifier : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        [SerializeReference] public BlackboardVariable<GameObject> TargetRef;
        [SerializeReference] public BlackboardVariable<string> PlayerTag;
        [SerializeReference] public BlackboardVariable<float> Radius;
        [SerializeReference] public BlackboardVariable<float> HeightTolerance;

        protected override Status OnStart()
        {
            var selfRef = Self?.Value;
            var tag = PlayerTag?.Value;

            if (selfRef == null || string.IsNullOrEmpty(tag))
                return Status.Failure;

            Vector3 pos = selfRef.transform.position;
            float yTolerance = HeightTolerance; // You can tweak this as needed

            Collider[] hits = Physics.OverlapSphere(pos, Radius.Value);

            foreach (var hit in hits)
            {
                if (!hit.CompareTag(tag))
                    continue;

                float yDiff = Mathf.Abs(hit.transform.position.y - pos.y);
                if (yDiff > yTolerance)
                {
                    Debug.Log($"Rejected {hit.name} due to Y difference: {yDiff}");
                    continue;
                }

                ICanBeKilled killable = hit.GetComponent<ICanBeKilled>();
                if (killable == null || !killable.CanBeKilled())
                    continue;

                TargetRef.Value = hit.gameObject;
                Debug.LogError("Has Found Player Nearby: " + hit.gameObject.name);
                return Status.Success;
            }

            return Status.Failure;
        }
    }
}