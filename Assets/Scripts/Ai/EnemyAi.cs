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
        
        private Enemy _actorController;
        
        private Blackboard _blackboard;
        
        private void Start()
        {
            _actorController = GetComponent<Enemy>();

            InitializeBlackboard();
            
            _attackSequence = new SequenceNode(_blackboard);
            _attackSequence.AddChild( new IsInAttackRangeConditionNode( transform, _blackboard ) );
            _attackSequence.AddChild( new LookAtTargetActionNode( _actorController, _blackboard ) );
            _attackSequence.AddChild( new AttackActionNode(_actorController, _blackboard) );
            
            _detectSequence = new SequenceNode(_blackboard);
            _detectSequence.AddChild( new DetectiveTargetConditionNode(transform, _blackboard ) );
            _detectSequence.AddChild( new FollowTargetAction(_actorController, _blackboard) );

            _rootNode = new SelectorNode(_blackboard);
            _rootNode.AddChild( _attackSequence );
            _rootNode.AddChild( _detectSequence );
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

        private void Update()
        {
            _rootNode?.Evaluate();
        }

        private Color _gizmosDetectColor = new(1, 0.92f, 0.016f, 0.05f);
        private Color _gizmosAttackColor = new(1f, 0f, 0f, 0.1f);
        private Color _gizmosTargetColor = new(0.6f, 0.1f, 0.4f, 0.2f);
        
        private void OnDrawGizmos()
        {
            Gizmos.color = _gizmosDetectColor;
            // Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawSphere(transform.position, detectiveRange);

            Gizmos.color = _gizmosAttackColor;
            // Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawSphere(transform.position, attackRange);

            Gizmos.color = _gizmosTargetColor;
            if (_blackboard != null && _blackboard.AttackTarget != null)
            {
                Gizmos.DrawCube( _blackboard.AttackTarget.position, Vector3.one );
            }
        }
    }
}