using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
namespace HandControl.App.Model;

public class LandmarksModel
{
    public event EventHandler? OnDataChanged;
    public List<Landmark> Landmarks { get; private set; } = new();
    public List<Landmark> LandmarksRelative { get; private set; } = new();

    #region Landmark
    public Landmark Wrist => GetLandmark(0);
    public Landmark ThumbCmc => GetLandmark(1);
    public Landmark ThumbMcp => GetLandmark(2);
    public Landmark ThumbIp => GetLandmark(3);
    public Landmark ThumbTip => GetLandmark(4);
    public Landmark IndexMcp => GetLandmark(5);
    public Landmark IndexPip => GetLandmark(6);
    public Landmark IndexDip => GetLandmark(7);
    public Landmark IndexTip => GetLandmark(8);
    public Landmark MiddleMcp => GetLandmark(9);
    public Landmark MiddlePip => GetLandmark(10);
    public Landmark MiddleDip => GetLandmark(11);
    public Landmark MiddleTip => GetLandmark(12);
    public Landmark RingMcp => GetLandmark(13);
    public Landmark RingPip => GetLandmark(14);
    public Landmark RingDip => GetLandmark(15);
    public Landmark RingTip => GetLandmark(16);
    public Landmark PinkyMcp => GetLandmark(17);
    public Landmark PinkyPip => GetLandmark(18);
    public Landmark PinkyDip => GetLandmark(19);
    public Landmark PinkyTip => GetLandmark(20); 
    #endregion

    private DateTime _lastUpdateTime = DateTime.MinValue;
    public bool IsNew => (DateTime.Now - _lastUpdateTime).TotalMilliseconds < 500;
    private Landmark GetLandmark(int index) => Landmarks[index];

    public LandmarksModel()
    {
        var lm = SingleManager.LandmarksData;
        lm.OnLandmarksChanged += (s, a) =>
        {
            if (lm.Hand != null)
            {
                Landmarks = lm.Hand.Points.Select((item, index) => new Landmark((Mark)index, item)).ToList();
                LandmarksRelative = lm.Hand.Points.Select((item, index) => new Landmark((Mark)index, new PointF(1 - item.X / (float)(lm.Frame?.Width ?? 640), item.Y / (float)(lm.Frame?.Height ?? 480)))).ToList();
            }
            OnDataChanged?.Invoke(this, EventArgs.Empty);
            _lastUpdateTime = DateTime.Now;
        };
    }
}
