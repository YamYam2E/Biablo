using System;
using ActionHandler;
using ActionHandler.Data;
using Common;
using Controller.Animation;
using Status;
using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(MovementController))]
    public class Player : ActorControllerBase
    {
        // 캐릭터 카메라
        [SerializeField] private Camera playerCamera;

        private InputController _inputController;
        
        protected override void Start()
        {
            Status = new ActorStatus
            {
                MaxHealth = 100,
                CurrentHealth = 100,
                AttackSpeed = 1.5f,
                WalkingSpeed = 3f,
                RunningSpeed = 6f,
                Stamina = 100
            };
            
            base.Start();
            
            MovementController.Initialize( animator, rigidBody, navMeshAgent, Status.WalkingSpeed, Status.RunningSpeed );
            MovementController.SetRotate(playerCamera);
            
            _inputController = gameObject.GetComponent<InputController>();
            _inputController.OnMovementEvent.AddListener( MovementController.OnMove );
            _inputController.OnAttackEvent.AddListener(OnAttackInput);
            _inputController.OnRollingEvent.AddListener(OnRollingInput);
            _inputController.OnWeaponSwitchEvent.AddListener(OnWeaponSwitchInput);
            
            // 기본 무기는 주먹
            WeaponController.Setup( animator );
            WeaponController.Equip( animator, EWeaponType.TwoHandSword);
        }

        protected override void BindAnimationEvents()
        {
            if (AnimationEventController != null)
                return;
            
            AnimationEventController = animator.gameObject.AddComponent<AnimationEventController>();
            
            /*
             * TODO: 발걸음에 이벤트를 집어넣을 경우 사용
             */
            // AnimationEventController.OnFootL.AddListener(() => Debug.Log("FootL"));
            // AnimationEventController.OnFootR.AddListener(() => Debug.Log("FootR"));
            AnimationEventController.OnHit.AddListener( OnAttackAnimationEvent );
        }

        protected override void BindActionHandlers()
        {
            _actionHandlers.Add(EActionHandler.Attack, new AttackHandler(
                this, 
                context =>
                {
                    OnInputLock(context.AdjustedDuration);
                    AnimationController.OnAttack(context);
                }));
            
            _actionHandlers.Add(EActionHandler.Rolling, new RollingHandler(this, 
                context =>
                {
                    OnInputLock(context.AdjustedDuration);
                    MovementController.OnRolling(context);
                }));
            
            _actionHandlers.Add(EActionHandler.TakeHit, new TakeHitHandler(this, AnimationController.OnTakeHit));
        }
        
        private void OnAttackAnimationEvent()
        {
            if (!_actionHandlers.TryGetValue(EActionHandler.Attack, out var handler))
                return;
           
            // 타겟 레이어 정의
            handler.Context.targetLayer = LayerMask.GetMask("Enemy");
            
            WeaponController.OnAttack(handler.Context);
        }

        private void OnAttackInput()
        {
            if (!_actionHandlers.TryGetValue(EActionHandler.Attack, out var handler))
                return;

            handler.StartAction(
                new HandlerContext
                {
                    WeaponType = WeaponController.CurrentWeaponType
                });
        }
        
        private void OnRollingInput(Vector3 direction)
        {
            if (!_actionHandlers.TryGetValue(EActionHandler.Rolling, out var handler))
                return;

            if (handler.IsActive)
                return;
                            
            handler.StartAction( new HandlerContext()
            {
                AnimationMultiplier = 1.5f,
                Direction = direction
            } );
        }

        private void OnWeaponSwitchInput(EWeaponType weaponType)
        {
            if (!AbleToAction)
                return;
            
            if (WeaponController.CurrentWeaponType != EWeaponType.Punch)
            {
                WeaponController.UnEquip(animator);
                WeaponController.Equip( animator, EWeaponType.Punch);
                
                OnInputLock(WeaponController.WeaponSwitchTime);
            }

            if (WeaponController.CurrentWeaponType != EWeaponType.TwoHandSword)
            {
                WeaponController.UnEquip(animator);
                WeaponController.Equip( animator, EWeaponType.TwoHandSword);
                
                OnInputLock(WeaponController.WeaponSwitchTime);
            }
        }

        public override void OnTakeDamage(bool isCritical, float damage)
        {
            if (!_actionHandlers.TryGetValue(EActionHandler.TakeHit, out var handler))
                return;
            
            handler.StartAction( 
                new HandlerContext
                {
                    Damage = damage,
                });
        }

        protected override void UnlockInput_Internal()
        {
            /*
             * TODO: 전체 핸들러의 액션을 끄는 것보다,
             * 활성화 된 액션이 무엇인지 체크하고 그 액션만 끄는게 좋지 않을까?
             */
            foreach (var handler in _actionHandlers.Values)
                handler.EndAction();
        }
    }
}