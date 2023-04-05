using HandControl.App.UdpServer;
using System.Windows.Input;

namespace HandControl.App.ViewModel;

public class ControlPanelMainCommandViewModel : ControlPanelBaseViewModel
{
    public ICommand KillProccessCommand { get; init; } = null!;
    public ICommand CreateProccessCommand { get; init; } = null!;

    public ICommand StartDataSendingCommand { get; init; } = null!;
    public ICommand StopDataSendingCommand { get; init; } = null!;


    public ControlPanelMainCommandViewModel()
    {
        Title = "Управление камерой";

        ConversationManager conversation = SingleManager.ConversationManager;
        StartDataSendingCommand = new Command((_) => conversation.CommandStartDetectingData());
        StopDataSendingCommand = new Command((_) => conversation.CommandStopDetectingData());

        KillProccessCommand = new Command((_) => conversation.CommandKill());
        CreateProccessCommand = new Command((_) => conversation.CommandCreate());
    }
}

