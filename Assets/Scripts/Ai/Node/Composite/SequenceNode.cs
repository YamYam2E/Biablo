using System.Collections.Generic;
using Ai.Node.Composite;

namespace Ai.Node
{
    /// <summary>
    /// 모든 자식 노드가 실패하지 않아야 성공 반환
    /// </summary>
    public class SequenceNode : CompositeNodeBase
    {
        private int _currentRunningNodeIndex = -1;

        public SequenceNode(Blackboard blackboard) : base(blackboard, new List<INode>())
        {
        }
        
        public SequenceNode(Blackboard blackboard, List<INode> children) : base(blackboard, children)
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
                if (state == INode.State.Failure)
                {
                    ResetSequence();
                    return INode.State.Failure;
                }

                _currentRunningNodeIndex += 1;
            }

            // 실패가 하나도 없다면 성공 반환
            ResetSequence();
            return INode.State.Success;
        }

        private void ResetSequence()
        {
            _currentRunningNodeIndex = -1;
        }
    }
}