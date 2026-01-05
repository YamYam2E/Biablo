using System;
using System.Collections.Generic;
using ActionHandler.Data;
using ActionHandler.Processor;
using Common;
using Controller;

namespace ActionHandler
{
    public class AttackHandler : ActionHandlerBase
    {
        public enum EAttackSide
        {
            None,
            Left,
            Right,
            Both,
        }
        
        private EAttackSide _attackSide;
        
        private readonly Dictionary<EWeaponType, IAttackProcessor> _attackProcessors;

        public AttackHandler(ActorControllerBase controller, Action<HandlerContext> animationCallback) 
            : base(controller, animationCallback)
        {
            /*
             * TODO: 의존성 주입을 위해 외부에서 주입받도록 변경
             * 객체간의 결합도를 낮추는 식으로 코딩해볼 것
             */
            _attackProcessors = new Dictionary<EWeaponType, IAttackProcessor>
            {
                { EWeaponType.Punch, new PunchAttackProcessor() },
                { EWeaponType.TwoHandSword , new TwoHandSwordAttackProcessor() }
            };
        }

        protected override void StartAction_Internal()
        {
            if (_attackProcessors.TryGetValue(Context.WeaponType, out var processor))
            {
                processor.SetContext(Context, ref _attackSide);
            }

            /*
             * TODO: 이렇게 하면 나중에 계속해서 강제로 속도 조절해야하는 문제 발생
             * 캐릭터의 스텟(예, 속도)이나 무기별 속도에 따라 변하도록 동적으로 바꾸는 것이 필요.
             */
            Context.AnimationMultiplier = 1.5f;
            AnimationCallback?.Invoke(Context);
        }

        protected override void EndAction_Internal()
        {
        }
    }
}