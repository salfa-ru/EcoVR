using System;

namespace HandControl.App.ReceivedDataHandlers;

public class LandmarksData
{
    private HandData? hand;
    public FrameData? Frame { get; set; }
    public HandData? Hand { get => hand; 
        set { 
            hand = value; 
            OnLandmarksChanged?.Invoke(this, EventArgs.Empty);
        } }
    public event EventHandler? OnLandmarksChanged;
}
