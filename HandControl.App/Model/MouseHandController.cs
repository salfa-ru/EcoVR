using HandControl.CursorRemote;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using HandControl.App.Loger;


namespace HandControl.App.Model;

public class MouseHandController
{
    private const int DELAY = 1;
    private CursorApi _cursor;
    private LandmarksModel _landmarks;
    private bool isMoveLeft = false;
    private bool isMoveRight = false;
    private bool isMoveUp = false;
    private bool isMoveDown = false;
    private float speedX = 0f;
    private float speedY = 0f;
    public event EventHandler? OnMouseHandStateChanged;

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
            if (isMoveLeft != value && IsAngleRight)
                isMoveLeft = value;
        }
    }

    public bool IsMoveRight
    {
        get => isMoveRight;
        private set
        {
            if (isMoveRight != value && IsAngleRight)
                isMoveRight = value;
        }
    }

    public bool IsMoveUp
    {
        get => isMoveUp;
        private set
        {
            if (isMoveUp != value && IsAngleRight)
                isMoveUp = value;
        }
    }

    public bool IsMoveDown
    {
        get => isMoveDown;
        private set
        {
            if (isMoveDown != value && IsAngleRight)
                isMoveDown = value;
        }
    }

    #endregion

    public ButtonTrigger LeftTrigger { get; }
    public ButtonTrigger RightTrigger { get; }


    public MouseHandController()
    {
        LeftTrigger = new ButtonTrigger();
        RightTrigger = new ButtonTrigger();

        _cursor = SingleManager.CursorApi;
        _landmarks = SingleManager.LandmarksModel;
        Task.Run(Mover());
        Task.Run(Trigging());
        _landmarks.OnDataChanged += (s, a) =>
        {
            IsAngleRight = CheckAngle(_landmarks.IndexMcp.Point, _landmarks.PinkyMcp.Point, _landmarks.Wrist.Point);
            OnMouseHandStateChanged?.Invoke(this, EventArgs.Empty);
        };
    }

    private Func<Task?> Trigging()
    {
        return async () =>
        {
            bool _lastLeft = false;
            bool _lastRight = false;
            while (true)
            {
                try
                {
                    await Task.Delay(DELAY * 5);
                    if (!_landmarks.IsNew)
                    {
                        _lastRight = _lastLeft = false;
                        continue;
                    }

                    if (_lastLeft != LeftTrigger.Trigger)
                    {
                        _lastLeft = LeftTrigger.Trigger;
                        if (_lastLeft)
                        {
                            _cursor.SetMouseLeftDown();
                        }
                        else
                        {
                            _cursor.SetMouseLeftUp();
                        }
                    }

                    if (_lastRight != RightTrigger.Trigger)
                    {
                        _lastRight = RightTrigger.Trigger;
                        if (_lastRight)
                        {
                            _cursor.SetMouseRightClick();
                        }
                    }
                }
                catch (Exception exc) 
                {
                    ErrorLoger.Log(exc.Message);
                }
            }
        };
    }


    private Func<Task?> Mover()
    {
        return async () =>
        {
            List<Landmark> lms;
            float left, right, top, bottom, width, height;
            PointF center;
            while (true)
            {
                await Task.Delay(DELAY);

                try
                {
                    if (!_landmarks.IsNew)
                    {
                        IsMoveLeft = IsMoveRight = IsMoveUp = IsMoveDown = false;
                        continue;
                    }
                    else
                    {
                        lms = _landmarks.LandmarksRelative;
                        left = lms.Select(s => s.Point.X).Min();
                        right = lms.Select(s => s.Point.X).Max();
                        top = lms.Select(s => s.Point.Y).Min();
                        bottom = lms.Select(s => s.Point.Y).Max();
                        center = new((right + left) / 2, (bottom + top) / 2);
                        width = _cursor.ScreenWidth * (1.0F + 2 * EdgeOffset);
                        height = _cursor.ScreenHeight * (1.0F + 2 * EdgeOffset);

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
                }
                catch (Exception exc)
                {
                    ErrorLoger.Log(exc.Message);
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
    public bool IsAngleRight { get; private set; }
}