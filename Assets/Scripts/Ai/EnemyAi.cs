using Ai.Node;
using Ai.Node.Action;
using Ai.Node.Composite;
using Ai.Node.Condition;
using Controller;
using UnityEngine;
using Util;

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
        private SequenceNode _idleSequence;
        
        private Enemy _actorController;

        private void Start()
        {
            _actorController = GetComponent<Enemy>();

            InitializeBlackboard();
            
            _attackSequence = new SequenceNode(_blackboard);
            _attackSequence.AddChild( new IsInAttackRangeConditionNode(transform, _blackboard) );
            _attackSequence.AddChild( new StopActionNode(_actorController, _blackboard) );
            _attackSequence.AddChild( new AttackActionNode(_actorController, _blackboard) );
            
            _detectSequence = new SequenceNode(_blackboard);
            _detectSequence.AddChild( new DetectiveTargetConditionNode(transform, _blackboard ) );
            _attackSequence.AddChild( new LookAtTargetActionNode( _actorController, _blackboard ) );
            _detectSequence.AddChild( new FollowTargetAction(_actorController, _blackboard) );
            
            
            _idleSequence = new SequenceNode(_blackboard);
            _idleSequence.AddChild( new StopActionNode(_actorController, _blackboard) );

            _rootNode = new SelectorNode(_blackboard);
            _rootNode.AddChild( _attackSequence );
            _rootNode.AddChild( _detectSequence );
            _rootNode.AddChild( _idleSequence );
            
            GameDebug.Log($"Start AI : {_actorController.name}");
        }

        private Blackboard _blackboard;

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

        private readonly Color _gizmosDetectColor = new(1, 0.92f, 0.016f, 0.09f);
        private readonly Color _gizmosAttackColor = new(1f, 0f, 0f, 0.1f);
        private readonly Color _gizmosTargetColor = new(0.6f, 0.1f, 0.4f, 0.2f);
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine( transform.position, transform.position + transform.forward * 10);

            var left = Quaternion.Euler(0, -60 * 0.5f, 0) * transform.forward;
            var right = Quaternion.Euler(0, 60 * 0.5f, 0) * transform.forward;

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + left * 10);
            Gizmos.DrawLine(transform.position, transform.position + right * 10);
            
            Gizmos.color = _gizmosDetectColor;
            Gizmos.DrawSphere(transform.position, detectiveRange);

            Gizmos.color = _gizmosAttackColor;
            Gizmos.DrawSphere(transform.position, attackRange);

            Gizmos.color = _gizmosTargetColor;
            if (_blackboard != null && _blackboard.AttackTarget != null)
            {
                Gizmos.DrawCube( _blackboard.AttackTarget.position, Vector3.one );
            }
        }
    }
}