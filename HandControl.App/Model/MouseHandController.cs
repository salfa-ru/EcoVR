using HandControl.CursorRemote;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;


namespace HandControl.App.Model;

public class MouseHandController
{
    private const double HISTEREZIS = 0.1;
    private const int DELAY = 10;
    private CursorApi _cursor;
    private LandmarksModel _landmarks;
    private bool mouseLeftDownTrigger = false;
    private bool mouseRightDownTrigger = false;
    private bool isMoveLeft = false;
    private bool isMoveRight = false;
    private bool isMoveUp = false;
    private bool isMoveDown = false;
    private float speedX = 0f;
    private float speedY = 0f;
    private float speedScroll = 0f;
    private bool isScrollUp = false;
    private bool isScrollDown = false;
   public event EventHandler? OnMouseHandStateChanged;


    #region scroll
    [Range<float>(0.1F, 0.9F)]
    public float ScrollNonsensitiveZone { get; set; } = 0.2F;

    public float ScrollOffset { get; set; } = 0.0F;

    [Range<float>(0.5f, 5)]
    public float ScrollAcseleration { get; set; } = 1F;

    [Range<float>(50, 200)]
    public float ScrollLimitSpeed { get; set; } = 100F;    
    public bool IsScrollUp
    {
        get => isScrollUp;
        private set { if (IsAngleRight && _landmarks.IsNew) isScrollUp = value; }
    }

    public bool IsScrollDown
    {
        get => isScrollDown;
        private set { if (IsAngleRight && _landmarks.IsNew) isScrollDown = value; }
    }
    #endregion

    #region moving
    [Range<int>(5, 50)]
    public int NonSensitiveZone { get; set; } = 10;

    [Range<float>(0.1F, 1.0F)]
    public float EdgeOffset { get; set; } = 0.8F;


    [Range<int>(1, 10)]
    public int Smoother { get; set; } = 5;

    public bool IsMoveLeft
    {
        get => isMoveLeft;
        private set
        {
            if (isMoveLeft != value && IsAngleRight && _landmarks.IsNew)
                isMoveLeft = value;
        }
    }

    public bool IsMoveRight
    {
        get => isMoveRight;
        private set
        {
            if (isMoveRight != value && IsAngleRight && _landmarks.IsNew)
                isMoveRight = value;
        }
    }

    public bool IsMoveUp
    {
        get => isMoveUp;
        private set
        {
            if (isMoveUp != value && IsAngleRight && _landmarks.IsNew)
                isMoveUp = value;
        }
    }

    public bool IsMoveDown
    {
        get => isMoveDown;
        private set
        {
            if (isMoveDown != value && IsAngleRight && _landmarks.IsNew)
                isMoveDown = value;
        }
    }

    #endregion

    #region button
    public bool MouseLeftDownTrigger
    {
        get => mouseLeftDownTrigger;
        private set
        {
            if (mouseLeftDownTrigger != value && IsAngleRight && _landmarks.IsNew)
            {
                mouseLeftDownTrigger = value;
                if (mouseLeftDownTrigger) _cursor.SetMouseLeftDown();
                else _cursor.SetMouseLeftUp();
            }
        }
    }

    public bool MouseRightDownTrigger
    {
        get => mouseRightDownTrigger;
        private set
        {
            if (mouseRightDownTrigger != value && IsAngleRight && _landmarks.IsNew)
            {
                mouseRightDownTrigger = value;
                if (mouseRightDownTrigger) _cursor.SetMouseRightDown();
                else _cursor.SetMouseRightUp();
            }
        }
    }

    [Range<float>(0.0F, 0.2F)]
    public float LeftTriggerHisterezis { get; set; } = 0.1F;

    [Range<float>(0.0F, 0.2F)]
    public float RightTriggerHisterezis { get; set; } = 0.1F;
    #endregion


    public MouseHandController()
    {
        _cursor = SingleManager.CursorApi;
        _landmarks = SingleManager.LandmarksModel;
        Task.Run(Mover());
        Task.Run(Scroller());
        _landmarks.OnDataChanged += (s, a) =>
        {
            IsAngleRight = CheckAngle(_landmarks.IndexMcp.Point, _landmarks.PinkyMcp.Point, _landmarks.Wrist.Point);
            TriggerLeftMouse();
            TriggerRightMouse();
            Scrolling();
            OnMouseHandStateChanged?.Invoke(this, EventArgs.Empty);
        };
    }

    private Func<Task?> Scroller()
    {
        return async () =>
        {
            while (true)
            {
                await Task.Delay(DELAY);
                if (IsAngleRight && _landmarks.IsNew)
                {
                    if (IsScrollDown)
                    {
                        if (speedScroll < 0) speedScroll = 0;
                        speedScroll += ScrollAcseleration;
                        if (speedScroll > ScrollLimitSpeed) speedScroll = ScrollLimitSpeed;
                    }
                    else if (IsScrollUp)
                    {
                        if (speedScroll > 0) speedScroll = 0;
                        speedScroll -= ScrollAcseleration;
                        if (speedScroll < -ScrollLimitSpeed) speedScroll = -ScrollLimitSpeed;
                    }
                    else speedScroll = 0;
                    //_cursor.SetMouseScroll((int)speedScroll);
                }
            }
        };
    }
    private Func<Task?> Mover()
    {
        return async () =>
        {
            while (true)
            {
                await Task.Delay(DELAY);

                if (IsAngleRight && _landmarks.IsNew && _landmarks.PinkyTip.Point.Y < _landmarks.PinkyMcp.Point.Y)
                {
                    MouseOneHandMover();
                }
                else
                {
                    IsMoveDown = IsMoveUp = IsMoveLeft = IsMoveRight = MouseLeftDownTrigger = MouseRightDownTrigger = false;
                }
            }
        };
    }

