using System;
using ActionHandler;
using ActionHandler.Data;
using Common;
using Controller.Animation;
using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(InputController), typeof(MovementController))]
    public class Player : ActorControllerBase
    {
        [Serializable]
        public class State
        {
            public bool IsRolling = false;
        }
        
        // 캐릭터 카메라
        [SerializeField] private new Camera camera;

        [SerializeField] private State ActorState = new();
        
        private InputController _inputController;
        
        protected override void Start()
        {
            base.Start();
            
            MovementController.Initialize( animator, rigidBody, navMeshAgent, walkingSpeed, runningSpeed );
            MovementController.SetRotate(camera);
            
            _inputController = gameObject.GetComponent<InputController>();
            _inputController.OnMovementEvent.AddListener( MovementController.OnMove );
            _inputController.OnAttackEvent.AddListener(OnAttackInput);
            _inputController.OnRollingEvent.AddListener(OnRollingInput);
            
            // 기본 무기는 주먹
            WeaponController.Setup( animator );
            WeaponController.Equip( animator, EWeaponType.TwoHandSword);
        }

        private void Update()
        {
            if (AbleToAction == false)
                return;
            
            if (Input.GetKeyDown(KeyCode.Alpha1) && WeaponController.CurrentWeaponType != EWeaponType.Punch)
            {
                WeaponController.UnEquip(animator);
                WeaponController.Equip( animator, EWeaponType.Punch);
                
                OnInputLock(0.8f);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && WeaponController.CurrentWeaponType != EWeaponType.TwoHandSword)
            {
                WeaponController.UnEquip(animator);
                WeaponController.Equip( animator, EWeaponType.TwoHandSword);
                
                OnInputLock(0.8f);
            }
        }

        protected override void BindAnimationEvents()
        {
            AnimationEventController = animator.gameObject.AddComponent<AnimationEventController>();
            
            AnimationEventController.OnFootL.AddListener(() => Debug.Log("FootL"));
            AnimationEventController.OnFootR.AddListener(() => Debug.Log("FootR"));
            AnimationEventController.OnHit.AddListener( OnAttackAnimationEvent );
        }

        protected override void BindActionHandlers()
        {
            _actionHandlers.Add(EActionHandler.Attack, new AttackHandler(
                this, 
                context =>
                {
                    OnInputLock(context.AnimationDuration / context.AnimationMultiplier);
                    AnimationController.OnAttack(context);
                }));
            
            _actionHandlers.Add(EActionHandler.Rolling, new RollingHandler(this, 
                context =>
                {
                    OnInputLock(context.AnimationDuration / context.AnimationMultiplier);
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
            
            if (ActorState.IsRolling)
                return;
                            
            ActorState.IsRolling = true;

            handler.StartAction( new HandlerContext()
            {
                AnimationMultiplier = 1.5f,
                Direction = direction
            } );
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
            ActorState.IsRolling = false;
        }
    }
}