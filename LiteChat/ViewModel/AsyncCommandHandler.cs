using System.Windows.Input;

namespace LiteChat.ViewModel;

internal class AsyncCommandHandler : ICommand
{
    private readonly Func<Task> _asyncAction;
    private readonly Func<bool> _canExecute;

    public AsyncCommandHandler(Func<Task> asyncAction, Func<bool> canExecute = null)
    {
        _asyncAction = asyncAction;
        _canExecute = canExecute ?? (() => true);
    }

    public bool CanExecute(object parameter) => _canExecute.Invoke();

    public async void Execute(object parameter)
    {
        if (CanExecute(parameter) && _asyncAction != null)
        {
            await ExecuteAsync();
        }
    }

    private async Task ExecuteAsync()
    {
        await _asyncAction.Invoke();
    }


    public event EventHandler CanExecuteChanged;
}