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
    
    public int SelectedTarget { get; set; }

    public Landmark LandmarkRemote
    {
        get
        {
            return SelectedFinger switch
            {
                0 => SingleManager.LandmarksModel.ThumbTip,
                1 => SingleManager.LandmarksModel.IndexTip,
                2 => SingleManager.LandmarksModel.MiddleTip,
                3 => SingleManager.LandmarksModel.RingTip,
                4 => SingleManager.LandmarksModel.PinkyTip,
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
                0 => SingleManager.LandmarksModel.ThumbIp,
                1 => SingleManager.LandmarksModel.IndexDip,
                2 => SingleManager.LandmarksModel.MiddleDip,
                3 => SingleManager.LandmarksModel.RingDip,
                4 => SingleManager.LandmarksModel.PinkyDip,
                _ => SingleManager.LandmarksModel.IndexDip
            };
        }
    }

    private bool CheckDistanceTriggerFar => GetDistance(LandmarkRemote.Point, LandmarkBase.Point) > ControllMeassure * Histerezis;

    private bool CheckDistanceTriggerLess => GetDistance(LandmarkRemote.Point, LandmarkBase.Point) < ControllMeassure * Histerezis;

    private bool CheckVerticalTriggerFar => LandmarkRemote.Point.Y - LandmarkBase.Point.Y < ControllMeassure * Histerezis;

    private bool CheckHorizontalTriggerFar => LandmarkRemote.Point.X - LandmarkBase.Point.X > ControllMeassure * Histerezis;

    private bool CheckVerticalTriggerLess => LandmarkRemote.Point.Y - LandmarkBase.Point.Y > ControllMeassure * Histerezis;

    private bool CheckHorizontalTriggerLess => LandmarkRemote.Point.X - LandmarkBase.Point.X < ControllMeassure * Histerezis;

    private float GetDistance(PointF p1, PointF p2) => (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));

    public bool Trigger => SelectedTarget switch
    {
        0 => CheckHorizontalTriggerLess,
        1 => CheckVerticalTriggerLess,
        2 => CheckHorizontalTriggerFar,
        3 => CheckVerticalTriggerFar,
        4 => CheckDistanceTriggerLess,
        5 => CheckDistanceTriggerFar,
        _ => CheckVerticalTriggerLess
    };

    public float ControllMeassure => GetDistance(SingleManager.LandmarksModel.Wrist.Point, SingleManager.LandmarksModel.IndexMcp.Point);

    public ButtonTrigger()
    {
        Fingers = new List<string>() {
            "Большой палец",
            "Указательный палец",
            "Средний палец",
            "Безымянный палец",
            "Мизинец",
        };

        TriggerTarget = new List<string>()
        {
            "сближение по горизонтали",
            "сближение по вертикали",
            "расхождение по горизонтали",
            "расхождение по вертикали",
            "сближение",
            "расхождение",
        };
    }
}

