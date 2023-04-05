namespace HandControl.CursorRemote.Mouse;

/// <summary>
/// аргументы событий действий с кнопками
/// </summary>
public record MouseEventButtonArgs
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
    /// кнопка на которую вызвано событие
    /// </summary>
    public MouseButtons Button { get; init; }
}
