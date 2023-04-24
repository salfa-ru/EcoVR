using HandControl.App.Model;
using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace HandControl.App.ViewModel;

public class ControlPanelRightButtonSettingsViewModel : ControlPanelBaseViewModel
{
    MouseHandController _controller = SingleManager.MouseHandController;

    public ControlPanelRightButtonSettingsViewModel()
    {
        Title = "Правая кнопка мыши";
    }

    public float TriggerHisterezis
    {
        get => SingleManager.MouseHandController.RightTrigger.Histerezis;
        set
        {
            SingleManager.MouseHandController.RightTrigger.Histerezis = value;
            OnPropertyChanged(nameof(TriggerHisterezis));
        }
    }
    public List<string> ItemsFingers => SingleManager.MouseHandController.RightTrigger.Fingers;
    public List<string> ItemsTargets => SingleManager.MouseHandController.RightTrigger.TriggerTarget;

    public int SelectedIndexFinger
    {
        get => SingleManager.MouseHandController.RightTrigger.SelectedFinger;
        set
        {
            SingleManager.MouseHandController.RightTrigger.SelectedFinger = value;
            OnPropertyChanged(nameof(SelectedIndexFinger));
        }

    }
    public int SelectedIndexTarget
    {
        get => SingleManager.MouseHandController.RightTrigger.SelectedTarget;
        set
        {
            SingleManager.MouseHandController.RightTrigger.SelectedTarget = value;
            OnPropertyChanged(nameof(SelectedIndexTarget));
        }
    }
}

