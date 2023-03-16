using System;
using System.Runtime.InteropServices;
using CursorRemote.Library.Mouse;

namespace CursorRemote.Library
{

    /// <summary>
    /// Управление курсором мыши
    /// </summary>
    public class CursorApi
    {
        #region fields
        private int _x;
        private int _y;
        #endregion

        #region delegates
        /// <summary>
        /// делегат события действия кнопки мышки
        /// </summary>
        /// <param name="sender">Отправитель</param>
        /// <param name="mouse">Аргументы</param>
        public delegate void OnMouseButtonHandler(object sender, MouseEventButtonArgs mouseArgs);
        /// <summary>
        /// Делегат события скрола
        /// </summary>
        /// <param name="sender">Отправитель</param>
        /// <param name="mouseArgs">Аргументы</param>
        public delegate void OnMouseScrollHandler(object sender, MouseEventScrollArgs mouseArgs);
        /// <summary>
        /// Делегат события перемещения
        /// </summary>
        /// <param name="sender">Отправитель</param>
        /// <param name="mouseArgs">Аргументы</param>
        public delegate void OnMouseMoveHandler(object sender, MouseEventMoveArgs mouseArgs);
        #endregion

        #region events
        /// <summary>
        /// Событие при скроле
        /// </summary>
        public event OnMouseScrollHandler OnMouseScrollEvent;
        /// <summary>
        /// Событие при перемещении
        /// </summary>
        public event OnMouseMoveHandler OnMouseMoveEvent;
        /// <summary>
        /// Событие при нажатиии кнопки
        /// </summary>
        public event OnMouseButtonHandler OnMouseButtonDownEvent;
        /// <summary>
        /// Событие при отпускании кнопки
        /// </summary>
        public event OnMouseButtonHandler OnMouseButtonUpEvent;
        /// <summary>
        /// Событие при клике
        /// </summary>
        public event OnMouseButtonHandler OnMouseButtonClickEvent;
        /// <summary>
        /// Событие при клике
        /// </summary>
        public event OnMouseButtonHandler OnMouseButtonDoubleClickEvent;
        #endregion

        #region properties
        /// <summary>
        /// Разрешение экрана по ширине
        /// </summary>
        public int ScreenWidth => GetDeviceCaps(GetDC(IntPtr.Zero), DESKTOP_HORZRES);

        /// <summary>
        /// Разрешение экрана по высоте
        /// </summary>
        public int ScreenHeight => GetDeviceCaps(GetDC(IntPtr.Zero), DESKTOP_VERTRES);

        /// <summary>
        /// Позиция курсора по горизонтали
        /// </summary>
        public int X
        {
            get { return _x; }
            private set
            {
                if (value < 0) _x = 0;
                if (value > ScreenWidth) _x = ScreenWidth;
                _x = value;
                OnMouseMoveEvent?.Invoke(this, new MouseEventMoveArgs() { X = _x, Y = _y });
            }
        }

        /// <summary>
        /// Позиция курсора по вертикали
        /// </summary>
        public int Y
        {
            get { return _y; }
            private set
            {
                if (value < 0) _y = 0;
                if (value > ScreenHeight) _y = ScreenHeight;
                _y = value;
                OnMouseMoveEvent?.Invoke(this, new MouseEventMoveArgs() { X = _x, Y = _y });
            }
        }
        #endregion

        #region imports
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        private const int DESKTOP_HORZRES = 0x0076;
        private const int DESKTOP_VERTRES = 0x0075;
        private const uint LEFTDOWN = 0x0002;
        private const uint LEFTUP = 0x0004;
        private const uint MIDDLEDOWN = 0x0020;
        private const uint MIDDLEUP = 0x0040;
        private const uint RIGHTDOWN = 0x0008;
        private const uint RIGHTUP = 0x0010;
        private const uint MOUSEEVENTF_WHEEL = 0x0800;
        #endregion

        #region move methods
        /// <summary>
        /// Установить в курсор в позицию
        /// </summary>
        /// <param name="x"> по горизонтали </param>
        /// <param name="y"> по вертикали </param>
        public void Move(int x, int y)
        {
            X = x; Y = y;
            SetCursorPos(X, Y);
        }

        /// <summary>
        /// Установить курсор в позицию относительно разрешения экрана
        /// </summary>
        /// <param name="x">0...1 от разрешение экрана по горизонтали</param>
        /// <param name="y">0...1 от разрешение экрана по вертикали</param>
        public void MoveRelative(double x, double y)
        {
            X = (int)(x * ScreenWidth);
            Y = (int)(y * ScreenHeight);
            SetCursorPos(X, Y);
        }
        #endregion

        #region click methods
        /// <summary>
        /// Клик левой кнопкой мыши
        /// </summary>
        public void SetMouseLeftClick()
        {
            SetMouseLeftDown();
            SetMouseLeftUp();
            OnMouseButtonClickEvent?.Invoke(this, new MouseEventButtonArgs() { X = X, Y = Y, Button = MouseButtons.Left });
        }

