using UnityEngine;
using Unity.Behavior;

// Bonta
// This class is responsible for resetting the Behavior graph, normally used to abort existing action and process re prioritized branches
public class BTAbortMonitor : MonoBehaviour
{
    private BehaviorGraphAgent agent;
    private AggroController aggro;

    void Awake()
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
            aggro.ShouldAbort = false;
            agent?.Graph.End();
            agent?.Graph.Restart();
        }
    }
}