using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace HandControl.App.ReceivedDataHandlers;

public class HandData
{ 
    public RectangleF Box { get; set; } 
    public PointF Center { get; set; }
    public HandType Type { get; set; }
    public List<PointF> Points { get; private set; } = new();

    public void AddPoints(int[][] lmlist)
    {
        Points = lmlist.Select(item => new PointF(item[0], item[1])).ToList();
    }

}
