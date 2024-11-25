using UnityEngine;

namespace Ai.Feature
{
    public class DetectiveTarget
    {
        public Transform Target { get; private set; }
        
        private Transform _owner;
        
        private float _detectiveRange;
        private LayerMask _targetLayerMask;
        private LayerMask _obstacleLayerMask;
        
        public DetectiveTarget(Transform owner, float detectiveRange, LayerMask targetLayerMask, LayerMask obstacleLayerMask)
        {
            _owner = owner;
            _detectiveRange = detectiveRange;
            _targetLayerMask = targetLayerMask;
            _obstacleLayerMask = obstacleLayerMask;
        }
        
        public bool IsInTarget()
        {
            var results = new Collider[1];
            
            var size = Physics.OverlapSphereNonAlloc(_owner.position, _detectiveRange, results, _targetLayerMask);

            if (size == 0)
                return false;

            if (HasObstacleBetweenActor(results[0].transform))
                return false;

            Target = results[0].transform;
            
            return true;
        }
        
        private bool HasObstacleBetweenActor(Transform target)
        {
            var directionToTarget = (target.position - _owner.position).normalized;
            var distanceToTarget = Vector3.Distance(_owner.position, target.position);

            // 거리 체크
            if (distanceToTarget > _detectiveRange)
                return false;

            // 장애물 체크
            if (!Physics.Raycast(
                    _owner.position, 
                    directionToTarget, 
                    out var hit, 
                    distanceToTarget,
                    _obstacleLayerMask | _targetLayerMask) ) 
                return true;
            
            // 맞은 오브젝트가 타겟인 경우
            if (hit.transform == target)
                return false;
            
            // 장애물에 가로막힌 경우
            return true;

        }
    }
}