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
    public class AttackHandler : ActionHandlerBase
    {
        public const float PunchAttackDuration = 0.6f;
        public const float SwordAttackDuration = 1.0f;
        
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
            switch (_attackSide)
            {
                case EAttackSide.Left:
                    return Random.Range(1, 4);
                case EAttackSide.Right:
                    return Random.Range(4, 7);
                case EAttackSide.Both:
                default:
                    return Random.Range(1, 4);
            }
        }

        protected override void StartAction_Internal()
        {
            switch (Context.WeaponType)
            {
                case EWeaponType.Punch: 
                    SetContext_Punch(); 
                    break;
                case EWeaponType.TwoHandSword: 
                    SetContext_Sword(); 
                    break;
                default:
                    break;
            }

            // Animation 속도 강제로 1.5f 조정
            Context.AnimationMultiplier = 1.5f;
            
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
            Context.AnimationDuration = PunchAttackDuration;
        }

        private void SetContext_Sword()
        {
            SetAttackSideType();
            
            Context.AnimationType = EAnimationType.Attack;
            Context.AttackSide = _attackSide;
            Context.ActionNumber = TakeRandomSwordAttackType();
            Context.TriggerNumber = 4;
            Context.AnimationDuration = SwordAttackDuration;
        }
        
        protected override void EndAction_Internal()
        {
        }
    }
}