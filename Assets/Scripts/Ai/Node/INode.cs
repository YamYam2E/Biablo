namespace Ai.Node
{
    public interface INode
    {
        public enum State
        {
            Running,
            Success,
            Failure,
        }

        public State Evaluate();
    }
}