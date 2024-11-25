using ActionHandler;
using ActionHandler.Data;
using Common;
using Common.SkillType;
using UnityEngine;

namespace Controller.Animation
{
    public class AnimationController : MonoBehaviour
    {
        private Animator _animator;

        public void Initialize(Animator animator)
        {
            _animator = animator;
        }

        public void OnAttack(HandlerContext context)
        {
            _animator.SetFloat(AnimatorHash.AnimationSpeed, context.AnimationMultiplier);
            
            _animator.SetInteger(AnimatorHash.Weapon, (int)context.WeaponType);
            _animator.SetInteger(AnimatorHash.Action, (int)context.ActionNumber);
            
            // 샘플 애니메이션에서 attack trigger 값이 4
            _animator.SetInteger(AnimatorHash.TriggerNumber, context.TriggerNumber);
            _animator.SetTrigger(AnimatorHash.Trigger);
        }
        
        public void OnTakeHit(HandlerContext context)
        {
            Debug.Log(gameObject.name + " taking damage");
            
            _animator.SetInteger(AnimatorHash.Action, 2);
            
            // 샘플 애니메이션에서 get hit trigger 값이 12
            _animator.SetInteger(AnimatorHash.TriggerNumber, 12);
            _animator.SetTrigger(AnimatorHash.Trigger);
        }
    }
}