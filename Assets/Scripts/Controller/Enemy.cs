using ActionHandler;
using ActionHandler.Data;
using Common;
using Controller.Animation;
using Status;
using UnityEngine;
using UnityEngine.AI;

namespace Controller
{
    public class Enemy : ActorControllerBase
    {
        [SerializeField] private NavMeshObstacle navMeshObstacle;

        protected override void Start()
        {
            Status = new ActorStatus
            {
                MaxHealth = 100,
                CurrentHealth = 100,
                AttackSpeed = 0.5f,
                WalkingSpeed = 2.5f,
                RunningSpeed = 5f,
                Stamina = 100
            };
            
            base.Start();
            
            MovementController.Initialize( animator, rigidBody, navMeshAgent, Status.WalkingSpeed, Status.RunningSpeed );
            
            // 기본 무기는 주먹
            WeaponController.Setup( animator );
            WeaponController.Equip( animator, EWeaponType.Punch);
            
            navMeshAgent.angularSpeed = 720f;
        }

        protected override void BindAnimationEvents()
        {
            AnimationEventController = animator.gameObject.AddComponent<AnimationEventController>();
            
            // AnimationEventController.OnFootL.AddListener(() => Debug.Log("FootL"));
            // AnimationEventController.OnFootR.AddListener(() => Debug.Log("FootR"));
            AnimationEventController.OnHit.AddListener( OnAttackAnimationEvent );
        }

        protected override void BindActionHandlers()
        {
            _actionHandlers.Add(EActionHandler.Attack, new AttackHandler(
                this, 
                handlerContext =>
                {
                    OnInputLock(handlerContext.AnimationDuration);
                    AnimationController.OnAttack(handlerContext);
                }));

            //_actionHandlers.Add(EActionHandler.Rolling, new RollingHandler(this, MovementController.OnPlayerRolling));
            _actionHandlers.Add(EActionHandler.TakeHit, new TakeHitHandler(this, AnimationController.OnTakeHit));
        }

        public void MoveTo(Vector3 destination)
        {
            MovementController.MoveTo(destination);
        }

        public void Stop()
        {
            MovementController.Stop();
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

        public void AttackFromAi()
        {
            if (!_actionHandlers.TryGetValue(EActionHandler.Attack, out var handler))
                return;

            handler.StartAction(
                new HandlerContext
                {
                    WeaponType = WeaponController.CurrentWeaponType
                });
        }
        
        private void OnAttackAnimationEvent()
        {
            if (!_actionHandlers.TryGetValue(EActionHandler.Attack, out var handler))
                return;
            
            // 타겟 레이어 정의
            handler.Context.targetLayer = LayerMask.GetMask("Player");
            
            WeaponController.OnAttack(handler.Context);
        }

        protected override void UnlockInput_Internal()
        {
        }
    }
}