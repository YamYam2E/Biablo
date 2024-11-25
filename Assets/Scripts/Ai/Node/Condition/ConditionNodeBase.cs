namespace Ai.Node.Condition
{
    public abstract class ConditionNodeBase : INode
    {
        protected Blackboard Blackboard;

        protected ConditionNodeBase(Blackboard blackboard)
        {
            Blackboard = blackboard;
        }
        
        public abstract INode.State Evaluate();
    }
}