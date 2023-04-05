using HandControl.App.Model;
using HandControl.App.ReceivedDataHandlers;
using HandControl.App.UdpServer;
using HandControl.CursorRemote;

namespace HandControl.App;

public static class SingleManager
{
    private static MouseHandController? _mouseHandController;
    private static StatusData? _statusData;
    private static LandmarksData? _data;
    private static LandmarksModel? _model;
    private static ConversationManager? _conversation;
    private static CursorApi? _cursorApi;

    public static ConversationManager ConversationManager
    {
        get
        {
            if (_conversation == null) _conversation = new ConversationManager();
            return _conversation;
        }
    }
    public static LandmarksData LandmarksData
    {
        get
        {
            if (_data == null)
            {
                _data = new LandmarksData();
            }
            return _data;
        }
    }
    public static StatusData StatusData
    {
        get
        {
            if (_statusData == null) _statusData = new StatusData();
            return _statusData;
        }
    }
    public static MouseHandController MouseHandController
    {
        get
        {
            if (_mouseHandController == null) 
                _mouseHandController = new(); 
            return _mouseHandController;
        }
    }
    public static CursorApi CursorApi
    {
        get
        {
            if (_cursorApi == null)
                _cursorApi = new();
            return _cursorApi;
        }
    }
    public static LandmarksModel LandmarksModel
    {
        get
        {
            if (_model == null)
                _model = new();
            return _model;
        }
    }
}
