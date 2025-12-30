using ActionHandler.Data;
using Controller.Animation;

namespace ActionHandler.Processor
{
    public class TwoHandSwordAttackProcessor : IAttackProcessor
    {
        private const float SwordAttackDuration = 1.0f;
        private const int DefaultAttackTrigger = 4;
        
        public void SetContext(HandlerContext context, ref AttackHandler.EAttackSide attackSide)
        {
            var nextAttackSide = attackSide == AttackHandler.EAttackSide.Left 
                ? AttackHandler.EAttackSide.Right 
                : AttackHandler.EAttackSide.Left;
            
            context.AnimationType = EAnimationType.Attack;
            context.AttackSide = nextAttackSide;
            context.ActionNumber = nextAttackSide == AttackHandler.EAttackSide.Left ? 1 : 4;
            context.TriggerNumber = DefaultAttackTrigger;
            context.AnimationDuration = SwordAttackDuration;
        }
    }
}