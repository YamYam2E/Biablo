using Controller;
using UnityEngine;

namespace Ai.Node.Action
{
    public class FollowTargetAction : ActionNodeBase
    {
        private Enemy _actorController;
        
        public FollowTargetAction(Enemy actorController, Blackboard blackboard) : base(blackboard)
        {
            _actorController = actorController;
        }

        public override INode.State Evaluate()
        {
            if( !Blackboard.AttackTarget )
                return INode.State.Failure;
            
            _actorController.MoveTo(Blackboard.AttackTarget.position);
            return INode.State.Success;
        }
    }
}