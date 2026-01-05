using System.Collections.Generic;
using UnityEngine;

namespace Ai
{
    /// <summary>
    /// Boxing, UnBoxing 최소화를 위해, object dictionary 미사용
    /// </summary>
    public class Blackboard
    {
        public Transform AttackTarget;
        public float AttackRange;
        public float DetectiveRange;
        public LayerMask TargetLayerMask;

        public float DetectiveFovAngle = 60f;
    }
}