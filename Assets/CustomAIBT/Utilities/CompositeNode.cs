using System.Collections.Generic;

public abstract class CompositeNode : BTNode
{
    protected List<BTNode> children = new List<BTNode>();

    public CompositeNode(List<BTNode> children)
    {
        this.children = children;
    }
}