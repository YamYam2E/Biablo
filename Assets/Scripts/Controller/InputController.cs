using Common;
using UnityEngine;
using UnityEngine.Events;

namespace Controller
{
    /// <summary>
    /// Class for character (keyboard, mouse, etc) input control
    /// </summary>
    public class InputController : MonoBehaviour
    {
        public UnityEvent<float, float, bool> OnMovementEvent = new();
        public UnityEvent OnAttackEvent = new();
        public UnityEvent<Vector3> OnRollingEvent = new();
        public UnityEvent<EWeaponType> OnWeaponSwitchEvent = new();
        
        public bool IsAbleInput { get; private set; } = true;
        public void SetAbleInput(bool isAbleInput) => IsAbleInput = isAbleInput;

        private float _horizontalInput;
        private float _verticalInput;
        private bool _isRunning;
        
        private void Update()
        {
            if (!IsAbleInput)
                return;
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                OnWeaponSwitchEvent.Invoke(EWeaponType.Punch);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                OnWeaponSwitchEvent.Invoke(EWeaponType.TwoHandSword);
            }
            
            
            Moving();
            Attacking();
            Rolling();
        }

        private void Moving()
        {
            _horizontalInput = Input.GetAxis("Horizontal");
            _verticalInput = Input.GetAxis("Vertical");
            _isRunning = Input.GetKey(KeyCode.LeftShift);
            
            OnMovementEvent.Invoke(_horizontalInput, _verticalInput, _isRunning);
        }

        private void Attacking()
        {
            if (!Input.GetButtonDown("Fire1"))
                return;
            
            OnAttackEvent.Invoke();
        }

        private void Rolling()
        {
            if (!Input.GetKeyDown(KeyCode.Space))
                return;

            var direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            
            OnRollingEvent.Invoke( direction.normalized );
        }
    }
}