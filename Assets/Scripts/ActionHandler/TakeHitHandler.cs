using System;
using ActionHandler.Data;
using Controller;

namespace ActionHandler
{
    public class TakeHitHandler : ActionHandlerBase
    {
        public TakeHitHandler(ActorControllerBase controller, Action<HandlerContext> animationCallback) : base(controller, animationCallback)
        {
        }

        protected override void StartAction_Internal()
        {
            AnimationCallback?.Invoke(Context);
        }

        protected override void EndAction_Internal()
        {
        }
    }
}