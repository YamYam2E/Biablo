using UnityEngine;
using Util;

namespace Ai.Node.Condition
{
    public class DetectiveTargetConditionNode : ConditionNodeBase
    {
        private readonly Transform _agent;
        private readonly int _playerLayerMask;
        private readonly int _obstacleLayerMask;

        /// <summary>
        /// 탐지 각도의 절반 값 (계산 목적으로 사용)
        /// </summary>
        private readonly float _halfFovAngle;
        
        public DetectiveTargetConditionNode(Transform agent, Blackboard blackboard) : base(blackboard)
        {
            _agent = agent;
            _playerLayerMask = LayerMask.GetMask("Player");
            _obstacleLayerMask = LayerMask.GetMask("Obstacle");
            _halfFovAngle = Blackboard.DetectiveFovAngle * 0.5f;
        }

        public override INode.State Evaluate()
        {
            if (IsFoundTarget())
                return INode.State.Success;
            
            return INode.State.Failure;
        }

        private bool IsFoundTarget()
        {
            if (Blackboard.AttackTarget)
            {
                var distanceToTarget = Vector3.Distance(_agent.position, Blackboard.AttackTarget.position);
                if (distanceToTarget > Blackboard.DetectiveRange)
                {
                    GameDebug.Log($"Target is out of range", Color.lawnGreen);
                    Blackboard.AttackTarget = null;
                    return false;
                }
            }
            
            // var results = new Collider[1]; 
            // var size = Physics.OverlapSphereNonAlloc(_agent.position, Blackboard.DetectiveRange, results, _playerLayerMask);
            // if (size == 0)
            //     return false;
            
            var results = Physics.OverlapSphere(_agent.position, Blackboard.DetectiveRange, _playerLayerMask);
            if (results.Length == 0)
            {
                GameDebug.Log("Can't found target", Color.orangeRed);
                return false;
            }

            var direction = (results[0].transform.position - _agent.position).normalized;

            if (!CheckTargetInSight(direction, results[0].transform))
            {
                GameDebug.Log("Isn't in sight target", Color.orangeRed);
                return false;
            }
            
            if (CheckObstacleBetweenActor(direction, results[0].transform))
            {
                GameDebug.Log("Has obstacles between target", Color.orangeRed);
                return false;
            }
            
            Blackboard.AttackTarget = results[0].transform;
            
            return true;
        }

        /// <summary>
        /// 부채꼴 범위 체크
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="target"></param>
        /// <returns>true: 타겟이 시야에 있는 경우</returns>
        private bool CheckTargetInSight(Vector3 direction, Transform target)
        {
            var angle = Vector3.Angle(_agent.forward, direction);
            
            if (angle > _halfFovAngle)
                return false;
            
            return true;
        }

        /// <summary>
        /// 오브젝트와 타겟 사이에 장애물 체크
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="target"></param>
        /// <returns>true: 장애물이 있는 경우</returns>
        private bool CheckObstacleBetweenActor(Vector3 direction, Transform target)
        {
            /*
             * 장애물 체크
             */
            var layerMasks = _obstacleLayerMask | _playerLayerMask;
            if( Physics.Raycast(_agent.position, direction, out var hit, 50f, layerMasks) )
                return hit.transform != target;

            return true;
        }
    }
}