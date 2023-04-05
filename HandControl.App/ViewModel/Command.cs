using System;
using System.Windows.Input;

namespace HandControl.App.ViewModel;

public class Command : ICommand
{
    private Action<object> _execute;

    public Command(Action<object> execute) 
    {
        _execute = execute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        _execute?.Invoke(parameter!);
    }
}

