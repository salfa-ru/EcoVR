namespace CursorRemote.Library.Mouse
{
    /// <summary>
    /// аргументы на события по скроллингу
    /// </summary>
    public class MouseEventScrollArgs
    {
        /// <summary>
        /// положение по горизонтали
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// положение по вертикали
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// напраление скроллинга
        /// </summary>
        public MouseScrollDirection Direction { get; set; }
        /// <summary>
        /// величина скроллинга
        /// </summary>
        public int Distance { get; set; }

    }
}