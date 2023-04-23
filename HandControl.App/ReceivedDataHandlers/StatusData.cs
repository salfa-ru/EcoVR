using HandControl.App.Defaults;
using System;

namespace HandControl.App.ReceivedDataHandlers;

public class StatusData
{
    private StatusState state = StatusState.Stopped;

    public StatusState State
    {
        get => state;
        set
        {
            state = value;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public event EventHandler? OnStateChanged;

    public string Status => State switch
    {
        StatusState.Prepare => Constants.PREPARE_PRESENTABLE,
        StatusState.Stopped => Constants.STOPPED_PRESENTABLE,
        StatusState.Captured => Constants.CAPTURED_PRESENTABLE,
        StatusState.Error => Constants.ERROR_PRESENTABLE,
        _ => "",
    };
}
