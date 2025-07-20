using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace Unity.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "IfCorpseNearby",
        story: "IfCorpseNearby: [Self] [Radius] [PlayerTag] [TargetRef] [HeightTolerance] [PrintLogs]", category: "Action",
        id: "15c1d53a471e27e0fbe561d0a603f5e7")]
    public partial class IfCorpseNearbyAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        [SerializeReference] public BlackboardVariable<GameObject> TargetRef;
        [SerializeReference] public BlackboardVariable<string> PlayerTag;
        [SerializeReference] public BlackboardVariable<float> Radius;
        [SerializeReference] public BlackboardVariable<float> HeightTolerance;
        [SerializeReference] public BlackboardVariable<bool> PrintLogs = new BlackboardVariable<bool>(false);

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
                    if (PrintLogs)
                    Debug.Log($"Rejected {hit.name} due to Y difference: {yDiff}");
                    continue;
                }

                ICanBeKilled killable = hit.GetComponent<ICanBeKilled>();
                if (killable == null || killable.CanBeKilled())
                    continue;

                TargetRef.Value = hit.gameObject;
                if (PrintLogs)
                Debug.LogError("Has Found Corpse Nearby: " + hit.gameObject.name);
                return Status.Success;
            }

            return Status.Failure;
        }
    }
}