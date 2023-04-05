using HandControl.App.Defaults;
using Newtonsoft.Json;
using System.Drawing;

namespace HandControl.App.ReceivedDataHandlers;

public class DataParser
{
    private StatusData _status;

    public DataParser()
    {
        _status = SingleManager.StatusData;
    }


    public void Parse(string data)
    {
        if (data.ToLower().Contains(Constants.STATUS)) ParseStatus(data);
        if (data.ToLower().Contains(Constants.DATA)) ParseData(data);
    }

    private void ParseData(string data)
    {
        var definition = new { 
            resolution = new { index = 0, width = 0, height = 0 }, 
            detection = new { 
                preview = false, 
                detect = false, 
                flip = false, 
                data = new {
                    lmlist = new int[0][], 
                    bbox= new int[0], 
                    center = new int[0], 
                    type = string.Empty } } };
        var deserialized = JsonConvert.DeserializeAnonymousType(data, definition);
        if (deserialized != null)
        {
            FrameData frame = new FrameData()
            {
                Width = deserialized.resolution.width,
                Height = deserialized.resolution.height,
                IsFlip = deserialized.detection.flip
            };
            RectangleF rect = new RectangleF(
                new PointF(
                    deserialized.detection.data.bbox[0],
                    deserialized.detection.data.bbox[1]),
                new SizeF(
                    deserialized.detection.data.bbox[2],
                    deserialized.detection.data.bbox[3]));
            PointF center = new PointF(
                deserialized.detection.data.center[0],
                deserialized.detection.data.center[1]);
            HandType handType = deserialized.detection.data.type == "left" && deserialized.detection.flip
                || deserialized.detection.data.type == "right" && !deserialized.detection.flip
                ? HandType.Right : HandType.Left;
            HandData hand = new HandData() { Box = rect, Center = center, Type = handType };
            hand.AddPoints(deserialized.detection.data.lmlist);

            SingleManager.LandmarksData.Frame = frame;
            SingleManager.LandmarksData.Hand = hand;
        }
    }

    private void ParseStatus(string data)
    {
        var definition = new { status = string.Empty};
        var deserialized = JsonConvert.DeserializeAnonymousType(data, definition);
        if (deserialized != null) 
        {
            _status.State = deserialized.status switch
            {
                Constants.PREPARE => StatusState.Prepare,
                Constants.STOPPED => StatusState.Stopped,
                Constants.CAPTURED => StatusState.Captured,
                _ => StatusState.Error
            };
        }
    }
}
