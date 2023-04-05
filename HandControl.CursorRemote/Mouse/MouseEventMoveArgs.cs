namespace HandControl.CursorRemote.Mouse;

/// <summary>
/// Аргументы события на перемещения  курсора 
/// </summary>
public record MouseEventMoveArgs
{
    /// <summary>
    /// положение по горизонтали
    /// </summary>
    public int X { get; init; }
    /// <summary>
    /// положение по вертикали
    /// </summary>
    public int Y { get; init; }
}
