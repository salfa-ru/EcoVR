using HandControl.App.Model;
using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace HandControl.App.ViewModel;

public class ControlPanelLeftButtonSettingsViewModel : ControlPanelBaseViewModel
{
    MouseHandController _controller = SingleManager.MouseHandController;

    public ControlPanelLeftButtonSettingsViewModel()
    {
        Title = "Левая кнопка мыши";
    }

    public float TriggerHisterezis
    {
        get => SingleManager.MouseHandController.LeftTrigger.Histerezis;
        set 
        {
            SingleManager.MouseHandController.LeftTrigger.Histerezis = value;
            OnPropertyChanged(nameof(TriggerHisterezis));
        }
    }

    public List<string> ItemsFingers => SingleManager.MouseHandController.LeftTrigger.Fingers;
    public List<string> ItemsTargets => SingleManager.MouseHandController.LeftTrigger.TriggerTarget;

    public int SelectedIndexFinger
    {
        get => SingleManager.MouseHandController.LeftTrigger.SelectedFinger;
        set
        {
            SingleManager.MouseHandController.LeftTrigger.SelectedFinger = value;
            OnPropertyChanged(nameof(SelectedIndexFinger));
        }
    }

    public int SelectedIndexTarget
    {
        get => SingleManager.MouseHandController.LeftTrigger.SelectedTarget;
        set
        {
            SingleManager.MouseHandController.LeftTrigger.SelectedTarget = value;
            OnPropertyChanged(nameof(SelectedIndexTarget));
        }
    }
}