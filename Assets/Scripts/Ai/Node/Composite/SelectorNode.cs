using System.Collections.Generic;

namespace Ai.Node.Composite
{
    /// <summary>
    /// 하나라도 성공이라면 즉시 성공 반환
    /// </summary>
    public class SelectorNode : CompositeNodeBase
    {
        private int _currentRunningNodeIndex = -1;

        public SelectorNode(Blackboard blackboard) : base(blackboard, new List<INode>())
        {
        }
        
        public SelectorNode(Blackboard blackboard, List<INode> children) : base(blackboard, children)
        {
        }

        public override INode.State Evaluate()
        {
            if (children.Count == 0)
                return INode.State.Failure;
            
            if ( _currentRunningNodeIndex == -1 )
                _currentRunningNodeIndex = 0;

            for (var index = _currentRunningNodeIndex; index < children.Count; ++index)
            {
                var child = children[index];
                var state = child.Evaluate();
                
                if (state == INode.State.Running)
                    return INode.State.Running;
                if (state == INode.State.Success)
                {
                    ResetSequence();
                    return INode.State.Success;
                }
            }

            // 성공이 하나도 나오지 않았다면, 실패 반환
            ResetSequence();
            return INode.State.Failure;
        }
        
        private void ResetSequence()
        {
            _currentRunningNodeIndex = -1;
        }
    }
}