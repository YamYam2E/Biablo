namespace Ai.Node.Action
{
    public abstract class ActionNodeBase : INode
    {
        protected Blackboard Blackboard;

        protected ActionNodeBase(Blackboard blackboard)
        {
            Blackboard = blackboard;
        }
        
        public abstract INode.State Evaluate();
    }
}