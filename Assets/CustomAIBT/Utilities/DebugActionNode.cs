public class DebugActionNode : BTNode
{
    private string message;

    public DebugActionNode(string msg)
    {
        message = msg;
    }

    public override NodeState Evaluate()
    {
        UnityEngine.Debug.Log("Running: " + message);
        return NodeState.Success;
    }
}