using System;
using Unity.Behavior;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveAiLoc", story: "MoveTheAITo: State: [State] Self: [NAgent] Targ:[Target]", category: "Action", id: "3e186534b0de9943e24c778e104a8650")]
public partial class MoveAiLocAction : Action
{
    private NavMeshAgent agent;

    [SerializeReference] public BlackboardVariable<GameObject> NAgent;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<AIBehaviorState> State;

    private const float StopThreshold = 1.0f;

    protected override Status OnStart()
    {
        agent = NAgent?.Value?.GetComponent<NavMeshAgent>();

        if (agent != null && Target?.Value != null)
        {
            agent.isStopped = false;
            agent.SetDestination(Target.Value.position);
            Debug.Log("MoveAiLocAction: Moving to Target");
            return Status.Running;
        }

        Debug.LogWarning("MoveAiLocAction: Missing agent or target");
        return Status.Failure;
    }

    protected override Status OnUpdate()
    {
        // Abort if state becomes Retreating
        if (State != null && State.Value == AIBehaviorState.Retreating)
        {
            if (agent != null)
                agent.isStopped = true;

            Debug.Log("MoveAiLocAction: Aborted due to Retreating");
            return Status.Failure;
        }

        if (agent == null || !agent.hasPath || agent.pathPending)
            return Status.Running;

        // Check if agent reached destination
        if (agent.remainingDistance <= StopThreshold && !agent.pathPending)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        // Optional: stop agent when task ends (cleanup)
        if (agent != null)
            agent.isStopped = true;
    }
}