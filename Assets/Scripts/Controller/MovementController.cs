using System;
using System.Collections;
using ActionHandler.Data;
using Common;
using UnityEngine;
using UnityEngine.AI;

namespace Controller
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private AnimationCurve rollingCurve;
        
        public bool Locked { get; private set; }
        public bool IsRolling { get; private set; }
        public bool IsRotate { get; private set; }
        
        private Animator _animator;
        private Rigidbody _rigidBody;
        private NavMeshAgent _navMeshAgent;
        private Camera _camera;

        private float _rollingSpeed = 4f;
        private float _walkingSpeed;
        private float _runningSpeed;
        private const float RotationSpeed = 80f;
        private Vector3 _currentVelocity = Vector3.zero;
        
        public void Initialize(
            Animator animator, Rigidbody rigidBody, NavMeshAgent navMeshAgent,
            float walkingSpeed, float runningSpeed)
        {
            _animator = animator;
            _rigidBody = rigidBody;
            _navMeshAgent = navMeshAgent;
            _camera = GetComponent<Camera>();
            
            _walkingSpeed = walkingSpeed;
            _runningSpeed = runningSpeed;

            _animator.applyRootMotion = false;

            if (rollingCurve.keys.Length == 0)
            {
                rollingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
                rollingCurve.postWrapMode = WrapMode.Once;
            }
        }
        
        private void FixedUpdate()
        { 
            if( IsRotate )
                RotatePlayer();
        }

        public void SetLock(bool isLocked)
        {
            Locked = isLocked;   
            
            if( Locked )
                LockMovement();
            else
                UnlockMovement();
        }

        public void SetRotate(Camera camera)
        {
            _camera = camera;
            IsRotate = true;
        }

        public void OnMove(float horizontal, float vertical, bool isRunning)
        {
            if (Locked)
                return;
            
            if (isRunning)
                _animator.SetFloat(AnimatorHash.AnimationSpeed, 1.5f);
            else
                _animator.SetFloat(AnimatorHash.AnimationSpeed, 1f);

            if (horizontal == 0 && vertical == 0)
            {
                _animator.SetBool(AnimatorHash.Moving, false);
                return;
            }

            // 이동 방향 계산
            _currentVelocity =
                Vector3.right * horizontal + Vector3.forward * vertical;

            var speed = isRunning ? _runningSpeed : _walkingSpeed;
            
            var point = _currentVelocity * (speed * Time.deltaTime);

            if (!_navMeshAgent.isActiveAndEnabled)
                return;

            _navMeshAgent.Move(point);

            // 월드 공간의 방향 벡터를 로컬 공간의 방향 벡터로 변환
            _animator.SetFloat(AnimatorHash.VelocityX, transform.InverseTransformDirection(_currentVelocity).x);
            _animator.SetFloat(AnimatorHash.VelocityZ, transform.InverseTransformDirection(_currentVelocity).z);
            _animator.SetBool(AnimatorHash.Moving, true);
        }

        public void OnRolling( HandlerContext handlerContext )
        {
            // if (Locked)
            //     return;

            if (IsRolling)
                return;

            _animator.SetFloat(AnimatorHash.AnimationSpeed, handlerContext.AnimationMultiplier);
            _animator.SetInteger(AnimatorHash.Action, 1);
            
            // 샘플 애니메이션에서 rolling trigger 값이 28
            _animator.SetInteger(AnimatorHash.TriggerNumber, 28);
            _animator.SetTrigger(AnimatorHash.Trigger);
            
            StartCoroutine(
                PlayerRolling(
                    handlerContext.Direction, 
                    handlerContext.AnimationDuration, 
                    handlerContext.AnimationMultiplier) );
        }

        private IEnumerator PlayerRolling(Vector3 direction, float duration, float multiplier = 1f)
        {
            if (!_navMeshAgent.isActiveAndEnabled)
                yield break;
            
            IsRolling = true;
            
            var elapsedTime = 0f;
            var startPosition = Vector3.zero;

            if (direction == Vector3.zero)
                direction = transform.forward;
            
            RotatePlayer_Directly(direction);
            
            var endPosition = direction * _rollingSpeed;

            duration /= multiplier;
            
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                var t = rollingCurve.Evaluate(elapsedTime / duration);
                _navMeshAgent.Move( Vector3.Lerp(startPosition, endPosition, t) * (Time.deltaTime * multiplier) );
                
                yield return null;
            }
            
            _animator.SetFloat(AnimatorHash.AnimationSpeed, 1);
            IsRolling = false;
        }
        
        private void RotatePlayer()
        {
            if (Locked)
                return;
            
            var mousePosition = Input.mousePosition;
            var objectPosition = transform.position;

            mousePosition = _camera.ScreenToWorldPoint(
                new Vector3(mousePosition.x, mousePosition.y, _camera.transform.position.y));

            // 캐릭터에서 마우스 방향으로의 벡터 계산
            var direction = mousePosition - objectPosition;
            direction.y = 0; // y축 회전 방지 (필요한 경우 이 줄을 제거하세요)

            // 회전 각도 계산
            var rotation = Quaternion.LookRotation(direction);

            // 회전 적용
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationSpeed * Time.deltaTime);
        }

        private void RotatePlayer_Directly(Vector3 direction)
        {
            var rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation;
        }
        

        private void LockMovement()
        {
            _currentVelocity = Vector3.zero;
            _animator.SetBool(AnimatorHash.Moving, false);
        }

        private void UnlockMovement()
        {
        }
    }
}