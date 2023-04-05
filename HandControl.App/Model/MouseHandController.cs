using HandControl.CursorRemote;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace HandControl.App.Model;

public class MouseHandController
{
    private const double HISTEREZIS = 0.1;
    private const int DELAY = 9;
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
    public float ControllMeassure => GetDistance(_landmarks.Wrist, _landmarks.IndexMcp);
    [RangeFloat(0.1F, 0.9F)]
    public float NonsensitiveZoneVerticalValue { get; set; } = 0.2F;
    [RangeFloat(0.1F, 0.9F)]
    public float NonsensitiveZoneHorizontalValue { get; set; } = 0.15F;
    [RangeFloat(0.7F, 4.0F)]
    public float HorizontalOffset { get; set; } = -0.1F;
    [RangeFloat(-0.2F, 0.2F)]
    public float VerticalOffset { get; set; } = 0.0F;

    [RangeFloat(0.1F, 0.5F)]
    public float ActivationLeftMouseZone { get; set; } = 0.4F;

    [RangeFloat(0.0F, 0.5F)]
    public float ActivationRightMouseZone { get; set; } = 0.3F;
    public bool IsRemote{ get; set; }

    [RangeFloat(0.1f, 1)]
    public float StepX { get; set; } = 0.5F;

    [RangeFloat(0.1f, 1)]
    public float StepY { get; set; } = 0.5F;

    [RangeFloat(10, 100)]
    public float LimitX { get; set; } = 50F;

    [RangeFloat(10, 100)]
    public float LimitY { get; set; } = 50F;

    public event EventHandler? OnMouseHandStateChanged;
    public bool IsMoveLeft {
        get => isMoveLeft;
        private set
        {
            if (isMoveLeft != value)
            {
                isMoveLeft = value;
            }
        }
    }
    public bool IsMoveRight
    {
        get => isMoveRight;
        private set
        {
            if (isMoveRight != value)
            {
                isMoveRight = value;
            }
        }
    }
    public bool IsMoveUp
    {
        get => isMoveUp;
        private set
        {
            if (isMoveUp != value)
            {
                isMoveUp = value;
            }
        }
    }
    public bool IsMoveDown
    {
        get => isMoveDown;
        private set
        {
            if (isMoveDown != value)
            {
                isMoveDown = value;
            }
        }
    }
    public bool MouseLeftDownTrigger
    {
        get => mouseLeftDownTrigger;
        private set
        {
            if (mouseLeftDownTrigger != value)
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
            if (mouseRightDownTrigger != value)
            {
                mouseRightDownTrigger = value;
                if (mouseRightDownTrigger) _cursor.SetMouseRightDown();
                else _cursor.SetMouseRightUp();
            }
        }
    }

    public MouseHandController()
    {
        _cursor = SingleManager.CursorApi;
        _cursor.MoveRelative(0.5, 0.5);
        _landmarks = SingleManager.LandmarksModel;
        Task.Run(CursorMover());
        _landmarks.OnDataChanged += (s, a) =>
        {
            IsRemote = CheckAngle(_landmarks.IndexMcp, _landmarks.PinkyMcp, _landmarks.Wrist);
            MoveVertical(_landmarks.MiddleTip, _landmarks.MiddlePip);
            MoveHorizontal(_landmarks.ThumbTip, _landmarks.ThumbMcp);
            TriggerLeftMouse(_landmarks.ThumbTip, _landmarks.IndexTip);
            TriggerRightMouse(_landmarks.RingTip, _landmarks.ThumbTip);
            OnMouseHandStateChanged?.Invoke(this, EventArgs.Empty);
        };
    }

    private Func<Task?> CursorMover()
    {
        return async () =>
        {
            while (true)
            {
                await Task.Delay(DELAY);

                if (IsRemote)
                {
                    if (IsMoveRight)
                    {
                        if (speedX < 0) speedX = 0;
                        speedX += StepX;
                        if (speedX > LimitX) speedX = LimitX;
                    }
                    else if (IsMoveLeft)
                    {
                        if (speedX > 0) speedX = 0;
                        speedX -= StepX;
                        if (speedX < -LimitX) speedX = -LimitX;
                    }
                    else speedX = 0f;

                    if (IsMoveDown)
                    {
                        if (speedY < 0) speedY = 0;
                        speedY += StepY;
                        if (speedY > LimitY) speedY = LimitY;
                    }
                    else if (IsMoveUp)
                    {
                        if (speedY > 0) speedY = 0;
                        speedY -= StepY;
                        if (speedY < -LimitY) speedY = -LimitY;
                    }
                    else speedY = 0f;
                    _cursor.Move((int)(_cursor.X + speedX), (int)(_cursor.Y + speedY));
                }
            }
        };
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

    private void TriggerRightMouse(PointF pt1, PointF pt2)
    {
        var dist = GetDistance(pt1, pt2);
        var zone = ControllMeassure * ActivationRightMouseZone;
        MouseRightDownTrigger = MouseRightDownTrigger ? dist < zone + ControllMeassure * HISTEREZIS : dist < zone - ControllMeassure * HISTEREZIS;
    }

    private void TriggerLeftMouse(PointF pt1, PointF pt2)
    {
        var dist = GetDistance(pt1, pt2);
        var zone = ControllMeassure * ActivationLeftMouseZone;
        MouseLeftDownTrigger = MouseLeftDownTrigger ? dist < zone + ControllMeassure * HISTEREZIS : dist < zone - ControllMeassure * HISTEREZIS;
    }

    private float GetDistance(PointF p1, PointF p2) => (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
    
    private void MoveHorizontal(PointF ptRemote, PointF ptMain)
    {
        var diff = ptMain.X - ptRemote.X + HorizontalOffset * ControllMeassure;
        var zone = (float)(NonsensitiveZoneHorizontalValue * ControllMeassure);
        IsMoveRight = Math.Abs(diff) - zone > ControllMeassure * HISTEREZIS && diff > 0;
        IsMoveLeft = Math.Abs(diff) - zone > ControllMeassure * HISTEREZIS && diff < 0;
    }

    private void MoveVertical(PointF ptRemote, PointF ptMain)
    {
        var diff = ptRemote.Y - ptMain.Y + VerticalOffset * ControllMeassure;
        var zone = (float)(NonsensitiveZoneVerticalValue * ControllMeassure);
        IsMoveUp = Math.Abs(diff) - zone > ControllMeassure * HISTEREZIS && diff < 0;
        IsMoveDown = Math.Abs(diff) - zone > ControllMeassure * HISTEREZIS && diff > 0;
    }
}