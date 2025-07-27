using System;
using System.Collections;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public static GhostManager Instance { get; private set; }

    public bool IsGhostTime = false;

    public event Action<bool> OnGhostTimeChanged; // passes the new value (true/false)
    public event Action OnGhostTimeStarted;       // only fired when IsGhostTime becomes true

    public GhostAI GhostAI;
    public CareTakerAI CareTakerAI;

    public AIBehaviorState CaretakerBehaviorState;
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

    // Optional: toggle ghost time
    public void SetGhostTime(bool active)
    {
        if (active)
        {            
            IsGhostTime = true;
            CareTakerAI.GetComponent<AggroController>().ShouldAbort = true;
            StartCoroutine(ReviveToLimbo());
        }
        else
        {
            IsGhostTime = false;
            OnGhostTimeChanged?.Invoke(false);
            GhostAI.gameObject.SetActive(false);
            GhostAI.transform.position = GhostAI.HidingSpot.position;
        }
    }
    
    IEnumerator ReviveToLimbo()
    {
        GhostAI.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        
        GhostAI.transform.position = GhostAI.SpawnSpot.position;
        OnGhostTimeChanged?.Invoke(IsGhostTime);
        GhostAI.ReleaseTarget();
        GhostAI.gameObject.SetActive(true);
    }
    
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(500, 10, 200, 60));
        GUILayout.BeginVertical("box");

        string label = IsGhostTime ? "Disable Ghost Time" : "Enable Ghost Time";
        if (GUILayout.Button(label))
        {
            SetGhostTime(!IsGhostTime);
            Debug.Log("Ghost Time is now: " + IsGhostTime);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
