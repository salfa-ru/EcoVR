namespace CursorRemote.Library.Mouse
{
    /// <summary>
    /// аргументы событий действий с кнопками
    /// </summary>
    public class MouseEventButtonArgs
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
        /// кнопка на которую вызвано событие
        /// </summary>
        public MouseButtons Button { get; set; }
    }
}
