using ActionHandler.Data;

namespace ActionHandler.Processor
{
    public interface IAttackProcessor
    {
        public void SetContext(HandlerContext context, ref AttackHandler.EAttackSide attackSide);
    }
}