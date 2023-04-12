using System.Drawing;

namespace HandControl.App.Model;

public record Landmark(Mark Mark, PointF Point)
{
    public override string ToString() => Mark.ToString();
}
