using System;
using System.Collections.Generic;
using System.Drawing;

namespace HandControl.App.Model;

public class ButtonTrigger
{
    [Range<float>(0.0F, 0.5F)]
    public float Histerezis { get; set; } = 0.1F;

    public List<string> Fingers { get; }
    
    public List<string> TriggerTarget { get; }

    public int SelectedFinger { get; set; }
    
    public int SelectedTarget { get; set; } = -1;

    public Landmark LandmarkRemote
    {
        get
        {
            return SelectedFinger switch
            {
                0 => SingleManager.LandmarksModel.IndexTip,
                1 => SingleManager.LandmarksModel.MiddleTip,
                _ => SingleManager.LandmarksModel.IndexTip
            };
        }
    }

    public Landmark LandmarkBase
    {
        get
        {
            return SelectedFinger switch
            {
                0 => SingleManager.LandmarksModel.IndexDip,
                1 => SingleManager.LandmarksModel.MiddleDip,
                _ => SingleManager.LandmarksModel.IndexDip
            };
        }
    }

    private bool CheckDistanceTriggerLess => GetDistance(SingleManager.LandmarksModel.ThumbTip.Point, LandmarkBase.Point) < ControllMeassure * Histerezis;

    private bool CheckVerticalTriggerLess => LandmarkRemote.Point.Y - LandmarkBase.Point.Y > 0;

    private float GetDistance(PointF p1, PointF p2) => (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));

    private bool trigger = false;

    public bool Trigger
    {
        get
        {
            if (SelectedTarget == -1) return false;
            
            bool result = SelectedTarget switch
            {
                0 => CheckVerticalTriggerLess,
                1 => CheckDistanceTriggerLess,
                _ => CheckDistanceTriggerLess,
            };
            if (trigger != result)
            {
                trigger = result;
            }
            return trigger;
        }
    }

    public float ControllMeassure => GetDistance(SingleManager.LandmarksModel.Wrist.Point, SingleManager.LandmarksModel.IndexMcp.Point);

    public ButtonTrigger()
    {
        Fingers = new List<string>() {
            "Указательный палец",
            "Средний палец"
        };

        TriggerTarget = new List<string>()
        {
            "наклон",
            "касание"
        };
    }
}