        /// <summary>
        /// Клик правой кнопкой мыши
        /// </summary>
        public void SetMouseRightClick()
        {
            SetMouseRightDown();
            SetMouseRightUp();
            OnMouseButtonClickEvent?.Invoke(this, new MouseEventButtonArgs() { X = X, Y = Y, Button = MouseButtons.Right });
        }

        /// <summary>
        /// Клик средней кнопкой мыши
        /// </summary>
        public void SetMouseMiddleClick()
        {
            SetMouseMiddleDown();
            SetMouseMiddleUp();
            OnMouseButtonClickEvent?.Invoke(this, new MouseEventButtonArgs() { X = X, Y = Y, Button = MouseButtons.Middle });
        }
        #endregion

        #region double click methods
        /// <summary>
        /// Клик левой кнопкой мыши
        /// </summary>
        public void SetMouseDoubleLeftClick()
        {
            SetMouseLeftClick();
            SetMouseLeftClick();
            OnMouseButtonDoubleClickEvent?.Invoke(this, new MouseEventButtonArgs() { X = X, Y = Y, Button = MouseButtons.Left });
        }

        /// <summary>
        /// Клик правой кнопкой мыши
        /// </summary>
        public void SetMouseDoubleRightClick()
        {
            SetMouseRightClick();
            SetMouseRightClick();
            OnMouseButtonDoubleClickEvent?.Invoke(this, new MouseEventButtonArgs() { X = X, Y = Y, Button = MouseButtons.Right });
        }

        /// <summary>
        /// Клик средней кнопкой мыши
        /// </summary>
        public void SetMouseDoubleMiddleClick()
        {
            SetMouseMiddleClick();
            SetMouseMiddleClick();
            OnMouseButtonDoubleClickEvent?.Invoke(this, new MouseEventButtonArgs() { X = X, Y = Y, Button = MouseButtons.Middle });
        }
        #endregion

        #region scroll methods
        /// <summary>
        /// Скролл вверх
        /// </summary>
        /// <param name="distance">зависит от разрешения, количество единиц прокрутки</param>
        public void SetMouseUpScroll(int distance)
        {
            mouse_event(MOUSEEVENTF_WHEEL, 0, 0, (uint)(distance * -1), 0);
            OnMouseScrollEvent?.Invoke(this, new MouseEventScrollArgs() { X = X, Y = Y, Direction = MouseScrollDirection.Up, Distance = distance });

        }

        /// <summary>
        /// Скролл вниз
        /// </summary>
        /// <param name="amount">зависит от разрешения, количество единиц прокрутки</param>
        public void SetMouseDownScroll(int distance)
        {
            mouse_event(MOUSEEVENTF_WHEEL, 0, 0, (uint)(distance), 0);
            OnMouseScrollEvent?.Invoke(this, new MouseEventScrollArgs() { X = X, Y = Y, Direction = MouseScrollDirection.Down, Distance = distance });
        }
        #endregion

        #region down methods
        /// <summary>
        /// Нажатие левой кнопки мыши
        /// </summary>
        public void SetMouseLeftDown()
        {
            mouse_event(LEFTDOWN, (uint)X, (uint)Y, 0, 0);
            OnMouseButtonDownEvent?.Invoke(this,
                new MouseEventButtonArgs() { X = X, Y = Y, Button = MouseButtons.Left });
        }

        /// <summary>
        /// Нажатие правой кнопки мыши
        /// </summary>
        public void SetMouseRightDown()
        {
            mouse_event(RIGHTDOWN, (uint)X, (uint)Y, 0, 0);
            OnMouseButtonDownEvent?.Invoke(this,
                new MouseEventButtonArgs() { X = X, Y = Y, Button = MouseButtons.Right });
        }

        /// <summary>
        /// Нажатие средней кнопки мыши
        /// </summary>
        public void SetMouseMiddleDown()
        {
            mouse_event(MIDDLEDOWN, (uint)X, (uint)Y, 0, 0);
            OnMouseButtonDownEvent?.Invoke(this,
                new MouseEventButtonArgs() { X = X, Y = Y, Button = MouseButtons.Middle });
        }
        #endregion

        #region up methods
        /// <summary>
        /// Отпускание левой кнопки мыши
        /// </summary>
        public void SetMouseLeftUp()
        {
            mouse_event(LEFTUP, (uint)X, (uint)Y, 0, 0);
            OnMouseButtonUpEvent?.Invoke(this,
                new MouseEventButtonArgs() { X = X, Y = Y, Button = MouseButtons.Left });
        }

        /// <summary>
        /// Отпускание правой кнопки мыши
        /// </summary>
        public void SetMouseRightUp()
        {
            mouse_event(RIGHTUP, (uint)X, (uint)Y, 0, 0);
            OnMouseButtonUpEvent?.Invoke(this,
                new MouseEventButtonArgs() { X = X, Y = Y, Button = MouseButtons.Right });
        }

        /// <summary>
        /// Отпускание средней кнопки мыши
        /// </summary>
        public void SetMouseMiddleUp()
        {
            mouse_event(MIDDLEUP, (uint)X, (uint)Y, 0, 0);
            OnMouseButtonUpEvent?.Invoke(this,
                new MouseEventButtonArgs() { X = X, Y = Y, Button = MouseButtons.Middle });
        }
        #endregion
    }
}