    private void MouseOneHandMover()
    {
        var lms = _landmarks.LandmarksRelative;
        var left = lms.Select(s => s.Point.X).Min();
        var right = lms.Select(s => s.Point.X).Max();
        var top = lms.Select(s => s.Point.Y).Min();
        var bottom = lms.Select(s => s.Point.Y).Max();
        PointF center = new PointF((right + left) / 2, (bottom + top) / 2);

        float width = _cursor.ScreenWidth * (1.0F + 2 * EdgeOffset);
        float height = _cursor.ScreenHeight * (1.0F + 2 * EdgeOffset);

        center.X = center.X * width - _cursor.ScreenWidth * EdgeOffset;
        center.Y = center.Y * height - _cursor.ScreenHeight * EdgeOffset;

        if (MathF.Abs(center.X - _cursor.Position.X) > NonSensitiveZone)
            speedX = (center.X - _cursor.Position.X) / Smoother;
        else
            speedX = 0;

        if (MathF.Abs(center.Y - _cursor.Position.Y) > NonSensitiveZone)
            speedY = (center.Y - _cursor.Position.Y) / Smoother;
        else
            speedY = 0;

        IsMoveLeft = speedX < 0 && _landmarks.IsNew;
        IsMoveRight = speedX > 0 && _landmarks.IsNew;
        IsMoveUp = speedY < 0 && _landmarks.IsNew;
        IsMoveDown = speedY > 0 && _landmarks.IsNew;
        _cursor.Move(speedX, speedY);
    }

    private bool CheckAngle(PointF pt1, PointF pt2, PointF center)
    {
        (float lenX, float lenY) AB = (pt1.X - center.X, pt1.Y - center.Y);
        (float lenX, float lenY) AC = (pt2.X - center.X, pt2.Y - center.Y);
        var scalar = AB.lenX * AC.lenX + AB.lenY * AC.lenY;
        var ABLength = MathF.Sqrt(MathF.Pow(AB.lenX, 2) + MathF.Pow(AB.lenY, 2));
        var ACLength = MathF.Sqrt(MathF.Pow(AC.lenX, 2) + MathF.Pow(AC.lenY, 2));
        var cos = scalar / (ABLength * ACLength);
        var radian = MathF.Acos(cos);
        var deg = radian * 180 / MathF.PI;
        return deg > 30 && deg < 70;
    }
    private float GetDistance(PointF p1, PointF p2) => (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
    public float ControllMeassure => GetDistance(_landmarks.Wrist.Point, _landmarks.IndexMcp.Point);
    public bool IsAngleRight { get; private set; }

    private void TriggerRightMouse()
    {
        Landmark? trigger1 = GetLandmark(RightButtonTrigger1);
        Landmark? trigger2 = GetLandmark(RightButtonTrigger2);
        if (trigger1 == null || trigger2 == null) return;
        var dist = trigger2.Point.Y - trigger1.Point.Y;
        MouseRightDownTrigger = MouseRightDownTrigger ? dist < RightTriggerHisterezis * ControllMeassure : dist < 0;
    }

    private void TriggerLeftMouse()
    {                                                              
        Landmark? trigger1 = GetLandmark(LeftButtonTrigger1);     
        Landmark? trigger2 = GetLandmark(LeftButtonTrigger2);
        if (trigger1 == null || trigger2 == null) return;
        var dist = trigger2.Point.Y - trigger1.Point.Y;
        MouseLeftDownTrigger = MouseLeftDownTrigger ? dist < LeftTriggerHisterezis * ControllMeassure : dist < 0 ;
    }

    private void Scrolling()
    {
        Landmark? remote = GetLandmark(ScrollRemote);
        Landmark? main = GetLandmark(ScrollMain);
        if (remote == null || main == null) return;
        var diff = remote.Point.Y - main.Point.Y + ScrollOffset * ControllMeassure;
        var zone = (float)(ScrollNonsensitiveZone * ControllMeassure);
        IsScrollUp = Math.Abs(diff) - zone > ControllMeassure * HISTEREZIS && diff < 0;
        IsScrollDown = Math.Abs(diff) - zone > ControllMeassure * HISTEREZIS && diff > 0;
    }

    private Landmark? GetLandmark(Mark mark) => _landmarks.Landmarks.FirstOrDefault(l => l.Mark == mark);

    public Mark HorizontalRemote { get; set; } = Mark.ThumbTip;
    public Mark HorizontalMain { get; set; } = Mark.ThumbMcp;
    public Mark VerticalRemote { get; set; } = Mark.MiddleTip;
    public Mark VerticalMain { get; set; } = Mark.MiddlePip;
    public Mark ScrollRemote { get; set; } = Mark.PinkyTip;
    public Mark ScrollMain { get; set; } = Mark.PinkyPip;

    public Mark LeftButtonTrigger1 { get; set; } = Mark.IndexTip;
    public Mark LeftButtonTrigger2 { get; set; } = Mark.ThumbTip;
    public Mark RightButtonTrigger1 { get; set; } = Mark.RingTip;
    public Mark RightButtonTrigger2 { get; set; } = Mark.ThumbTip;

}