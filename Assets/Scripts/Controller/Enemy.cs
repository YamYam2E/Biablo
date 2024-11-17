using ActionHandler;
using ActionHandler.Data;
using Common;
using UnityEngine;
using UnityEngine.AI;

namespace Controller
{
    public class Enemy : ActorControllerBase
    {
        [SerializeField] private NavMeshObstacle navMeshObstacle;
        
        protected override void BindAnimationEvents()
        {
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