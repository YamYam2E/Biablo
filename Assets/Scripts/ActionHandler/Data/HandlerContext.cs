using Common;
using Common.SkillType;
using Controller.Animation;
using UnityEngine;

namespace ActionHandler.Data
{
    public class HandlerContext
    {
        public Vector3 Direction = Vector3.zero;

        public EWeaponType WeaponType = EWeaponType.Punch;
        public EAnimationType AnimationType = EAnimationType.None;
        public AttackHandler.EAttackSide AttackSide;
        public int ActionNumber;
        public int TriggerNumber;
        public float AnimationDuration;
        
        public float Damage = 0f;
        
    }
}