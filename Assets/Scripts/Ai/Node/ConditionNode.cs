using System;

namespace Ai.Node
{
    public class ConditionNode : INode
    {
        protected Blackboard blackboard;
        private Func<bool> _condition;

        public ConditionNode(Func<bool> condition)
        {
            _condition = condition;
        }
        
        public INode.State Evaluate()
        {
            return _condition.Invoke() ? INode.State.Success : INode.State.Failure;
        }
    }
}