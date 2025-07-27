using System;
using Unity.Behavior;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveAiLoc", story: "MoveTheAITo: Self: [NAgent] Targ:[Target] [ToWhere] [PrintLogs] [StopThreshold]", category: "Action", id: "3e186534b0de9943e24c778e104a8650")]
public partial class MoveAiLocAction : Action
{
    private NavMeshAgent agent;

    [SerializeReference] public BlackboardVariable<GameObject> NAgent;
    [SerializeReference] public BlackboardVariable<Transform> Target;

    [SerializeReference] public BlackboardVariable<bool> PrintLogs = new BlackboardVariable<bool>(true);
    [SerializeReference] public BlackboardVariable<float> StopThreshold = new BlackboardVariable<float>(1.0f);
    [SerializeReference] public BlackboardVariable<string> ToWhere;

    protected override Status OnStart()
    {
        agent = NAgent?.Value?.GetComponent<NavMeshAgent>();

        if (agent != null && Target?.Value != null)
        {
            if (Vector3.Distance(agent.transform.position, Target.Value.position) <= StopThreshold)
            {
                return Status.Failure;
            }

            agent.isStopped = false;
            agent.SetDestination(Target.Value.position);
            if (PrintLogs)
            Debug.Log("MoveAiLocAction: Moving to Target: [" + ToWhere + "]");
            return Status.Running;
        }

        Debug.LogWarning("MoveAiLocAction: Missing agent or target");
        return Status.Failure;
    }

    protected override Status OnUpdate()
    {
        // Abort if state becomes Retreating
        if (GhostManager.Instance != null && GhostManager.Instance.CaretakerBehaviorState == AIBehaviorState.Retreating)
        {
            if (agent != null)
                agent.isStopped = true;

            if (PrintLogs)
            Debug.Log("MoveAiLocAction: Aborted due to Retreating");
            return Status.Failure;
        }

        if (agent == null || !agent.hasPath || agent.pathPending)
        {
            //Debug.Log("MoveAiLocAction: Now Moving");
            if (agent.pathPending)
            {            
                return Status.Running;
            }
            else
            {
                Debug.Log("MoveAiLocAction: Failed to move: " + ToWhere);
                return Status.Failure;
            }
        }

        // Check if agent reached destination
        if (agent.remainingDistance <= StopThreshold && !agent.pathPending)
        {
            if (PrintLogs)
            Debug.Log("MoveAiLocAction: Finished!");
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (PrintLogs)
        Debug.Log("MoveAiLocAction: EndMove: " + ToWhere);
        // Optional: stop agent when task ends (cleanup)
        if (agent != null)
            agent.isStopped = true;
    }
}