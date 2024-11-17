using System;
using System.Collections;
using Common;
using UnityEngine;

namespace Controller.Animation
{
    public class IKHandController : MonoBehaviour
    {
        public bool IsUsed { get; private set; }
        
        private float _leftHandPositionWeight;
        private float _leftHandRotationWeight;
        private Transform _attachLeft;
        private Transform _blendToTransform;
        
        private Animator _animator;
        private WeaponController _weaponController;

        private Coroutine _coroutine;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void Initialize(WeaponController weaponController)
        {
            _weaponController = weaponController;
        }

        public void SetIKOn(EWeaponType weaponType)
        {
            BlendIK(true, 0, 0, weaponType);
        }

        public void SetIKOff()
        {
            _leftHandPositionWeight = 0;
            _leftHandRotationWeight = 0;
        }
        
        public void BlendIK(bool isBlendOn, float delay, float timeToBlend, EWeaponType weaponType)
        {
            if (isBlendOn && weaponType == EWeaponType.TwoHandSword)
                IsUsed = true;

            if (IsUsed)
            {
                StopAllCoroutines();
                _coroutine = StartCoroutine( _BlendIK(isBlendOn, delay, timeToBlend, weaponType) );
            }
            
            if( !isBlendOn )
                IsUsed = false;
        }
        
        private IEnumerator _BlendIK(bool blendOn, float delay, float timeToBlend, EWeaponType weapon)
        {
            GetCurrentWeaponAttachPoint(weapon);
            
            yield return new WaitForSeconds(delay);
            
            var t = 0f;
            var blendTo = 0;
            var blendFrom = 0;

            if (blendOn)
            {
                blendTo = 1;
            }
            else
            {
                blendFrom = 1;
            }

            while (t < 1)
            {
                t += Time.deltaTime / timeToBlend;
                _attachLeft = _blendToTransform;
                _leftHandPositionWeight = Mathf.Lerp(blendFrom, blendTo, t);
                _leftHandRotationWeight = Mathf.Lerp(blendFrom, blendTo, t);
                yield return null;
            }
        }

        private void GetCurrentWeaponAttachPoint(EWeaponType weaponType)
        {
            switch (weaponType)
            {
                case EWeaponType.TwoHandSword:
                    // TODO: Test code : 현재 왼쪽 무기만 테스트
                    _blendToTransform = _weaponController.LeftWeapon.transform.GetChild(0).transform;
                    break;
            }
        }
        
        public void SetIKPause(float pauseTime)
        {
            if (!IsUsed) 
                return;
            
            StopAllCoroutines();
            _coroutine = StartCoroutine(_SetIKPause(pauseTime));
        }
        
        private IEnumerator _SetIKPause(float pauseTime)
        {
            var t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / 0.1f;
                _leftHandPositionWeight = Mathf.Lerp(1, 0, t);
                _leftHandRotationWeight = Mathf.Lerp(1, 0, t);
                yield return null;
            }

            yield return new WaitForSeconds(pauseTime - 0.2f);
            t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / 0.1f;
                _leftHandPositionWeight = Mathf.Lerp(0, 1, t);
                _leftHandRotationWeight = Mathf.Lerp(0, 1, t);
                yield return null;
            }
        }
        
        private void OnAnimatorIK(int layerIndex)
        {
            if (_weaponController == null ||_weaponController.LeftHand == null )
                return;
            
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, _leftHandPositionWeight);
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, _leftHandRotationWeight);

            if (_attachLeft == null)
                return;
            
            _animator.SetIKPosition(AvatarIKGoal.LeftHand, _attachLeft.position);
            _animator.SetIKRotation(AvatarIKGoal.LeftHand, _attachLeft.rotation);
        }
    }
}