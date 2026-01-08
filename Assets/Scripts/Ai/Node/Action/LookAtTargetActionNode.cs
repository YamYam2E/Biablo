using Controller;
using UnityEngine;

namespace Ai.Node.Action
{
    public class LookAtTargetActionNode : ActionNodeBase
    {
        private readonly Enemy _actorController;
        
        public LookAtTargetActionNode(Enemy actorController, Blackboard blackboard) : base(blackboard)
        {
            _actorController = actorController;
        }

        public override INode.State Evaluate()
        {
            if (!Blackboard.AttackTarget)
            {
                Debug.Log("<color=red>Target is null</color>");
                return INode.State.Failure;
            }

            _actorController.Stop();
            
            var direction = Blackboard.AttackTarget.position - _actorController.transform.position;
            direction.y = 0f;
            
            if (direction != Vector3.zero)
            {
                Debug.Log("<color=green>Looking at target</color>");
                // Y축 기준으로만 회전
                var rotation = Quaternion.LookRotation(direction);
                _actorController.transform.rotation = rotation;
            }

            return INode.State.Success;
        }
    }
}