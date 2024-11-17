using UnityEngine;
using UnityEngine.Events;

namespace Controller.Animation
{
    public class AnimationEventController : MonoBehaviour
    {
        public UnityEvent OnHit = new();
        public UnityEvent OnShoot = new();
        public UnityEvent OnFootR = new();
        public UnityEvent OnFootL = new();
        public UnityEvent OnLand = new();
        public UnityEvent OnWeaponSwitch = new();

        // Animation Events (Built-In animation frame)
        private void Hit() => OnHit.Invoke();
        private void Shoot() => OnShoot.Invoke();
        private void FootR() => OnFootR.Invoke();
        private void FootL() => OnFootL.Invoke();
        private void Land() => OnLand.Invoke();
        private void WeaponSwitch() => OnWeaponSwitch.Invoke();


        // 유니티 이벤트 함수로 이동처리를 할 수 있지만 InputEvent로 처리
        // private void OnAnimatorMove()
        // {
        //     Debug.Log($"Animator Move: {gameObject.name}");
        // }
    }
}