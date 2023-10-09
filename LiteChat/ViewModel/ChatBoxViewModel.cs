using LiteChat.Model;
using System.Collections.ObjectModel;

namespace LiteChat.ViewModel;

public class ChatBoxViewModel : BaseViewModel
{
    public ObservableCollection<MessageViewModel> Messages { get; set; } = new ObservableCollection<MessageViewModel>();
    public ChatBoxViewModel()
    {
        _userName = "Morteza";
        Messages.Add(new MessageViewModel(new Message() { Text = $"morteza and alice started to chat", Timestamp = DateTime.Now }));

        for (int i = 0; i < 10; i++)
        {
            Messages.Add(new MessageViewModel(new Message() { Text = $"Hello ! How are you! {i}", From = _userName, To = "Alice", Timestamp = DateTime.Now }));
            Messages.Add(new MessageViewModel(new Message() { Text = $"Hey!! I'm Great :) {i}", From = "Alice", To = _userName, Timestamp = DateTime.Now }));
        }
        Label l1 = new Label();


    }
    public string UserId
    {
        get => _userId;
        set => SetField(ref _userId, value);
    }
    private string _userId;

    public string UserName
    {
        get => _userName;
        set => SetField(ref _userName, value);
    }
    private string _userName;
}
