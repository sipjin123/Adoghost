using Unity.Behavior;

[BlackboardEnum]
public enum AIBehaviorState
{
    Idle = 0,
	Hunting = 1,
	Retreating = 2,
	Roaming = 3,
	Killing = 4,
	CorpseCleanup = 5,
}
