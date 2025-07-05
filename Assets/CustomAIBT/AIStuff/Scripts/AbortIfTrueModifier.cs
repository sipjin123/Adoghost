using System;
using Unity.Properties;
using UnityEngine;

namespace Unity.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "AbortIfTrue", story: "AbortIfTrue: [ShouldAbortFlag]", category: "Flow", id: "b96379c2f4d4298e5ea99304c60a1194")]
    public partial class AbortIfTrueModifier : Composite
    {
        [CreateProperty] private int m_CurrentChild;
        [NonSerialized] private bool m_Aborted;
        [SerializeReference]
        public BlackboardVariable<bool> ShouldAbortFlag; // New bool flag

        protected override Status OnStart()
        {
            m_CurrentChild = 0;
            m_Aborted = ShouldAbort();
            return m_Aborted ? Status.Failure : StartChild(m_CurrentChild);
        }

        protected override Status OnUpdate()
        {
            if (ShouldAbort())
            {
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
            // Abort if AI state is Retreating OR the abort flag is set to true
            return (ShouldAbortFlag != null && ShouldAbortFlag.Value);
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
