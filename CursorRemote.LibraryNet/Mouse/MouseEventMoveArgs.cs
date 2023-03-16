namespace CursorRemote.Library.Mouse
{ 

/// <summary>
/// Аргументы события на перемещения  курсора 
/// </summary>
    public class MouseEventMoveArgs
    {
        /// <summary>
        /// положение по горизонтали
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// положение по вертикали
        /// </summary>
        public int Y { get; set; }
    }
}
