using System.Collections.Generic;

namespace Ai.Node.Composite
{
    public abstract class CompositeNodeBase : INode
    {
        protected Blackboard blackboard;
        protected List<INode> children;

        public CompositeNodeBase(Blackboard blackboard, List<INode> children)
        {
            this.blackboard = blackboard;
            this.children = children;
        }
        
        public void AddChild(INode child)
        {
            children.Add(child);
        }
        
        public void AddChildren(IEnumerable<INode> children)
        {
            this.children.AddRange(children);
        }

        public void RemoveChild(INode child)
        {
            children.Remove(child); 
        }
        
        public abstract INode.State Evaluate();
    }
}