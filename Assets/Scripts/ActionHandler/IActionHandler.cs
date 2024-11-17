using ActionHandler.Data;

namespace ActionHandler
{
    public interface IActionHandler
    {
        void StartAction(HandlerContext context = null);
        void EndAction();
    }
}