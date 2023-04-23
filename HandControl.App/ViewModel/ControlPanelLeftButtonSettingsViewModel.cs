﻿using HandControl.App.Model;
using System;

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
        get => SingleManager.MouseHandController.LeftTriggerHisterezis;
        set 
        {
            SingleManager.MouseHandController.LeftTriggerHisterezis = value;
            OnPropertyChanged(nameof(TriggerHisterezis));
        }
    }

    public Mark[] Items => (Mark[])Enum.GetValues(typeof(Mark));

    public int SelectedIndexTrigger1
    {
        get => (int)_controller.LeftButtonTrigger1;
        set
        {
            _controller.LeftButtonTrigger1 = (Mark)value;
            OnPropertyChanged(nameof(SelectedIndexTrigger1));
        }
    }

    public int SelectedIndexTrigger2
    {
        get => (int)_controller.LeftButtonTrigger2;
        set
        {
            _controller.LeftButtonTrigger2 = (Mark)value;
            OnPropertyChanged(nameof(SelectedIndexTrigger2));
        }
    }
}


public class ControlPanelRightButtonSettingsViewModel : ControlPanelBaseViewModel
{
    MouseHandController _controller = SingleManager.MouseHandController;

    public ControlPanelRightButtonSettingsViewModel()
    {
        Title = "Правая кнопка мыши";
    }

    public float TriggerHisterezis
    {
        get => SingleManager.MouseHandController.RightTriggerHisterezis;
        set
        {
            SingleManager.MouseHandController.RightTriggerHisterezis = value;
            OnPropertyChanged(nameof(TriggerHisterezis));
        }
    }
    public Mark[] Items => (Mark[])Enum.GetValues(typeof(Mark));

    public int SelectedIndexTrigger1
    {
        get => (int)_controller.RightButtonTrigger1;
        set
        {
            _controller.RightButtonTrigger1 = (Mark)value;
            OnPropertyChanged(nameof(SelectedIndexTrigger1));
        }

    }
    public int SelectedIndexTrigger2
    {
        get => (int)_controller.RightButtonTrigger2;
        set
        {
            _controller.RightButtonTrigger2 = (Mark)value;
            OnPropertyChanged(nameof(SelectedIndexTrigger2));
        }
    }
}

