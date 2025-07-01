using System;
using Unity.Behavior;

[BlackboardEnum]
public enum AIBehaviorState
{
    Idle,
	Hunting,
	Retreating,
	Roaming,
	Killing
}
