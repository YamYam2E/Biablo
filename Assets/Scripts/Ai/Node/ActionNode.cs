using System;

namespace Ai.Node
{
    public class ActionNode : INode
    {
        // 결과를 반환값으로 받아야하므로 Action 대신 Func 사용
        private Func<INode.State> _action;

        public ActionNode(Func<INode.State> action)
        {
            _action = action;
        }

        public INode.State Evaluate()
        {
            if (_action != null)
                return _action.Invoke();
            
            return INode.State.Failure;
        }
    }
}