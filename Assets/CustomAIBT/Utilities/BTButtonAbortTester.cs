using UnityEngine;
using System.Collections.Generic;
using Unity.Behavior;

public class BTButtonAbortTester : MonoBehaviour
{
    [SerializeField] private AggroController aggroManager;
    [SerializeField] private KeyCode abortKey = KeyCode.Space; // Press Space to abort

    private void Start()
    {
        if (aggroManager == null)
        {
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(abortKey))
        {
            if (aggroManager != null)
            {
                aggroManager.ShouldAbort = true;
                Debug.Log("Abort triggered via keypress.");
            }
            else
            {
                Debug.LogWarning("AggroManager not found.");
            }
        }
    }
 }
