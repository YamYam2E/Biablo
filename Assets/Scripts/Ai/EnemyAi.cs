using System.Collections.Generic;
using Ai.Feature;
using Ai.Node;
using Ai.Node.Action;
using Ai.Node.Composite;
using Ai.Node.Condition;
using Controller;
using UnityEngine;

namespace Ai
{
    public class EnemyAi : MonoBehaviour
    {
        [SerializeField] private float detectiveRange;
        [SerializeField] private float attackRange;
        [SerializeField] private LayerMask targetLayerMask;
        [SerializeField] private LayerMask obstacleLayerMask;
        
        private SelectorNode _rootNode;

        private SequenceNode _attackSequence;
        private SequenceNode _detectSequence;
        private ActionNode _idleAction;
        
        private DetectiveTarget _detectiveTarget;
        
        private Enemy _actorController;
        
        private Blackboard _blackboard;
        
        private void Start()
        {
            _actorController = GetComponent<Enemy>();

            InitializeBlackboard();
            
            _detectiveTarget = new DetectiveTarget(transform, detectiveRange, targetLayerMask, obstacleLayerMask);
            
            _attackSequence = new SequenceNode(_blackboard);
            _attackSequence.AddChild( new IsInAttackRangeConditionNode( transform, _blackboard ) );
            _attackSequence.AddChild( new AttackActionNode(_actorController, _blackboard) );
            
            _detectSequence = new SequenceNode(_blackboard);
            _detectSequence.AddChild( new ActionNode( TryFindTarget ) );
            _detectSequence.AddChild( new ActionNode( FollowTarget ) );

            _idleAction = new ActionNode(Idle);
            
            _rootNode = new SelectorNode(_blackboard);
            _rootNode.AddChild( _attackSequence );
            _rootNode.AddChild( _detectSequence );
            _rootNode.AddChild( _idleAction );
        }

        private void InitializeBlackboard()
        {
            _blackboard = new Blackboard
            {
                AttackTarget = null,
                AttackRange = attackRange,
                DetectiveRange = detectiveRange,
                TargetLayerMask = targetLayerMask
            };
        }

        private INode.State Attack()
        {
            return INode.State.Success;
        }

        private INode.State TryFindTarget()
        {
            if (_detectiveTarget.IsInTarget())
                return INode.State.Success;

            return INode.State.Failure;
        }
        
        private INode.State FollowTarget()
        {
            if( ReferenceEquals(_detectiveTarget.Target, null) )
                return INode.State.Failure;
            
            _actorController.MoveTo(_detectiveTarget.Target.position);
            return INode.State.Success;
        }
        
        private INode.State Idle()
        {
            return INode.State.Success;
        }

        private void Update()
        {
            _rootNode?.Evaluate();
        }

        private Color _gizmosDetectColor = new(1, 0.92f, 0.016f, 0.05f);
        private Color _gizmosAttackColor = new(1f, 0f, 0f, 0.1f);
        
        private void OnDrawGizmos()
        {
            Gizmos.color = _gizmosDetectColor;
            // Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawSphere(transform.position, detectiveRange);

            Gizmos.color = _gizmosAttackColor;
            // Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawSphere(transform.position, attackRange);
        }
    }
}