using System;
using UnityEngine;

namespace Ai.Node.Condition
{
    public class DetectiveTargetConditionNode : ConditionNodeBase
    {
        private Transform _agent;


        public DetectiveTargetConditionNode(Transform agent, Blackboard blackboard) : base(blackboard)
        {
            _agent = agent;
        }

        public override INode.State Evaluate()
        {
            return IsFoundTarget() ? INode.State.Success : INode.State.Failure;
        }

        private bool IsFoundTarget()
        {
            Blackboard.AttackTarget = null;
            
            var results = new Collider[1];
            var playerLayerMask = LayerMask.GetMask("Player");
            
            var size = Physics.OverlapSphereNonAlloc(_agent.position, Blackboard.DetectiveRange, results, playerLayerMask);

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

            var playerLayerMask = LayerMask.GetMask("Player");
            var obstacleLayerMask = LayerMask.GetMask("Obstacle");
            // 거리 체크
            if (distanceToTarget > Blackboard.AttackRange)
                return false;

            // 장애물 체크
            if (!Physics.Raycast(
                    _agent.position, 
                    directionToTarget, 
                    out var hit, 
                    distanceToTarget,
                    obstacleLayerMask | playerLayerMask) ) 
                return true;
            
            // 맞은 오브젝트가 타겟인 경우
            if (hit.transform == target)
                return false;
            
            // 장애물에 가로막힌 경우
            return true;

        }
    }
}