using Controller;

namespace Ai.Node.Action
{
    public class StopActionNode : ActionNodeBase
    {
        private readonly Enemy _actorController;
        
        public StopActionNode(Enemy actorController, Blackboard blackboard) : base(blackboard)
        {
            _actorController = actorController;
        }
        
        public override INode.State Evaluate()
        {
            _actorController.Stop();
            return INode.State.Success;
        }
    }
}