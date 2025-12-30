using System.Collections;
using System.Collections.Generic;
using ActionHandler;
using Common;
using Controller.Animation;
using Status;
using UnityEngine;
using UnityEngine.AI;

namespace Controller
{
    /// <summary>
    /// Base class for all actor controllers in the game.
    /// </summary>
    /// <remarks>
    /// This abstract class handles core functionalities such as movement, animation, status,
    /// and action handling for actors. It integrates various components like
    /// <c>AnimationController</c>, <c>WeaponController</c>, and <c>MovementController</c>.
    /// Derived classes must implement certain abstract methods to define
    /// custom behavior.
    /// </remarks>
    [RequireComponent(typeof(AnimationController), typeof(WeaponController))]
    public abstract class ActorControllerBase : MonoBehaviour
    {
        // 이동 관련 변수
        [SerializeField] protected float stoppingDistance = 0.1f;

        // 컴포넌트 참조
        [SerializeField] protected Rigidbody rigidBody;
        [SerializeField] protected Animator animator;

        // 네비
        [SerializeField] protected NavMeshAgent navMeshAgent;

        public bool AbleToAction { get; private set; } = true;

        protected AnimationController AnimationController;
        protected AnimationEventController AnimationEventController;
        protected MovementController MovementController;
        protected WeaponController WeaponController;
        protected ActorStatus Status;

        protected readonly Dictionary<EActionHandler, ActionHandlerBase> _actionHandlers = new();

        private Coroutine InputLockCoroutine;


        protected virtual void Start()
        {
            navMeshAgent.speed = Status.WalkingSpeed;
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

        /// <summary>
        /// Locks the input for a specified duration, disabling actions and movement.
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator LockInput(float duration)
        {
            AbleToAction = false;
            MovementController.SetLock(true);

            yield return new WaitForSeconds(duration);

            UnlockInput();
        }

        /// <summary>
        /// Unlocks the input.
        /// </summary>
        private void UnlockInput()
        {
            AbleToAction = true;
            MovementController.SetLock(false);

            UnlockInput_Internal();
        }
        
        protected abstract void UnlockInput_Internal();
        
        public Animator GetAnimator() => animator;
    }
}