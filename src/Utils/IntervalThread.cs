namespace gltest.Utils;

public class IntervalThread
{
    public int Interval { get; set; }
    public bool IsRunning { get; set; }
    private bool _continue;
    private readonly Thread _thread;
    private string? _slowedLogEntry;

    public IntervalThread(int interval, Action callBack)
    {
        Interval = interval;
        _continue = true;
        IsRunning = true;
        _thread = new Thread(() =>
        {
            while (_continue)
            {
                if (IsRunning)
                {
                    try
                    {
                        Thread.Sleep(Interval);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        if(_slowedLogEntry != null) Log.Warning(_slowedLogEntry);
                    }
                    callBack();
                }
            }
        });
    }

    public void SetLogWarningForWhenSlow(string entry)
    {
        _slowedLogEntry = entry;
    }
    
    public void Start()
    {
        _thread.Start();
    }

    public void Pause()
    {
        IsRunning = false;
    }

    public void End()
    {
        _continue = false;
    }

    public void Resume()
    {
        IsRunning = true;
    }
    
}