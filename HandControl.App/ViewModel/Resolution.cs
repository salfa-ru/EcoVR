using System.Collections.Generic;

namespace HandControl.App.ViewModel;

public class Resolution
{
    public int Width { get; set; }
    public int Height { get; set; }

    public override string ToString() => $"{Width} x {Height}";

    public static List<Resolution> GetResolutions()
    { 
        return new()
        { 
            new(){ Width = 320, Height = 240 },
            new(){ Width = 640, Height = 480 },
            new(){ Width = 800, Height = 600 },
            new(){ Width = 1024, Height = 768 },
            new(){ Width = 1280, Height = 720 },
            new(){ Width = 1280, Height = 1024 },
            new(){ Width = 1920, Height = 1080 },
        };

    }
}



