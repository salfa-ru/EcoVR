namespace CursorRemote.Library.Mouse;

/// <summary>
/// аргументы на события по скроллингу
/// </summary>
public record MouseEventScrollArgs
{
    /// <summary>
    /// положение по горизонтали
    /// </summary>
    public int X { get; init; }
    /// <summary>
    /// положение по вертикали
    /// </summary>
    public int Y { get; init; }
    /// <summary>
    /// напраление скроллинга
    /// </summary>
    public MouseScrollDirection Direction { get; init; }
    /// <summary>
    /// величина скроллинга
    /// </summary>
    public int Distance { get; init; }

}
