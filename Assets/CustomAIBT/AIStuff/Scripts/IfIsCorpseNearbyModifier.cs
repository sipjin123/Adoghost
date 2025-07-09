using System;
using Unity.Behavior;
using UnityEngine;
using Modifier = Unity.Behavior.Modifier;
using Unity.Properties;

namespace Unity.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "IfIsCorpseNearby", story: "IfIsCorpseNearby: [Self] [Radius] [PlayerTag] [TargetRef] [HeightTolerance]", category: "Flow",
        id: "ab65ffb0298a80349dc25d9c5cb0477f")]
    public partial class IfIsCorpseNearbyModifier : Action
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
                if (killable == null || killable.CanBeKilled() || killable.IsCorpseCarried() || !killable.IsValidCorpse())
                    continue;

                TargetRef.Value = hit.gameObject;
                Debug.LogError("Has Found Corpse Nearby: " + hit.gameObject.name);
                return Status.Success;
            }

            return Status.Failure;
        }
    }
}