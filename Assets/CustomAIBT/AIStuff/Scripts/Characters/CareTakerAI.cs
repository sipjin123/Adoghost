using System;
using Unity.Behavior;
using UnityEngine;
using System.Collections.Generic;
using Action = Unity.Behavior.Action;
using Unity.Properties;

public class CareTakerAI : MonoBehaviour
{    
    
    [SerializeField]
    public AIBehaviorState RequiredState;
    [SerializeField] private AggroController aggroManager;
    public GameObject CorpseZone;
    [SerializeField]
    public GameObject HidingSpot;

    public bool IsGhostTime = false;
    
    public bool HasDetectedTarget;
    public GameObject TargetPlayer;
    public GameObject CarriedPlayer;
    public VisionDetectorComp VisionDetectorComp;
    public static CareTakerAI Instance { get; private set; }

    private void Awake()
    {
        // Enforce singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        VisionDetectorComp.OnGameObjectBroadcast += OnTargetSeen;
    }

    void Update()
    {
        if (!HasDetectedTarget && TargetPlayer == null && !GhostManager.Instance.IsGhostTime)
        {
            VisionDetectorComp.ScanForTargets();
        }
    }

    void OnTargetSeen(GameObject target)
    {
        Debug.Log("Target seen: " + target.name);
        HasDetectedTarget = true;
        TargetPlayer = target;
        aggroManager.ShouldAbort = true;
        ForceDropCorpse();
        // Call your custom logic here
    }
    
    public void ReleaseTarget()
    {
        HasDetectedTarget = false;
        TargetPlayer = null;
        aggroManager.ShouldAbort = true;
    }

    public void ForceDropCorpse()
    {
        if (CarriedPlayer)
        {
            CarriedPlayer.GetComponent<ICanBeKilled>().OnDropCorpse();
        }
    }
}
