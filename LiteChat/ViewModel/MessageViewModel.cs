using LiteChat.Model;

namespace LiteChat.ViewModel;

public class MessageViewModel : BaseViewModel
{


    public MessageViewModel(Message message)
    {
        _userName = "Morteza";
        _text = message.Text;
        _horizontal = message.From == _userName ? new LayoutOptions(LayoutAlignment.End,true)
            : message.To == _userName ? new LayoutOptions(LayoutAlignment.Start, true)
            : new LayoutOptions(LayoutAlignment.Center, true);
    }
    public string UserName
    {
        get => _userName;
        set => SetField(ref _userName, value);
    }
    private string _userName;
    public string Text
    {
        get => _text;
        set => SetField(ref _text, value);
    }
    private string _text;
    public LayoutOptions Horizontal
    {
        get => _horizontal;
        set => SetField(ref _horizontal, value);
    }
    private LayoutOptions _horizontal;
}
