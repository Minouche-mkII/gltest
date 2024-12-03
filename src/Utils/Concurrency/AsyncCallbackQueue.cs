namespace gltest.Utils.Concurrency;

public class AsyncCallbackQueue()
{
    private readonly List<Action> _instructionsQueue = [];

    public void AddInstructionsToQueue(Action instructions)
    {
        _instructionsQueue.Add(instructions);
    }

    public void ExecuteWaitingInstructions()
    {
        foreach (var instructions in _instructionsQueue.ToList())
        {
            instructions();
            _instructionsQueue.Remove(instructions);
        }
    }
}