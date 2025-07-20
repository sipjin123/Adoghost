public abstract class BTNode
{
    public abstract NodeState Evaluate();
}
public enum NodeState
{
    Success,
    Failure,
    Running
}
