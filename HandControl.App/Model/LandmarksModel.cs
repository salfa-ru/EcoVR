using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace HandControl.App.Model;

public class LandmarksModel
{
    public event EventHandler? OnDataChanged;
    public List<PointF> _landmarks = new();

    public PointF Wrist => GetLandmark(0);
    public PointF ThumbCmc => GetLandmark(1);
    public PointF ThumbMcp => GetLandmark(2);
    public PointF ThumbIp => GetLandmark(3);
    public PointF ThumbTip => GetLandmark(4);
    public PointF IndexMcp => GetLandmark(5);
    public PointF IndexPip => GetLandmark(6);
    public PointF IndexDip => GetLandmark(7);
    public PointF IndexTip => GetLandmark(8);
    public PointF MiddleMcp => GetLandmark(9);
    public PointF MiddlePip => GetLandmark(10);
    public PointF MiddleDip => GetLandmark(11);
    public PointF MiddleTip => GetLandmark(12);
    public PointF RingMcp => GetLandmark(13);
    public PointF RingPip => GetLandmark(14);
    public PointF RingDip => GetLandmark(15);
    public PointF RingTip => GetLandmark(16);
    public PointF PinkyMcp => GetLandmark(17);
    public PointF PinkyPip => GetLandmark(18);
    public PointF PinkyDip => GetLandmark(19);
    public PointF PinkyTip => GetLandmark(20);

    private DateTime _lastUpdateTime = DateTime.MinValue;
    public bool IsNew => (DateTime.Now - _lastUpdateTime).TotalMilliseconds < 500;
    private PointF GetLandmark(int index) => _landmarks[index];

    public LandmarksModel()
    {
        var lm = SingleManager.LandmarksData;
        lm.OnLandmarksChanged += (s, a) =>
        {
            if (lm.Hand != null)
                _landmarks = lm.Hand.Points.Select(p => p).ToList();
            OnDataChanged?.Invoke(this, EventArgs.Empty);
            _lastUpdateTime = DateTime.Now;
        };
    }
}
