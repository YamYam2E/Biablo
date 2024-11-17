using System;
using System.Linq;
using ActionHandler.Data;
using Common;
using Common.SkillType;
using Controller;
using Controller.Animation;
using Random = UnityEngine.Random;

namespace ActionHandler
{
    public class AttackHandler : ActionBaseHandler
    {
        private const float AttackDuration = 0.75f;
        
        public enum EAttackSide
        {
            None,
            Left,
            Right,
            Both,
        }
        
        
        private EAttackSide _attackSide;

        public AttackHandler(ActorControllerBase controller, Action<HandlerContext> animationCallback) 
            : base(controller, animationCallback)
        {
        }

        private void SetAttackSideType()
        {
            switch (_attackSide)
            {
                case EAttackSide.None:
                    _attackSide = EAttackSide.Left; break;
                case EAttackSide.Left:
                    _attackSide = EAttackSide.Right; break;
                case EAttackSide.Right:
                    _attackSide = EAttackSide.Left; break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private CharacterSkillType.EPunchType TakeRandomPunchType(EAttackSide sideType)
        {
            var value = sideType == EAttackSide.Left ? 
                CharacterSkillType.LeftPunches : 
                CharacterSkillType.RightPunches;

            return value[Random.Range(0, value.Length)];
        }

        private int TakeRandomSwordAttackType()
        {
            // 임시코드
            var value = Enum.GetNames(typeof(CharacterSkillType.ESwordAttackType));
            return Random.Range(0, value.Length);
        }

        protected override void StartAction_Internal()
        {
            switch (Context.WeaponType)
            {
                case EWeaponType.Punch: SetContext_Punch(); break;
                case EWeaponType.TwoHandSword: SetContext_Sword(); break;
                default:
                    break;
            }
            
            AnimationCallback?.Invoke(Context);
        }

        private void SetContext_Punch()
        {
            SetAttackSideType();

            var actionType = TakeRandomPunchType(_attackSide);

            Context.AnimationType = EAnimationType.Attack;
            Context.AttackSide = _attackSide;
            Context.ActionNumber = (int)actionType;
            Context.TriggerNumber = 4;
            Context.AnimationDuration = AttackDuration;
        }

        private void SetContext_Sword()
        {
            _attackSide = EAttackSide.Left;
            Context.AnimationType = EAnimationType.Attack;
            Context.AttackSide = _attackSide;
            Context.ActionNumber = TakeRandomSwordAttackType();
            Context.TriggerNumber = 4;
            Context.AnimationDuration = AttackDuration;
        }
        
        protected override void EndAction_Internal()
        {
        }
    }
}