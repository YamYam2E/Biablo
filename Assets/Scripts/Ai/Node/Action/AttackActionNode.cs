using System.Collections;
using ActionHandler;
using Controller;
using UnityEngine;

namespace Ai.Node.Action
{
    public class AttackActionNode : ActionNodeBase
    {
        private readonly Enemy _actorController;
        private bool _isAttacking;
        private float _attackDuration;
        private float _attackStartTime;
        
        public AttackActionNode(Enemy actorController, Blackboard blackboard) : base(blackboard)
        {
            _actorController = actorController;
            
        }

        public override INode.State Evaluate()
        {
            if (!_isAttacking)
            {
                _actorController.AttackFromAi();
                
                /*
                 * TODO: (테스트 코드) 펀치만 사용
                 * PunchAttackProcessor.PunchAttackDuration; 참조
                 */
                _attackDuration = 0.6f;
                _attackStartTime = Time.time;
                _isAttacking = true;
            }

            var elapsedTime = Time.time - _attackStartTime;

            if (elapsedTime >= _attackDuration)
            {
                _isAttacking = false;
                return INode.State.Success;
            }

            if (!_actorController.GetAnimator())
                return INode.State.Failure;

            return INode.State.Running;
        }
    }
}