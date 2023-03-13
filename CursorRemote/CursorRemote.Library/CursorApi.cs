using System.Runtime.InteropServices;

namespace CursorRemote.Library
{
    /// <summary>
    /// Управление курсором мыши
    /// </summary>
    public class CursorApi
    {
        private int _x;
        private int _y;

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
                else if (value > ScreenWidth) _x = ScreenWidth;
                else _x = value;
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
                else if (value > ScreenHeight) _y = ScreenHeight;
                else _y = value;
            }
        }

        #region Imports
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, IntPtr dwExtraInfo);
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        private const int DESKTOP_HORZRES = 118;
        private const int DESKTOP_VERTRES = 117;
        private const uint LEFTDOWN = 0x0002;
        private const uint LEFTUP = 0x0004;
        private const uint MIDDLEDOWN = 0x0020;
        private const uint MIDDLEUP = 0x0040;
        private const uint RIGHTDOWN = 0x0008;
        private const uint RIGHTUP = 0x0010;
        private const uint MOUSEEVENTF_WHEEL = 0x0800;
        #endregion

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

        /// <summary>
        /// Клик левой кнопкой мыши
        /// </summary>
        public void SetMouseLeftClick()
        {
            SetMouseLeftDown();
            SetMouseLeftUp();
        }

        /// <summary>
        /// Клик правой кнопкой мыши
        /// </summary>
        public void SetMouseRightClick()
        {
            SetMouseRightDown();
            SetMouseRightUp();
        }

        /// <summary>
        /// Клик средней кнопкой мыши
        /// </summary>
        public void SetMouseMiddleClick()
        {
            SetMouseMiddleDown();
            SetMouseMiddleUp();
        }

        /// <summary>
        /// Скролл вверх
        /// </summary>
        /// <param name="amount">зависит от разрешения, количество единиц прокрутки</param>
        public void SetMouseDownScroll(int amount) => mouse_event(MOUSEEVENTF_WHEEL, 0, 0, (uint)(amount * -1), IntPtr.Zero);

        /// <summary>
        /// Скролл вниз
        /// </summary>
        /// <param name="amount">зависит от разрешения, количество единиц прокрутки</param>
        public void SetMouseUpScroll(int amount) => mouse_event(MOUSEEVENTF_WHEEL, 0, 0, (uint)(amount * 1), IntPtr.Zero);

        /// <summary>
        /// Нажатие левой кнопки мыши
        /// </summary>
        public void SetMouseLeftDown() => mouse_event(LEFTDOWN, (uint)X, (uint)Y, 0, 0);

        /// <summary>
        /// Отпускание левой кнопки мыши
        /// </summary>
        public void SetMouseLeftUp() => mouse_event(LEFTUP, (uint)X, (uint)Y, 0, 0);

        /// <summary>
        /// Нажатие правой кнопки мыши
        /// </summary>
        public void SetMouseRightDown() => mouse_event(RIGHTDOWN, (uint)X, (uint)Y, 0, 0);

        /// <summary>
        /// Отпускание правой кнопки мыши
        /// </summary>
        public void SetMouseRightUp() => mouse_event(RIGHTUP, (uint)X, (uint)Y, 0, 0);

        /// <summary>
        /// Нажатие средней кнопки мыши
        /// </summary>
        public void SetMouseMiddleDown() => mouse_event(MIDDLEDOWN, (uint)X, (uint)Y, 0, 0);

        /// <summary>
        /// Отпускание средней кнопки мыши
        /// </summary>
        public void SetMouseMiddleUp() => mouse_event(MIDDLEUP, (uint)X, (uint)Y, 0, 0);
    }
}