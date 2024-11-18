using System;
using ActionHandler.Data;
using Controller;

namespace ActionHandler
{
    public class RollingHandler : ActionHandlerBase
    {
        public RollingHandler(ActorControllerBase controller, Action<HandlerContext> animationCallback) 
            : base(controller, animationCallback)
        {
            IgnoreControllerLock = true;
        }

        protected override void StartAction_Internal()
        {
            Context.AnimationDuration = 1f;
            AnimationCallback?.Invoke(Context);
        }

        protected override void EndAction_Internal()
        {
        }
    }
}