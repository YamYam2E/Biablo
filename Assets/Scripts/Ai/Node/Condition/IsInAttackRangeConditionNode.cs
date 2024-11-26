using UnityEngine;

namespace Ai.Node.Condition
{
    public class IsInAttackRangeConditionNode : ConditionNodeBase
    {
        private Transform _agent;
        
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
            Blackboard.AttackTarget = null;
            
            var results = new Collider[1];
            var playerLayerMask = LayerMask.GetMask("Player");
            
            var size = Physics.OverlapSphereNonAlloc(_agent.position, Blackboard.AttackRange, results, playerLayerMask);

            if (size == 0)
                return false;

            if (HasObstacleBetweenActor(results[0].transform))
                return false;

            Blackboard.AttackTarget = results[0].transform;
            
            return true;
        }

        private bool HasObstacleBetweenActor(Transform target)
        {
            var directionToTarget = (target.position - _agent.position).normalized;
            var distanceToTarget = Vector3.Distance(_agent.position, target.position);
            var obstacleLayerMask = LayerMask.GetMask("Wall");
            
            // 장애물 체크
            if (Physics.Raycast(
                    _agent.position,
                    directionToTarget,
                    out var hit,
                    distanceToTarget,
                    obstacleLayerMask))
            {
                Debug.Log(hit.collider.gameObject.name);
                return true;
            }
            // 장애물에 가로막힌 경우
            return false;

        }
    }
}