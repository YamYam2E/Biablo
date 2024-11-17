using ActionHandler;
using ActionHandler.Data;
using UnityEngine;

namespace Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        protected const int MaxHitColliders = 10;
        
        public abstract void EquipWeapon(AttackHandler.EAttackSide side, Transform parent, Vector3 position, Quaternion rotation);
        public abstract void OnAttack(HandlerContext context);
    }
}