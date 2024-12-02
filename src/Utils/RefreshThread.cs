namespace gltest.Utils;

public class RefreshThread : IntervalThread
{
    private int _maxCallPerSeconds;
    public int MaxCallPerSeconds
    {
        set
        {
            _maxCallPerSeconds = value;
            Interval = 1000 / _maxCallPerSeconds;
        }
        get => _maxCallPerSeconds;
    }
    
    public RefreshThread(int maxCallPerSeconds, Action callBack) : base(1000/maxCallPerSeconds, callBack)
    {
        _maxCallPerSeconds = maxCallPerSeconds;
    }
}