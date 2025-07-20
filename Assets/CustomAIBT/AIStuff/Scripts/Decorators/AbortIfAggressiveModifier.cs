using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

namespace Unity.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "AbortIfAggressive", story: "AbortIfAggressive [Self]", category: "Flow",
        id: "eb3542480fce1299d5794d1fc257fc9d")]
    public partial class AbortIfAggressiveModifier : Composite
    {
        [CreateProperty] private int m_CurrentChild;
        [NonSerialized] private bool m_Aborted;
        [SerializeReference] public BlackboardVariable<GameObject> Self;

        private AggroController aggroManager;

        protected override Status OnStart()
        {
            m_CurrentChild = 0;

            // Get AggroController from the same GameObject the BT is attached to
            if (aggroManager == null)
            {
                aggroManager = Self?.Value.GetComponent<AggroController>();
                if (aggroManager == null)
                {
                    Debug.LogWarning("AggroController not found on BT owner GameObject.");
                }
            }

            m_Aborted = ShouldAbort();
            return m_Aborted ? Status.Failure : StartChild(m_CurrentChild);
        }

        protected override Status OnUpdate()
        {
            if (ShouldAbort())
            {
                Debug.Log("ABORTABORTABORT");
                m_Aborted = true;
                return Status.Failure;
            }

            var currentChild = Children[m_CurrentChild];
            var childStatus = currentChild.CurrentStatus;

            if (childStatus == Status.Failure)
            {
                m_CurrentChild++;
                return m_CurrentChild >= Children.Count ? Status.Failure : StartChild(m_CurrentChild);
            }

            return childStatus == Status.Running ? Status.Waiting : childStatus;
        }

        private bool ShouldAbort()
        {
            return aggroManager != null && aggroManager.ShouldAbort;
        }

        protected void OnReset()
        {
            m_Aborted = false;
            m_CurrentChild = 0;
        }

        private Status StartChild(int childIndex)
        {
            if (childIndex >= Children.Count)
                return Status.Failure;

            var childStatus = StartNode(Children[childIndex]);
            return childStatus switch
            {
                Status.Failure => (childIndex + 1 >= Children.Count) ? Status.Failure : Status.Running,
                Status.Running => Status.Waiting,
                _ => childStatus
            };
        }
    }
}