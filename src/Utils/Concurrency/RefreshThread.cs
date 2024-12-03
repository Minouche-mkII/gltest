namespace gltest.Utils.Concurrency;

public class RefreshThread(int maxCallPerSeconds, Action callBack, Action? preliminaryInstruction = null)
    : IntervalThread(1000 / maxCallPerSeconds, callBack)
{
    private int _maxCallPerSeconds = maxCallPerSeconds;
    public int MaxCallPerSeconds
    {
        set
        {
            _maxCallPerSeconds = value;
            Interval = 1000 / _maxCallPerSeconds;
        }
        get => _maxCallPerSeconds;
    }
}