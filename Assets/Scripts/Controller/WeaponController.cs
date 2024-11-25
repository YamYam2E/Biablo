using System.Collections.Generic;
using ActionHandler;
using ActionHandler.Data;
using Common;
using Controller.Animation;
using UnityEngine;
using Weapons;

namespace Controller
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;

        [SerializeField] private WeaponBase leftWeapon;
        [SerializeField] private WeaponBase rightWeapon;

        public EWeaponType CurrentWeaponType = EWeaponType.Punch;
        
        private const int MaxHitCount = 5;

        private Dictionary<EWeaponType, List<WeaponBase>> _weapons = new();
        
        private IKHandController _ikHandController;
        
        public void Setup(Animator animator)
        {
            _ikHandController = animator.gameObject.AddComponent<IKHandController>();
            _ikHandController.Initialize( this );
        }

        public void UnEquip(Animator animator)
        {
            if (!_weapons.TryGetValue(CurrentWeaponType, out var weapon))
                return;

            leftWeapon = null;
            rightWeapon = null;
            
            weapon.ForEach(item => item.gameObject.SetActive(false));
        }

        public void Equip(Animator animator, EWeaponType weaponType)
        {
            if ( !_weapons.TryGetValue(weaponType, out var weapons) )
                weapons = CreateEquippedWeapons(weaponType);

            if (weapons == null || weapons.Count == 0)
                return;

            leftWeapon = weapons[0];
            rightWeapon = weapons.Count > 1 ? weapons[1] : null;

            for (var index = 0; index < weapons.Count; index++)
                weapons[index].gameObject.SetActive(true);

            CurrentWeaponType = weaponType;
            animator.SetFloat(AnimatorHash.AnimationSpeed, 1f);
            animator.SetInteger(AnimatorHash.Weapon, 1);
            animator.SetInteger(AnimatorHash.TriggerNumber, GetTriggerNumberForSwitching(weaponType));
            animator.SetTrigger(AnimatorHash.Trigger);

            if (weaponType == EWeaponType.Punch)
                _ikHandController.SetIKOff();
            else
                _ikHandController.SetIKOn( (EWeaponType)animator.GetInteger(AnimatorHash.Weapon) );
        }

        private int GetTriggerNumberForSwitching(EWeaponType weaponType)
        {
            switch (weaponType)
            {
                case EWeaponType.Punch: return 15;
                case EWeaponType.TwoHandSword: return 16;
                default: return 0;
            }
        }

        private List<WeaponBase> CreateEquippedWeapons(EWeaponType weaponType)
        {
            List<WeaponBase> returnValue = null;
            
            switch (weaponType)
            {
                case EWeaponType.Punch:
                    var glove = Resources.Load<Gloves>("Prefab/Weapon/Glove");
                    _weapons.Add( 
                        weaponType, 
                        new List<WeaponBase>
                        {
                            Instantiate(glove, leftHand), 
                            Instantiate(glove, rightHand)
                        });

                    returnValue = _weapons[weaponType];
                    break;
                case EWeaponType.TwoHandSword:
                    var sword = Resources.Load<Sword>("Prefab/Weapon/Sword");
                    _weapons.Add( 
                        weaponType, 
                        new List<WeaponBase>
                        {
                            Instantiate(sword, leftHand), 
                        });
                    returnValue = _weapons[weaponType];
                    break;
                    
                default:
                    break;
            }

            return returnValue;
        }

        public void OnAttack(HandlerContext context)
        {
            WeaponBase weapon;
            
            if (context.WeaponType == EWeaponType.TwoHandSword)
                weapon = leftWeapon;
            else
                weapon = context.AttackSide == AttackHandler.EAttackSide.Left ? leftWeapon : rightWeapon;
  
            weapon.OnAttack(context);
        }
        
        public Transform LeftHand => leftHand;
        public Transform RightHand => rightHand;
        public WeaponBase LeftWeapon => leftWeapon;
        public WeaponBase RightWeapon => rightWeapon;
    }
}