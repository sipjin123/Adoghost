using System;
using Unity.Behavior;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveAiLoc", story: "MoveTheAI", category: "Action", id: "3e186534b0de9943e24c778e104a8650")]
public partial class MoveAiLocAction : Action
{

    private NavMeshAgent agent;
    [SerializeReference] public BlackboardVariable<GameObject> NAgent;

    [SerializeReference] public BlackboardVariable<Transform> Target;
    protected override Status OnStart()
    {
        agent = NAgent?.Value.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (Target?.Value != null && agent)
        {
            Debug.Log("Move to Target");
            agent.SetDestination(Target.Value.position);
        }
        else
        {
            Debug.Log("Cant Move to Target");
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

