using UnityEngine;

namespace Common
{
    public class AnimatorHash
    {
        public static readonly int Moving = Animator.StringToHash("Moving");
        public static readonly int VelocityX = Animator.StringToHash("Velocity X");
        public static readonly int VelocityZ = Animator.StringToHash("Velocity Z");
        public static readonly int AnimationSpeed = Animator.StringToHash("AnimationSpeed");
        
        public static readonly int TriggerNumber = Animator.StringToHash("TriggerNumber");
        public static readonly int Trigger = Animator.StringToHash("Trigger");
        public static readonly int Side = Animator.StringToHash("Side");
        public static readonly int Action = Animator.StringToHash("Action");
        public static readonly int Weapon = Animator.StringToHash("Weapon");
    }
}