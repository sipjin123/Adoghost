using System;
using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform SpawnSpot;
    public Transform HidingSpot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private AggroController aggroManager;
    public bool HasDetectedTarget;
    public GameObject TargetPlayer;
    public VisionDetectorComp VisionDetectorComp;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (GhostManager.Instance != null)
        {
            GhostManager.Instance.OnGhostTimeChanged += OnGhostTimeChanged;
            VisionDetectorComp.OnGameObjectBroadcast += OnTargetSeen;
        }
    }

    void Update()
    {
        if (!HasDetectedTarget && TargetPlayer == null && GhostManager.Instance.IsGhostTime)
        {
            VisionDetectorComp.ScanForTargets();
        }
    }
    
    void OnTargetSeen(GameObject target)
    {
        Debug.Log("Ghost Target seen: " + target.name);
        HasDetectedTarget = true;
        TargetPlayer = target;
        aggroManager.ShouldAbort = true;
    }

    public void ReleaseTarget()
    {
        HasDetectedTarget = false;
        TargetPlayer = null;
        aggroManager.ShouldAbort = true;
    }

    private void OnGhostTimeChanged(bool isActive)
    {
        if (isActive)
        {
            agent.Warp(SpawnSpot.position);
            Debug.Log("Ghost Time Started (Changed)");
        }
        else
        {
            Debug.Log("Ghost Time Ended");
        }
    }
}
