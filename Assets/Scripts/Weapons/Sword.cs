using ActionHandler;
using ActionHandler.Data;
using Controller;
using UnityEngine;

namespace Weapons
{
    public class Sword : WeaponBase
    {
        [SerializeField] private BoxCollider targetCollider;
        
        public override void EquipWeapon(AttackHandler.EAttackSide side, Transform parent, Vector3 position, Quaternion rotation)
        {
        }

        public override void OnAttack(HandlerContext context)
        {
            var targets = new Collider[MaxHitColliders];

            // 박스의 중심점 계산 (BoxCollider의 위치 + offset)
            var center = transform.TransformPoint(targetCollider.center);
            
            // 박스의 크기 계산 (로컬 스케일 적용)
            var size = Vector3.Scale(targetCollider.size, transform.localScale);
            
            // 박스의 회전 적용
            var rotation = transform.rotation;

            var count = Physics.OverlapBoxNonAlloc(
                center,
                size * 0.5f,
                targets,
                rotation,
                LayerMask.GetMask("Enemy"));
            
            for (var index = 0; index < count; index++)
            {
                targets[index].GetComponent<ActorControllerBase>().OnTakeDamage(false, 10f);
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(targetCollider.center, targetCollider.size);
        }
    }
}