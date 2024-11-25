using System.Collections;
using System.Collections.Generic;
using ActionHandler;
using Common;
using Controller.Animation;
using UnityEngine;
using UnityEngine.AI;

namespace Controller
{
    [RequireComponent(typeof(AnimationController), typeof(WeaponController))]
    public abstract class ActorControllerBase : MonoBehaviour
    {
        // 이동 관련 변수
        [SerializeField] protected float walkingSpeed = 2f;
        [SerializeField] protected float runningSpeed = 6f;
        [SerializeField] protected float stoppingDistance = 0.1f;

        // 컴포넌트 참조
        [SerializeField] protected Rigidbody rigidBody;
        [SerializeField] protected Animator animator;

        // 네비
        [SerializeField] protected NavMeshAgent navMeshAgent;

        public bool AbleToAction { get; protected set; } = true;

        protected AnimationController AnimationController;
        protected AnimationEventController AnimationEventController;
        protected MovementController MovementController;
        protected WeaponController WeaponController;

        protected readonly Dictionary<EActionHandler, ActionHandlerBase> _actionHandlers = new();

        protected Coroutine InputLockCoroutine;
        
        protected virtual void Start()
        {
            navMeshAgent.speed = walkingSpeed;
            navMeshAgent.stoppingDistance = stoppingDistance;

            MovementController = gameObject.GetComponent<MovementController>();
            AnimationController = gameObject.GetComponent<AnimationController>();
            WeaponController = gameObject.GetComponent<WeaponController>();
            
            AnimationController.Initialize(animator);

            BindAnimationEvents();
            BindActionHandlers();
        }

        protected abstract void BindAnimationEvents();
        protected abstract void BindActionHandlers();

        public abstract void OnTakeDamage(bool isCritical, float damage);

        protected void OnInputLock(float duration)
        {
            if (InputLockCoroutine != null)
                StopCoroutine(InputLockCoroutine);

            InputLockCoroutine = StartCoroutine(LockInput(duration));
        }

        private IEnumerator LockInput(float duration)
        {
            AbleToAction = false;
            MovementController.SetLock(true);

            yield return new WaitForSeconds(duration);

            UnlockInput();
        }

        protected void UnlockInput()
        {
            AbleToAction = true;
            MovementController.SetLock(false);

            UnlockInput_Internal();
        }
        
        protected abstract void UnlockInput_Internal();
        
        public Animator GetAnimator() => animator;
    }
}