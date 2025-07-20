using UnityEngine;
using Unity.AI;
using Unity.Behavior;

public class BTAbortMonitor : MonoBehaviour
{
    private BehaviorGraphAgent agent;
    private AggroController aggro;

    void Start()
    {
        agent = GetComponent<BehaviorGraphAgent>();
        aggro = GetComponent<AggroController>();

        if (agent == null)
            Debug.LogError("BehaviorGraphAgent component is missing!");
        if (aggro == null)
            Debug.LogWarning("AggroController not found—abort condition won't work.");
    }

    void Update()
    {
        if (aggro != null && aggro.ShouldAbort)
        {
            Debug.LogError("BehaviorGraphAgentABORTTTTTTTT!");
            aggro.ShouldAbort = false;
            agent?.Graph.End();
            agent?.Graph.Restart();
        }
    }
}