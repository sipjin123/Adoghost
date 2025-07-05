using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ConditionalStateAbortComposite ", story: "ConditionalStateAbort: [State] [RequiredState] [DetectionRadius]", category: "Flow", id: "989c7c33404f69fe6f4cc25c8632fe7b")]
public partial class ConditionalStateAbortCompositeModifier : Composite
{

    [CreateProperty] private int m_CurrentChild;
    [NonSerialized] private bool m_Aborted;

    [SerializeReference]
    public BlackboardVariable<AIBehaviorState> State;

    [SerializeReference] public BlackboardVariable<int> RequiredState;

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
        return State == null || State.Value != (AIBehaviorState)RequiredState?.Value;
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