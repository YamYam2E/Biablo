﻿using Common;
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
        public LayerMask targetLayer;
        public int ActionNumber;
        public int TriggerNumber;
        public float AnimationDuration;
        public float AnimationMultiplier = 1f;
        
        public float Damage = 0f;
        
    }
}