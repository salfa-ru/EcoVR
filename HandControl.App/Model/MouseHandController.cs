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
   
    [Range<float>(0.1F, 0.9F)]
    public float VerticalNonSensitiveZone { get; set; } = 0.2F;
    [Range<float>(0.1F, 0.9F)]
    public float HorizontalNonSensitiveZone { get; set; } = 0.2F;
    [Range<float>(0.1F, 0.9F)]
    public float ScrollNonsensitiveZone { get; set; } = 0.2F;
    
    [Range<float>(0.1F, 0.5F)]
    public float ActivationLeftMouseZone { get; set; } = 0.4F;

    [Range<float>(0.0F, 0.5F)]
    public float ActivationRightMouseZone { get; set; } = 0.3F;


    [Range<float>(0.7F, 4.0F)]
    public float HorizontalOffset { get; set; } = -0.1F;
    [Range<float>(-0.2F, 0.2F)]
    public float VerticalOffset { get; set; } = 0.0F;
    [Range<float>(-0.2F, 0.2F)]
    public float ScrollOffset { get; set; } = 0.0F;


    [Range<float>(0.1f, 1)]
    public float HorizontalAcseleration { get; set; } = 0.5F;

    [Range<float>(0.1f, 1)]
    public float VerticalAcseleration { get; set; } = 0.5F;

    [Range<float>(0.5f, 5)]
    public float ScrollAcseleration { get; set; } = 1F;


    [Range<float>(50, 200)]
    public float ScrollLimitSpeed { get; set; } = 100F;

    [Range<float>(10, 100)]
    public float HorizontalLimitSpeed { get; set; } = 50F;

    [Range<float>(10, 100)]
    public float VerticalLimitSpeed { get; set; } = 50F;


    public event EventHandler? OnMouseHandStateChanged;
   
    public bool IsMoveLeft
    {
        get => isMoveLeft;
        private set
        {
            if (isMoveLeft != value && IsAngleRight && _landmarks.IsNew)
            {
                isMoveLeft = value;
            }
            else
                isMoveLeft = false;
        }
    }
    public bool IsMoveRight
    {
        get => isMoveRight;
        private set
        {
            if (isMoveRight != value && IsAngleRight && _landmarks.IsNew)
            {
                isMoveRight = value;
            }
            else 
                isMoveRight  = false;
        }
    }
    public bool IsMoveUp
    {
        get => isMoveUp;
        private set
        {
            if (isMoveUp != value && IsAngleRight && _landmarks.IsNew)
            {
                isMoveUp = value;
            }
            else 
                isMoveUp = false;
        }
    }
    public bool IsMoveDown
    {
        get => isMoveDown;
        private set
        {
            if (isMoveDown != value && IsAngleRight && _landmarks.IsNew)
            {
                isMoveDown = value;
            }
            else
                isMoveDown = false;
        }
    }
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

    public MouseHandController()
    {
        _cursor = SingleManager.CursorApi;
        _cursor.MoveRelative(0.5, 0.5);
        _landmarks = SingleManager.LandmarksModel;
        Task.Run(Mover());
        Task.Run(Scroller());
        _landmarks.OnDataChanged += (s, a) =>
        {
            IsAngleRight = CheckAngle(_landmarks.IndexMcp.Point, _landmarks.PinkyMcp.Point, _landmarks.Wrist.Point);
            MoveVertical();
            MoveHorizontal();
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

                if (IsAngleRight && _landmarks.IsNew)
                {
                    //MouseSpeedController();
                    MouseOneHandMover();
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

        float width = _cursor.ScreenWidth * 2.6F;
        float height = _cursor.ScreenHeight * 2.6F;

        center.X = center.X * width - _cursor.ScreenWidth * 0.8F;
        center.Y = center.Y * height - _cursor.ScreenHeight * 0.8F;
     
        int zoneVertical = 10;
        int zoneHorizontal = 10;

        speedX = 0;
        if (MathF.Abs(center.X - _cursor.Position.X) > zoneHorizontal)
        {
            speedX = (center.X - _cursor.Position.X) / 5;
        }
        speedY = 0;
        if (MathF.Abs(center.Y - _cursor.Position.Y) > zoneVertical)
        {
            speedY = (center.Y - _cursor.Position.Y) / 5;
        }

        IsMoveLeft = speedX < 0;
        IsMoveRight = speedX > 0;
        IsMoveUp = speedY < 0;
        IsMoveDown = speedY > 0;

        _cursor.Move(speedX, speedY);

    }

    private void MouseSpeedController()
    {
        if (IsMoveRight)
        {
            if (speedX < 0) speedX = 0;
            speedX += HorizontalAcseleration;
            if (speedX > HorizontalLimitSpeed) speedX = HorizontalLimitSpeed;
        }
        else if (IsMoveLeft)
        {
            if (speedX > 0) speedX = 0;
            speedX -= HorizontalAcseleration;
            if (speedX < -HorizontalLimitSpeed) speedX = -HorizontalLimitSpeed;
        }
        else speedX = 0f;

        if (IsMoveDown)
        {
            if (speedY < 0) speedY = 0;
            speedY += VerticalAcseleration;
            if (speedY > VerticalLimitSpeed) speedY = VerticalLimitSpeed;
        }
        else if (IsMoveUp)
        {
            if (speedY > 0) speedY = 0;
            speedY -= VerticalAcseleration;
            if (speedY < -VerticalLimitSpeed) speedY = -VerticalLimitSpeed;
        }
        else speedY = 0f;
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
        var dist = GetDistance(trigger1.Point, trigger2.Point);
        var zone = ControllMeassure * ActivationRightMouseZone;
        MouseRightDownTrigger = MouseRightDownTrigger ? dist < zone + ControllMeassure * HISTEREZIS : dist < zone - ControllMeassure * HISTEREZIS;
    }

    private void TriggerLeftMouse()
    {
        Landmark? trigger1 = GetLandmark(LeftButtonTrigger1);
        Landmark? trigger2 = GetLandmark(LeftButtonTrigger2);
        if (trigger1 == null || trigger2 == null) return;
        var dist = GetDistance(trigger1.Point, trigger2.Point);
        var zone = ControllMeassure * ActivationLeftMouseZone;
        MouseLeftDownTrigger = MouseLeftDownTrigger ? dist < zone + ControllMeassure * HISTEREZIS : dist < zone - ControllMeassure * HISTEREZIS;
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

    private void MoveHorizontal()
    {
        Landmark? remote = GetLandmark(HorizontalRemote);
        Landmark? main = GetLandmark(HorizontalMain);
        if (remote == null || main == null) return;
        var diff = main.Point.X - remote.Point.X + HorizontalOffset * ControllMeassure;
        var zone = (float)(HorizontalNonSensitiveZone * ControllMeassure);
        IsMoveRight = Math.Abs(diff) - zone > ControllMeassure * HISTEREZIS && diff > 0;
        IsMoveLeft = Math.Abs(diff) - zone > ControllMeassure * HISTEREZIS && diff < 0;
    }

    private void MoveVertical()
    {
        Landmark? remote = GetLandmark(VerticalRemote);
        Landmark? main = GetLandmark(VerticalMain);
        if (remote == null || main == null) return;
        var diff = remote.Point.Y - main.Point.Y + VerticalOffset * ControllMeassure;
        var zone = (float)(VerticalNonSensitiveZone * ControllMeassure);
        IsMoveUp = Math.Abs(diff) - zone > ControllMeassure * HISTEREZIS && diff < 0;
        IsMoveDown = Math.Abs(diff) - zone > ControllMeassure * HISTEREZIS && diff > 0;
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