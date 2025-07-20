using System;
using Unity.Properties;
using UnityEngine;

namespace Unity.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "AbortIfStateIsEscape", story: "AbortIfStateIsEscape [State]", category: "Flow",
        id: "c72930fea3e6d4e6ccaab646eff1c6b2")]
    internal partial class EscapeAbortSelectorComposite : Composite
    {  
        [CreateProperty] private int m_CurrentChild;
        [NonSerialized] private bool m_Aborted;

        [SerializeReference]
        public BlackboardVariable<AIBehaviorState> State;

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
            return State != null && State.Value == AIBehaviorState.Retreating;
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