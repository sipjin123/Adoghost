using System;
using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform SpawnSpot;
    public Transform HidingSpot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (GhostManager.Instance != null)
        {
            GhostManager.Instance.OnGhostTimeChanged += OnGhostTimeChanged;
        }
    }

    private void OnGhostTimeChanged(bool isActive)
    {
        return;
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
