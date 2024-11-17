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
        // 캐릭터 카메라
        [SerializeField] private new Camera camera;

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
            WeaponController.Equip( animator, EWeaponType.TwoHandSword);
            IKHandController.SetIKOn( (EWeaponType)animator.GetInteger(AnimatorHash.Weapon) );
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && WeaponController.CurrentWeaponType != EWeaponType.Punch)
            {
                WeaponController.UnEquip(animator);
                WeaponController.Equip( animator, EWeaponType.Punch);
                IKHandController.SetIKOff();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && WeaponController.CurrentWeaponType != EWeaponType.TwoHandSword)
            {
                WeaponController.UnEquip(animator);
                WeaponController.Equip( animator, EWeaponType.TwoHandSword);
                IKHandController.SetIKOn( (EWeaponType)animator.GetInteger(AnimatorHash.Weapon) );
            }
        }

        protected override void BindAnimationEvents()
        {
            AnimationEventController = animator.gameObject.AddComponent<AnimationEventController>();
            
            AnimationEventController.OnFootL.AddListener(() => Debug.Log("FootL"));
            AnimationEventController.OnFootR.AddListener(() => Debug.Log("FootR"));
            AnimationEventController.OnHit.AddListener( OnAttackAnimationEvent );

            IKHandController = animator.gameObject.AddComponent<IKHandController>();
            IKHandController.Initialize( WeaponController );
        }

        protected override void BindActionHandlers()
        {
            _actionHandlers.Add(EActionHandler.Attack, new AttackHandler(
                this, 
                context =>
                {
                    OnInputLock(context.AnimationDuration);
                    AnimationController.OnAttack(context);
                }));
            
            _actionHandlers.Add(EActionHandler.Rolling, new RollingHandler(this, 
                context =>
                {
                    OnInputLock(context.AnimationDuration);
                    MovementController.OnRolling(context);
                }));
            
            _actionHandlers.Add(EActionHandler.TakeHit, new TakeHitHandler(this, AnimationController.OnTakeHit));
        }
        
        private void OnAttackAnimationEvent()
        {
            if (!_actionHandlers.TryGetValue(EActionHandler.Attack, out var handler))
                return;
            
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
            
            handler.StartAction( new HandlerContext() {Direction = direction} );
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
    }
}