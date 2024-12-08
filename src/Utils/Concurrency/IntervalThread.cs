using gltest.Utils.Concurrency;
using gltest.Utils.Logging;

namespace gltest.Utils;

public class IntervalThread
{
    public int Interval { get; set; }
    public bool IsRunning { get; set; }
    private readonly AsyncCallbackQueue _ponctualInstructions;
    private bool _continue;
    private readonly Thread _thread;
    private string? _slowedLogEntry;

    public IntervalThread(int interval, Action callBack)
    {
        _ponctualInstructions = new AsyncCallbackQueue();
        Interval = interval;
        _continue = true;
        IsRunning = true;
        _thread = new Thread(() =>
        {
            var lastInterval =  DateTimeOffset.Now.ToUnixTimeMilliseconds();
            while (_continue)
            {
                if (!IsRunning) continue;
                _ponctualInstructions.ExecuteWaitingInstructions();
                callBack();
                    
                try
                {
                    var timeSinceLastInterval = DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastInterval;
                    Thread.Sleep((int)(Interval-timeSinceLastInterval));
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    if(_slowedLogEntry != null) Log.Warning(_slowedLogEntry);
                }
            }
        });
    }

    public void SetLogWarningForWhenSlow(string entry)
    {
        _slowedLogEntry = entry;
    }

    public void ExecutePonctualInstructions(Action instructions)
    {
        _ponctualInstructions.AddInstructionsToQueue(instructions);
    }
    
    public void Start()
    {
        _thread.Start();
    }
    
    public void End()
    {
        _continue = false;
    }

    public void Pause()
    {
        IsRunning = false;
    }

    public void Resume()
    {
        IsRunning = true;
    }
    
}