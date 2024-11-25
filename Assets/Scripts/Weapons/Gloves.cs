using ActionHandler;
using ActionHandler.Data;
using Controller;
using UnityEngine;

namespace Weapons
{
    public class Gloves : WeaponBase
    {
        [SerializeField] private SphereCollider targetCollider;
        [SerializeField] private float radius;

        private void Start()
        {
            if( radius <= 0f )
                radius = targetCollider.radius;
        }

        public override void EquipWeapon(AttackHandler.EAttackSide side, Transform parent, Vector3 position, Quaternion rotation)
        {
            
        }

        public override void OnAttack(HandlerContext context)
        {
            var targets = new Collider[MaxHitColliders];
            var size = Physics.OverlapSphereNonAlloc(
                targetCollider.transform.position, 
                radius, 
                targets, 
                context.targetLayer);

            if (size == 0)
                return;

            for (var index = 0; index < size; index++)
            {
                targets[index].GetComponent<ActorControllerBase>().OnTakeDamage(false, 10f);
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawWireSphere(targetCollider.transform.position, radius);
        }
    }
}