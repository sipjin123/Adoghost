using Unity.Behavior;
using UnityEngine;
using System.Collections.Generic;
using Action = Unity.Behavior.Action;
using Unity.Properties;

public class CareTakerAI : MonoBehaviour
{    
    
    [SerializeField]
    public AIBehaviorState RequiredState;

    [SerializeField]
    public GameObject CorpseZone;
    [SerializeField]
    public GameObject HidingSpot;

    public bool IsGhostTime = false;
}
