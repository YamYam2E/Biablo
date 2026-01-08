using UnityEngine;

namespace Ai.Node.Condition
{
    public class IsInAttackRangeConditionNode : ConditionNodeBase
    {
        private readonly int _obstacleLayerMask = LayerMask.GetMask("Wall");
        private readonly Transform _agent;
        
        public IsInAttackRangeConditionNode(Transform agent, Blackboard blackboard) : base(blackboard)
        {
            _agent = agent;
        }

        public override INode.State Evaluate()
        {
            return IsInTarget() ? INode.State.Success : INode.State.Failure;
        }
        
        private bool IsInTarget()
        {
            if (!Blackboard.AttackTarget)
                return false;

            var distanceToTarget = Vector3.Distance(_agent.position, Blackboard.AttackTarget.position);

            if (distanceToTarget > Blackboard.AttackRange)
                return false;
            
            if (HasObstacleBetweenActor(Blackboard.AttackTarget))
                return false;
            
            return true;
        }

        private bool HasObstacleBetweenActor(Transform target)
        {
            var directionToTarget = (target.position - _agent.position).normalized;
            var distanceToTarget = Vector3.Distance(_agent.position, target.position);
            
            // 장애물 체크
            if (Physics.Raycast(
                    _agent.position,
                    directionToTarget,
                    out var hit,
                    distanceToTarget,
                    _obstacleLayerMask))
            {
                return true;
            }

            return false;
        }
    }
}