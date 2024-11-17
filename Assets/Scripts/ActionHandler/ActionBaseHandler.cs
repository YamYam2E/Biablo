using System;
using ActionHandler.Data;
using Controller;

namespace ActionHandler
{
    public abstract class ActionBaseHandler : IActionHandler
    {
        public ActorControllerBase Controller { get; protected set; }
        public HandlerContext Context { get; protected set; }
        public bool IsActive { get; protected set; } = true;
        
        protected abstract void StartAction_Internal();

        protected abstract void EndAction_Internal();

        protected Action<HandlerContext> AnimationCallback;
        
        public ActionBaseHandler(ActorControllerBase controller, Action<HandlerContext> animationCallback )
        {
            Controller = controller;
            AnimationCallback = animationCallback;
        }

        public void StartAction(HandlerContext context = default)
        {
            if (!Controller.AbleToAction)
                return;
            
            if (!IsActive)
                return;

            Context = context ?? new HandlerContext();
            
            StartAction_Internal();
        }

        public void EndAction()
        {
            if (!IsActive)
                return;

            EndAction_Internal();
        }
    }
}